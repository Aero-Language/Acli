namespace ACLI;

public record Argument(string[] Names, Action<Cli, PassedArg[]> Command);
public record SuperArgument(string[] Names, Action<Cli, PassedArg[]> Command, Argument[]? SubArgs = null) : Argument(Names, Command);