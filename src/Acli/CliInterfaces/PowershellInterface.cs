namespace Acli.CliInterfaces;

public sealed record PowershellInterface : CliInterface
{
    public override bool CanParamMerge => false;

    public override string? CleanParam(string param)
    {
        if (!param.StartsWith('-')) return null; // Not a flag
        return param[1..].ToLower(); // Return the parameter
    }
    public override string[]? CleanParams(string param) => null;
}