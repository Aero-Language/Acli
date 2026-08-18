namespace ACLI;

public enum FlagPrefixType
{
    /// <summary>
    /// DotNet style tooling
    /// </summary>
    None,
    
    /// <summary>
    /// Unix/BSD style | -b  --build  --some-long-thing
    /// </summary>
    Dash,
    
    /// <summary>
    /// Windows(PowerShell) style | -B  -Build
    /// </summary>
    StrictSingleDash,
    
    /// <summary>
    /// Traditional Windows(CMD) style | /b  /build
    /// </summary>
    Slash
}