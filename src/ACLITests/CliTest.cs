using ACLI;
using Xunit;

namespace ACLITests;

public class CliTest
{
    [Fact]
    public void ArgTest()
    {
        // Arrange
        PassedArg[] buildParams = [];
        PassedArg[] runParams = [];
        PassedArg[] helpParams = [];
        
        IEnumerable<SuperArgument> actions =
        [
            new(["b", "build"], (_, parameters) => { buildParams = parameters; } ),
            new(["r", "run"], (_, parameters) => { runParams = parameters; } ),
            new(["h", "help"], (_, parameters) => { helpParams = parameters; } ),
        ];
        
        var cliProperties = new CliProperties(actions, ConsoleStreams.Dummy);
        var cli = new Cli(cliProperties);
        
        
        // Act
        cli.Start("--build", "test.aero", "run.aero");

        // Assert
        Assert.Equal<string[]>(["test.aero", "run.aero"], buildParams.SelectMany(a => a.Values).ToArray());
        Assert.Equal<string[]>([], runParams.SelectMany(a => a.Values).ToArray());
        Assert.Equal<string[]>([], helpParams.SelectMany(a => a.Values).ToArray());
    }
}