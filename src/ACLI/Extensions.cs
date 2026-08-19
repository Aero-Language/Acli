namespace ACLI;

internal static class Extensions
{
    public static string WithoutPrefix(this string str)
        => str.Remove(0, str.PrefixOnly().Length);
    
    public static string PrefixOnly(this string str)
    {
        var firstChar = str[0];
        var secondChar = str[1];

        if (firstChar == '-' && secondChar == '-')
            return "--";
        return firstChar.ToString();
    }
    
    public static string ExplainString(this FlagPrefixType prefix)
        => prefix switch
        {
            FlagPrefixType.None => "''",
            FlagPrefixType.Dash => "'-' or '--'",
            FlagPrefixType.Slash => "'/'",
            FlagPrefixType.StrictSingleDash => "'-'",
            _ => throw new NotImplementedException()
        };
    
    public static bool StartsWith(this string str, FlagPrefixType prefix)
        => prefix switch
        {
            // Handles SingleLetter and MultiLetter parameters
            FlagPrefixType.Dash => str.StartsWith("-") && str.Length == 2 || str.StartsWith("--"),
            FlagPrefixType.StrictSingleDash => str.StartsWith("-"),
            FlagPrefixType.Slash => str.StartsWith("/"),
            FlagPrefixType.None => !str.StartsWith("-") && !str.StartsWith("/"), // Make sure it doesn't start with any other prefix
            _ => false
        };
}