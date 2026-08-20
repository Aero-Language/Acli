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
            // Check for any arguments and throw if none are found
            if (args.Length <= 0)
            {
                throw properties.NoArgumentsError();
            }
            
            int argCursor = 0;
            var flag = ValidateFlag(args[argCursor]);
                    
            // If the first flag is any of the super-args, parse them, otherwise throw
            if (properties.Arguments.TryGetValue(flag, out var arg) && arg is SuperArgument super)
            {
                argCursor++;
                var parameters = new List<PassedArg>();
                
                // Get all the parameters that follow the flag
                while (args.Length > argCursor)
                {
                    var nextFlag = args[argCursor].WithoutPrefix();
                    
                    // When the flag is a sub-arg
                    if (properties.Arguments.ContainsKey(nextFlag))
                    {
                        var passedValues = new List<string>();
                        
                        while (args.Length > argCursor && !properties.Arguments.ContainsKey(args[argCursor].WithoutPrefix()))
                        {
                            passedValues.Add(args[argCursor]);
                            argCursor++;
                        }
                        
                        parameters.Add(new PassedArg(ValidateFlag(nextFlag), passedValues.ToArray()));
                        argCursor++;
                    }
                    else // The flag is a value
                    {
                        var passedValues = new List<string>();
                        
                        while (args.Length > argCursor && !properties.Arguments.ContainsKey(args[argCursor]))
                        {
                            passedValues.Add(args[argCursor]);
                            argCursor++;
                        }
                        
                        parameters.Add(new PassedArg(flag, passedValues.ToArray()));
                        argCursor++;
                    }
                }
                
                super.Command(this, parameters.ToArray());
            }
            else
            {
                throw properties.IncorrectSuperArgError(flag);
            }
#if RELEASE
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
#endif

        string ValidateFlag(string str)
        {
            // If it's not the expected prefix, throw
            if (!str.StartsWith(properties.PrefixType))
            {
                throw properties.IncorrectPrefixError(properties.PrefixType.ExplainString(), str.PrefixOnly());
            }
            
            // Return the string without the prefix
            return str.Remove(0, str.PrefixOnly().Length);
        }
    }
    
    public void Print(string text)
        => properties.Output.Write(text);
    public void PrintLn(string text)
        => properties.Output.WriteLine(text);
    public void Error(string error)
        => properties.Error.WriteLine(error);

    
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