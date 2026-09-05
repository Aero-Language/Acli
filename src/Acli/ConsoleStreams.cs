namespace ACLI;

public record ConsoleStreams(StreamReader Input, StreamWriter Output, StreamWriter Error)
{
    /// <summary>
    /// Constructs a ConsoleStreams collection with default Console.OpenStandard*() methods.
    /// </summary>
    public static ConsoleStreams Default => new(new(Console.OpenStandardInput()), new(Console.OpenStandardOutput()) {AutoFlush = true}, new(Console.OpenStandardError()) {AutoFlush = true});
}