namespace Acli;

public class Cli(CliProperties properties)
{
    public void Start(string[] args)
    {
#if RELEASE
        try
        {
#endif
            // Make sure there is at least one Argument
            if (args.Length < 1) throw new ArgumentException(nameof(args));

            List<Flag> flags = [];
            List<string> values = [];
            foreach (var arg in args)
            {
                if (properties.Interface.CanParamMerge)
                {
                    var clean = properties.Interface.CleanParams(arg);
                    if (clean != null)
                    {
                        foreach (var str in clean)
                        {
                            var flag = properties.Flags.FirstOrDefault(f => f.Names.Contains(str));
                            if (flag != null) flags.Add(flag);
                            else values.Add(arg);
                        }
                    }
                    else values.Add(arg);
                }
                else
                {
                    var clean = properties.Interface.CleanParam(arg);
                    if (clean != null)
                    {
                        var flag = properties.Flags.FirstOrDefault(f => f.Names.Contains(clean));
                        if (flag != null)
                        {
                            flags.Add(flag);
                            continue;
                        }
                    }

                    values.Add(arg);
                }
            }

            // Enforce singleFlag rule
            if (properties.SingleFlag)
            {
                if (flags.Count > 1) throw new ArgumentException(nameof(args));

                if (flags.SingleOrDefault(f => f is CommandFlag) is CommandFlag exec)
                {
                    exec.Command.Invoke(flags.ToArray(), values.ToArray());
                }
                else
                {
                    throw new ArgumentException(nameof(flags));
                }
            }
            else
            {
                var execs = flags.OfType<CommandFlag>().ToArray();
                if (execs.Length > 0)
                {
                    foreach (var exec in execs) exec.Command.Invoke(flags.ToArray(), values.ToArray());
                }
                else
                {
                    throw new ArgumentException(nameof(flags));
                }
            }
#if RELEASE
        }
        catch (Exception ex)
        {
            Error(properties.CompactExceptions ? ex.Message : ex.ToString());
        }
#endif
    }

    public string? ReadLine() => properties.Streams.Input.ReadLine();
    public string ReadAll() => properties.Streams.Input.ReadToEnd();
    public char Read() => (char)properties.Streams.Input.Read();


    // Object logs
    public void Log(string line)
    {
        properties.Streams.Output.Write(line);
        if (!properties.Streams.Output.AutoFlush) properties.Streams.Output.Flush();
    }
    public void Log<T>(T obj) => Log(obj?.ToString() ?? "");
    public void LogLine(string line)
    {
        properties.Streams.Output.WriteLine(line);
        if (!properties.Streams.Output.AutoFlush) properties.Streams.Output.Flush();
    }
    public void LogLine<T>(T obj) => LogLine(obj?.ToString() ?? "");
    // Array Logs
    public void Log(string[] elements) => Log($"[{string.Join(", ", elements)}]");
    public void Log<T>(T[] objs) => Log(objs.Select(o => o?.ToString() ?? "").ToArray());
    public void LogLine(string[] elements) => LogLine($"[{string.Join(", ", elements)}]");
    public void LogLine<T>(T[] objs) => LogLine(objs.Select(o => o?.ToString() ?? "").ToArray());
    
    
    // Object Errors
    public void Error(string line)
    {
        properties.Streams.Error.Write(line);
        if (!properties.Streams.Error.AutoFlush) properties.Streams.Error.Flush();
    }
    public void Error<T>(T obj) => Error(obj?.ToString() ?? "");
    public void ErrorLine(string line)
    {
        properties.Streams.Error.WriteLine(line);
        if (!properties.Streams.Error.AutoFlush) properties.Streams.Error.Flush();
    }
    public void ErrorLine<T>(T obj) => ErrorLine(obj?.ToString() ?? "");
    // Array Errors
    public void Error(string[] elements) => Error($"[{string.Join(", ", elements)}]");
    public void Error<T>(T[] objs) => Error(objs.Select(o => o?.ToString() ?? "").ToArray());
    public void ErrorLine(string[] elements) => ErrorLine($"[{string.Join(", ", elements)}]");
    public void ErrorLine<T>(T[] objs) => ErrorLine(objs.Select(o => o?.ToString() ?? "").ToArray());
}