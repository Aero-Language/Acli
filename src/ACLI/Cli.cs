namespace ACLI;

public class Cli
{
    private CliProperties _properties;
    
    public Cli(CliProperties properties)
    {
        _properties = properties;
    }
    
    public void Start(string[] args)
    {
#if RELEASE
        try
        {
#endif
            // Check for any arguments and display a help-message if none are found
            if (args.Length <= 0)
            {
                Console.WriteLine("No options specified.");
                if (!string.IsNullOrEmpty(_properties.HelpMessage))
                {
                    Console.WriteLine(_properties.HelpMessage);
                }
                Console.WriteLine("Quitting...");
                return;
            }
            
            int flagCursor = 0;
            var flags = new Dictionary<int, (string Value, FlagType Type)>();

            // Validate the flags and assign them a type
            // to differentiate between a flag like 'build'
            // and a parameter like 'test.aero'
            for (int index = 0; index < args.Length; index++)
            {
                var arg = args[index];

                flags.Add(index,
                    _properties.Options.TryGetValue(arg, out var option)
                        ? (option, FlagType.Flag)
                        : (arg, FlagType.Parameter));
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

                        if (_properties.Actions.TryGetValue(flag.Value, out var action))
                        {
                            // Invoke the action for the specific flag
                            action.Invoke(parameters.ToArray()); 
                        }
                    }
                }
                else
                {
                    throw new Exception("Something went wrong ): ...");
                }
            }
#if RELEASE
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
#endif
    }
}