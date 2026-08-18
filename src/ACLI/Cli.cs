namespace ACLI;

public class Cli(CliProperties properties) : IDisposable
{
    public void Start(params string[] args)
    {
#if RELEASE
        try
        {
#endif
            // Check for any arguments and throw if none are found
            if (args.Length <= 0)
            {
                throw properties.NoArgumentsError();
            }
            
            int flagCursor = 0;
            var flags = new Dictionary<int, (string Value, FlagType Type)>();

            // Validate the flags and assign them a type
            // to differentiate between a flag like 'build'
            // and a parameter like 'test.aero'
            for (int index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                var pureArg = arg;
                
                if (properties.PrefixType == FlagPrefixType.Dash)
                {
                    bool isDoubleDash = arg.Length > 2;
                    pureArg = arg.Remove(0, 1);
                    
                    if (isDoubleDash)
                    {
                        // remove the remaining '-'
                        pureArg = pureArg.Remove(0, 1);
                    }
                }
                else if (properties.PrefixType is FlagPrefixType.None)
                {
                    // Do nothing here because there is no Prefix
                }
                else
                {
                    // Remove the first character otherwise | /build, -Build
                    pureArg = arg.Remove(0, 1);
                }

                var isFlag = properties.Options.TryGetValue(pureArg, out var flag);

                if (isFlag)
                {
                    // Checks if the pureArg still starts with a '-', meaning '--' was used for single-letter
                    if (properties.PrefixType is FlagPrefixType.Dash && pureArg.StartsWith("-")) 
                        throw properties.DashPrefixError();
                    
                    // Check if the flag prefix is correct IF it is a flag and not a parameter
                    if (!arg.StartsWith(properties.PrefixType))
                        throw properties.IncorrectPrefixError(properties.PrefixType.ExplainString(), pureArg.PrefixOnly());
                }
                
                
                
                flags.Add(index, isFlag
                        ? (flag!, FlagType.Flag)
                        : (arg, FlagType.Parameter) // Important! Use original, unmodified arg here
                );
            }

            // Check if multiple flags are used when only a single one is allowed.
            var flagCount = flags.Count(f => f.Value.Type == FlagType.Flag);
            if (properties.SingleFlagOnly && flagCount > 1)
            {
                throw properties.TooManyArgumentsError(flagCount);
            }
            
            // Execute the flags
            while (flags.Count > flagCursor)
            {
                if (flags.TryGetValue(flagCursor, out var flag))
                {
                    flagCursor++;
                    
                    if (flag.Type is FlagType.Flag)
                    {
                        var parameters = new List<string>();
                        
                        // Get all the parameters that follow the flag
                        while (flags.TryGetValue(flagCursor, out var nextFlag) && nextFlag.Type is FlagType.Parameter)
                        {
                            parameters.Add(nextFlag.Value);
                            flagCursor++;
                        }

                        if (properties.Actions.TryGetValue(flag.Value, out var action))
                        {
                            // Invoke the action for the specific flag
                            action.Invoke(this, parameters.ToArray());
                        }
                    }
                }
                else
                {
                    throw properties.DefaultError();
                }
            }
#if RELEASE
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
#endif
    }

    
    public void Print(string text)
        => properties.Output.Write(text);
    public void PrintLn(string text)
        => properties.Output.WriteLine(text);
    public void Error(string error)
        => properties.Error.WriteLine(error);

    
    public char Read()
    {
        var read = properties.Input.Read();
        return read == -1 ? '\n' : (char)read;
    }
    public string ReadLn()
        => properties.Input.ReadLine() ?? "\n";
    public string ReadAll()
        => properties.Input.ReadToEnd();


    public void Dispose()
    {
        properties.Dispose();
    }
}