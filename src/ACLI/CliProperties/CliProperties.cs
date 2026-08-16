namespace ACLI;

public record CliProperties
{
    public CliProperties(IEnumerable<string> options, IDictionary<string, Action<string[]>> actions)
    {
        var opts = options.ToArray();
        
        // Only accept if options contains at least one element
        if (opts.Length <= 0)
        {
            throw new ArgumentException("options must contain at least one option.", nameof(options));
        }

        // Only accept if actions only have valid keys
        
        Options = new HashSet<string>(opts);
        Actions = actions;
    }
    
    public readonly HashSet<string> Options;
    public readonly IDictionary<string, Action<string[]>> Actions;
    
    public string HelpMessage { get; init; } = "";
    public FlagDashType DashType { get; init; } = FlagDashType.NoDash;
}