using Xunit;

namespace ACLI.Tests;

public class CliTest
{
    [Fact]
    public void FullTest()
    {
        // Arrange
        int buildExecuted = 0;
        string[] buildParams = [];
        int runExecuted = 0;
        string[] runParams = [];
        int helpExecuted = 0;
        string[] helpParams = [];
        
        IEnumerable<string> options = ["build", "run", "help"];
        Dictionary<string, Action<string[]>> actions = new Dictionary<string, Action<string[]>>()
        {
            { "build", (parameters) => { buildExecuted += 1; buildParams = parameters; } },
            { "run", (parameters) => { runExecuted += 1; runParams = parameters; } },
            { "help", (parameters) => { helpExecuted += 1; helpParams = parameters; } },
        };
        
        var cliProperties = new CliProperties(options, actions);
        var cli = new Cli(cliProperties);
        
        
        // Act
        cli.Start(["build", "test.aero", "run.aero", "run", "help", "run"]);


        // Assert
        Assert.True(buildExecuted == 1);
        Assert.True(runExecuted == 2);
        Assert.True(helpExecuted == 1);
        
        Assert.Equal<string[]>(["test.aero", "run.aero"], buildParams);
        Assert.Equal<string[]>([], runParams);
        Assert.Equal<string[]>([], helpParams);
    }
}