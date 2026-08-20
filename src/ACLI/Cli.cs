using System.Diagnostics.CodeAnalysis;

namespace ACLI;

public class Cli(CliProperties properties) : IDisposable
{
    public void Start(params string[] args)
    {
#if RELEASE
        try
        {
#endif
            if (args.Length <= 0)
            {
                throw properties.NoArgumentsError();
            }
            
            int argCursor = 0;
            var initialRawFlag = args[argCursor];
            var initialFlag = StripAndValidatePrefix(initialRawFlag);
                    
            if (properties.Arguments.TryGetValue(initialFlag, out var arg) && arg is SuperArgument super)
            {
                argCursor++;
                var parameters = new List<PassedArg>();
                
                while (args.Length > argCursor)
                {
                    var currentRaw = args[argCursor];
                    var currentClean = currentRaw.StartsWith(properties.PrefixType) 
                        ? StripAndValidatePrefix(currentRaw) 
                        : currentRaw;

                    if (properties.Arguments.ContainsKey(currentClean))
                    {
                        var flagName = currentClean;
                        argCursor++;
                        var passedValues = new List<string>();
                        
                        while (args.Length > argCursor)
                        {
                            var candidateRaw = args[argCursor];
                            var candidateClean = candidateRaw.StartsWith(properties.PrefixType) 
                                ? StripAndValidatePrefix(candidateRaw) 
                                : candidateRaw;

                            if (properties.Arguments.ContainsKey(candidateClean))
                            {
                                break; // Stop collecting values when hitting the next flag/argument
                            }

                            passedValues.Add(candidateRaw);
                            argCursor++;
                        }
                        
                        parameters.Add(new PassedArg(flagName, passedValues.ToArray()));
                    }
                    else
                    {
                        // Positional values belonging directly to the super argument
                        var passedValues = new List<string>();
                        
                        while (args.Length > argCursor)
                        {
                            var candidateRaw = args[argCursor];
                            var candidateClean = candidateRaw.StartsWith(properties.PrefixType) 
                                ? StripAndValidatePrefix(candidateRaw) 
                                : candidateRaw;

                            if (properties.Arguments.ContainsKey(candidateClean))
                            {
                                break;
                            }

                            passedValues.Add(candidateRaw);
                            argCursor++;
                        }
                        
                        parameters.Add(new PassedArg(initialFlag, passedValues.ToArray()));
                    }
                }
                
                super.Command(this, parameters.ToArray());
            }
            else
            {
                throw properties.IncorrectSuperArgError(initialFlag);
            }
#if RELEASE
        }
        catch (Exception e)
        {
            Error(e.ToString());
        }
#endif
    }

    private string StripAndValidatePrefix(string str)
    {
        if (!str.StartsWith(properties.PrefixType))
        {
            throw properties.IncorrectPrefixError(properties.PrefixType.ExplainString(), str.PrefixOnly());
        }
        
        return str.Remove(0, str.PrefixOnly().Length);
    }
    
    public void Print(string text)
    {
        properties.Output.Write(text);
        properties.Output.Flush();
    }

    public void PrintLn(string text)
    {
        properties.Output.WriteLine(text);
        properties.Output.Flush();
    }

    public void Error(string error)
    {
        properties.Error.WriteLine(error);
        properties.Error.Flush();
    }

    
    public char Read() 
    {
        var read = properties.Input.Read();
        return read == -1 ? '\n' : (char)read;
    }
    public string ReadLn()
        => properties.Input.ReadLine() ?? "\n";
    public string ReadAll()
        => properties.Input.ReadToEnd();
    public bool IsReadEnd => properties.Input.EndOfStream;


    public void Dispose()
    {
        properties.Dispose();
    }
}