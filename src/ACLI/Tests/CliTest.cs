using Xunit;

namespace ACLI.Tests;

public class CliTest
{
    [Fact]
    public void ArgTest()
    {
        // Arrange
        string[] buildParams = [];
        string[] runParams = [];
        string[] helpParams = [];
        
        IEnumerable<(string[], Action<string[]>)> actions =
        [
            ( ["b", "build"], (parameters) => { buildParams = parameters; } ),
            ( ["r", "run"], (parameters) => { runParams = parameters; } ),
            ( ["h", "help"], (parameters) => { helpParams = parameters; } ),
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