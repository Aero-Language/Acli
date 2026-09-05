namespace Acli;

public abstract record Flag
{
    protected Flag() { }
    public abstract string[] Names { get;  init; }
}

public abstract record Flag<TFlag>(string[] Names) : Flag where TFlag : Flag<TFlag>, new()
{
    public static readonly TFlag Instance = new();
}

public abstract record CommandFlag : Flag
{
    protected CommandFlag(string[] names) : base() { Names = names; }
    public abstract Action<Flag[], string[]> Command { get; init; }
}

public abstract record CommandFlag<TFlag>(string[] Names, Action<Flag[], string[]> Command) : CommandFlag(Names) where TFlag : CommandFlag<TFlag>, new()
{
    public static readonly TFlag Instance = new();
}