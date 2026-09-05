namespace Acli.CliInterfaces;

public abstract record CliInterface
{
    internal CliInterface() { }

    public virtual bool IsCaseSensitive => true;
    public abstract bool CanParamMerge { get; }
    
    public abstract string? CleanParam(string param);
    public abstract string[]? CleanParams(string param);
}