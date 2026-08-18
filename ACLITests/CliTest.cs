using ACLI;
using Xunit;

namespace ACLITests;

public class CliTest
{
    [Fact]
    public void ArgTest()
    {
        // Arrange
        string[] buildParams = [];
        string[] runParams = [];
        string[] helpParams = [];
        
        IEnumerable<(string[], Action<Cli, string[]>)> actions =
        [
            ( ["b", "build"], (cli, parameters) => { buildParams = parameters; } ),
            ( ["r", "run"], (cli, parameters) => { runParams = parameters; } ),
            ( ["h", "help"], (cli, parameters) => { helpParams = parameters; } ),
        ];
        
        var cliProperties = new CliProperties(actions, ConsoleStreams.Dummy);
        var cli = new Cli(cliProperties);
        
        
        // Act
        cli.Start("--build", "test.aero", "run.aero");
        cli.Start("-r");
        cli.Start("--help");


        // Assert
        Assert.Equal<string[]>(["test.aero", "run.aero"], buildParams);
        Assert.Equal<string[]>([], runParams);
        Assert.Equal<string[]>([], helpParams);
    }
}