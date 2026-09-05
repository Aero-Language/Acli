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

    public string? ReadLine() => properties.InputStream.ReadLine();
    public string ReadAll() => properties.InputStream.ReadToEnd();
    public char Read() => (char)properties.InputStream.Read();
    
    
    public void LogLine(string line) => properties.LogStream.WriteLine(line);
    public void Log(string line) => properties.LogStream.Write(line);
    public void Log(string[] elements) => properties.LogStream.Write($"[{string.Join(", ", elements)}]");
    public void Log<T>(T obj) => properties.LogStream.Write(obj?.ToString());
    public void Log<T>(T[] objs) => properties.LogStream.Write($"[{string.Join(", ", objs.Select(obj => obj?.ToString()))}]");
    
    public void ErrorLine(string line) => properties.ErrorStream.WriteLine(line);
    public void Error(string line) => properties.ErrorStream.Write(line);
}