namespace Acli.CliInterfaces;

public sealed record PrefixlessInterface : CliInterface
{
    public override bool IsCaseSensitive => false;
    public override bool CanParamMerge => false;

    public override string? CleanParam(string param) => param;
    public override string[]? CleanParams(string param) => null;
}