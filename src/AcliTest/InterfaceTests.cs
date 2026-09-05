using Acli;
using Acli.CliInterfaces;

namespace AcliTest;

public class InterfaceTests
{
    private record BuildFlag() : CommandFlag<BuildFlag>(["b", "build"], Build);

    private record VerboseFlag() : Flag<VerboseFlag>(["v", "verbose"]);
    private record RunFlag() : CommandFlag<BuildFlag>(["r", "run"], Run);
    
    static readonly Flag[] Flags =
    [
        BuildFlag.Instance,
        VerboseFlag.Instance,
        RunFlag.Instance
    ];

    static bool _buildExecuted = false;
    static string[] _buildValues = [];
    static Flag[] _buildFlags = [];
    static void Build(Flag[] flags, string[] values)
    {
        _buildExecuted = true;
        _buildValues = values;
        _buildFlags = flags;
    }
    
    static bool _runExecuted = false;
    static void Run(Flag[] flags, string[] values)
    {
        _runExecuted = true;
    }
    
    
    [Fact]
    public void DosTest()
    {
        // Arrange
        var properties = new CliProperties(Flags) { Interface = new DosInterface() };
        var cli = new Cli(properties);
        string[] parameters = ["/b",  "test.aero"];
        
        // Act
        cli.Start(parameters);
        
        
        // Assert
        Assert.True(_buildExecuted);
        Assert.False(_runExecuted);
        Assert.Equal(["test.aero"], _buildValues);
        Assert.Equal([BuildFlag.Instance, VerboseFlag.Instance], _buildFlags);
        
        
        // Reset
        _buildExecuted = false;
        _runExecuted = false;
        _buildValues = [];
        _buildFlags = [];
    }
    [Fact]
    public void PosixTest()
    {
        // Arrange
        var properties = new CliProperties(Flags) { Interface = new PosixInterface() };
        var cli = new Cli(properties);
        string[] parameters = ["-bv", "test.aero"];
        
        // Act
        cli.Start(parameters);
        
        
        // Assert
        Assert.True(_buildExecuted);
        Assert.False(_runExecuted);
        Assert.Equal(["test.aero"], _buildValues);
        Assert.Equal([BuildFlag.Instance, VerboseFlag.Instance], _buildFlags);
        
        
        // Reset
        _buildExecuted = false;
        _runExecuted = false;
        _buildValues = [];
        _buildFlags = [];
    }
    [Fact]
    public void PowershellTest()
    {
        // Arrange
        var properties = new CliProperties(Flags) { Interface = new PowershellInterface() };
        var cli = new Cli(properties);
        string[] parameters = ["-b", "-v", "test.aero"];
        
        // Act
        cli.Start(parameters);
        
        
        // Assert
        Assert.True(_buildExecuted);
        Assert.False(_runExecuted);
        Assert.Equal(["test.aero"], _buildValues);
        Assert.Equal([BuildFlag.Instance, VerboseFlag.Instance], _buildFlags);
        
        
        // Reset
        _buildExecuted = false;
        _runExecuted = false;
        _buildValues = [];
        _buildFlags = [];
    }
    [Fact]
    public void PrefixlessTest()
    {
        // Arrange
        var properties = new CliProperties(Flags) { Interface = new PrefixlessInterface() };
        var cli = new Cli(properties);
        string[] parameters = ["b", "v", "test.aero"];
        
        // Act
        cli.Start(parameters);
        
        
        // Assert
        Assert.True(_buildExecuted);
        Assert.False(_runExecuted);
        Assert.Equal(["test.aero"], _buildValues);
        Assert.Equal([BuildFlag.Instance, VerboseFlag.Instance], _buildFlags);
        
        
        // Reset
        _buildExecuted = false;
        _runExecuted = false;
        _buildValues = [];
        _buildFlags = [];
    }
}
