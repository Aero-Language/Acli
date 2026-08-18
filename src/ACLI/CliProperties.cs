namespace ACLI;

public record CliProperties : IDisposable
{
    public CliProperties(IEnumerable<(string[] flags, Action<Cli, string[]> action)> actions, ConsoleStreams streams)
    {
        var distinctActions = actions.Distinct().ToArray();
        var options = distinctActions.Select(t => t.flags).SelectMany(action => action).ToArray();
        
        // Only accept if options contains at least one element
        if (options.Length <= 0)
        {
            throw new ArgumentException("Options must contain at least one option.", nameof(options));
        }

        // Only accept if actions only have valid keys
        Options = new HashSet<string>(options);
        Actions = distinctActions
            .SelectMany(item => item.flags.Select(flag => new { flag, item.action }))
            .ToDictionary(x => x.flag, x => x.action);
        
        // Make StreamReader/Writer for the standard streams
        Input = new StreamReader(streams.Input);
        Output = new StreamWriter(streams.Output);
        Error = new StreamWriter(streams.Error);
    }
    
    public readonly HashSet<string> Options;
    public readonly IDictionary<string, Action<Cli, string[]>> Actions;
    public readonly StreamReader Input;
    public readonly StreamWriter Output;
    public readonly StreamWriter Error;
    
    public FlagPrefixType PrefixType { get; init; } = FlagPrefixType.Dash;
    public bool SingleFlagOnly { get; init; } = true;
    
    
    public Func<int, Exception> TooManyArgumentsError { get; init; } = (amount) => new Exception($"Too many arguments passed, only one flag is allowed. Should be: 1, is: {amount}");
    public Func<Exception> NoArgumentsError { get; init; } = () => new Exception("No arguments passed, use help to get a list of options.");
    public Func<string, string, Exception> IncorrectPrefixError { get; init; } = (expected, actual) => new Exception($"Incorrect prefix. Expected: {expected}, Actual: {actual}");
    public Func<Exception> DashPrefixError { get; init; } = () => new Exception("Incorrect prefix. Should be '--' for multi-letter flags and '-' for single-letter flags.");
    public Func<Exception> DefaultError { get; init; } = () => new Exception($"Something went wrong ): ...");


    public void Dispose()
    {
        Input.Dispose();
        Output.Dispose();
        Error.Dispose();
    }
}