namespace ACLI;

public record ConsoleStreams(Stream Input, Stream Output, Stream Error)
{
    /// <summary>
    /// Constructs a ConsoleStreams collection with default Console.OpenStandard*() methods.
    /// </summary>
    public ConsoleStreams() : this(
        Console.OpenStandardInput(),
        Console.OpenStandardOutput(),
        Console.OpenStandardError()
    ) { }

    public static ConsoleStreams Dummy => new(new DummyStream(), new DummyStream(), new DummyStream());
}