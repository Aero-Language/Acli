namespace Acli.CliInterfaces;

public sealed record PosixInterface : CliInterface
{
    public override bool CanParamMerge => true;

    public override string? CleanParam(string param) => null;

    public override string[]? CleanParams(string param)
    {
        if (!param.StartsWith('-')) return null; // Not a flag
        bool isLong = param.StartsWith("--");
        string raw = isLong ? param[2..] : param[1..];

        if (isLong)
        {
            return [raw];
        }
        else
        {
            return raw.Select(c => c.ToString()).ToArray();
        }
    }
}