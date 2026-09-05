using ACLI;
using Acli.CliInterfaces;

namespace Acli;

public record CliProperties(Flag[] Flags)
{
    public CliInterface Interface { get; init; } = new PosixInterface();
    public bool SingleFlag { get; init; } = false;
    public bool SingleCommandFlag { get; init; } = true;
    public bool CompactExceptions { get; init; } = true;
    
    public ConsoleStreams Streams { get; init; } = ConsoleStreams.Default;
}