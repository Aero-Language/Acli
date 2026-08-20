namespace ACLI;

public record ConsoleStreams(Stream Input, Stream Output, Stream Error)
{
    /// <summary>
    /// Constructs a ConsoleStreams collection with default Console.OpenStandard*() methods.
    /// </summary>
    public static ConsoleStreams Default => new(Console.OpenStandardInput(), Console.OpenStandardOutput(), Console.OpenStandardError());
    
    /// <summary>
    /// Constructs a ConsoleStreams collection with dummy streams
    /// </summary>
    public static ConsoleStreams Dummy => new(new DummyStream(), new DummyStream(), new DummyStream());
}