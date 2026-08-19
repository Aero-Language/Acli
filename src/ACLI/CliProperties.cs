using System.Diagnostics.Contracts;

namespace ACLI;

public record CliProperties : IDisposable
{
    public CliProperties(IEnumerable<SuperArgument> arguments, ConsoleStreams streams)
    {
        // Store args as an array once to avoid enumerating multiple times
        var args = arguments.ToArray();

        // Make sure there is at least one argument
        if (!args.Any())
        {
            throw new ArgumentException("Arguments must contain at least one element.", nameof(arguments));
        }
        
        // Make sure that the SuperArgs and SubArgs of them are valid - no duplicate flags
        if (HasDupes(args))
        {
            throw new ArgumentException("Super-Arguments must have unique names/flags", nameof(arguments));
        }
        if (args.Any(a => a.SubArgs is not null && HasDupes(a.SubArgs)))
        {
            throw new ArgumentException("Sub-Arguments must have unique names/flags", nameof(arguments));
        }

        // Make a Fast dictionary for the args
        var commandLookup = new Dictionary<string, Argument>();

        foreach (var superArg in args)
        {
            // 1. Primary commands indexed by their plain names (e.g., "remote")
            foreach (var name in superArg.Names)
            {
                commandLookup[name] = superArg;
            }

            if (superArg.SubArgs is not null)
            {
                // 2. Sub-args indexed with parent prefix (e.g., "remote:add")
                string primaryParentName = superArg.Names[0];

                foreach (var subArg in superArg.SubArgs)
                {
                    foreach (var subName in subArg.Names)
                    {
                        string compositeKey = $"{primaryParentName}:{subName}";
                        commandLookup[compositeKey] = subArg;
                    }
                }
            }
        }

        Arguments = commandLookup;
        
        // Make StreamReader/Writer for the standard streams
        Input = new StreamReader(streams.Input);
        Output = new StreamWriter(streams.Output);
        Error = new StreamWriter(streams.Error);
    }
    
    public readonly Dictionary<string, Argument> Arguments;
    public readonly StreamReader Input;
    public readonly StreamWriter Output;
    public readonly StreamWriter Error;
    
    public FlagPrefixType PrefixType { get; init; } = FlagPrefixType.Dash;
    
    
    public Func<Exception> NoArgumentsError { get; init; } = () => new Exception("No arguments passed, use help to get a list of options.");
    public Func<string, string, Exception> IncorrectPrefixError { get; init; } = (expected, actual) => new Exception($"Incorrect prefix. Expected: {expected}, Actual: {actual}");
    public Func<Exception> DashPrefixError { get; init; } = () => new Exception("Incorrect prefix. Should be '--' for multi-letter flags and '-' for single-letter flags.");
    public Func<string, Exception> IncorrectSuperArgError { get; init; } = (realArg) => new Exception($"'{realArg}' is not an accepted argument.");
    public Func<Exception> DefaultError { get; init; } = () => new Exception($"Something went wrong ): ...");
    

    public void Dispose()
    {
        Input.Dispose();
        Output.Dispose();
        Error.Dispose();
    }

    // This takes the distinct arguments and then compares if they are the same amount as the raw
    private bool HasDupes(Argument[] args)
        => args.SelectMany(a => a.Names)
               .Distinct()
               .Count() != args.Sum(a => a.Names.Length);
}