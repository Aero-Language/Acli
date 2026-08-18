# ACLI - Advanced Command Line Interface

[![NuGet Version](https://img.shields.io/nuget/v/ACLI.svg?style=flat-square)](https://www.nuget.org/packages/ACLI)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ACLI.svg?style=flat-square)](https://www.nuget.org/packages/ACLI)
[![Framework](https://img.shields.io/badge/framework-.NET%2010.0-purple.svg?style=flat-square)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)

ACLI (Advanced Command Line Interface) is a modern, lightweight, and extensible C# / .NET 10 library designed to make building robust command-line applications straightforward. It provides built-in stream handling, flag prefix validation, positional parameter grouping, and custom exception handling.

---

## Features

- **Intuitive Configuration:** Define flags, aliases, and callback actions using modern C# tuple collections.
- **.NET 10 Ready:** Built targeting .NET 10 for optimal performance and modern C# features.
- **Flexible Prefixes:** Supports Dash (`-` / `--`), Slash (`/`), Custom prefixes, or No prefixes (`None`).
- **Customizable Exceptions:** Tailor error handling for missing arguments, invalid prefixes, or unknown flags using delegate factories.
- **Mockable Streams:** Accepts custom `ConsoleStreams` (or `ConsoleStreams.Dummy`) to streamline unit testing.
- **Automatic Parameter Grouping:** Automatically routes positional parameters following a flag directly into its action delegate.

---

## Installation

Install ACLI using the .NET CLI:

```bash
dotnet add package ACLI
```

Or via Package Manager Console:

```powershell
Install-Package ACLI
```

---

## Quick Start

The following example demonstrates setting up a CLI instance with actions for `build`, `run`, and `help`:

```csharp
using ACLI;

// 1. Define action handlers
string[] buildParams = [];
string[] runParams = [];
string[] helpParams = [];

IEnumerable<(string[] flags, Action<string[]> action)> actions = [
    (["b", "build"], (cli, parameters) => { buildParams = parameters; }),
    (["r", "run"],   (cli, parameters) => { runParams = parameters; }),
    (["h", "help"],  (cli, parameters) => { helpParams = parameters; })
];

// 2. Configure CLI properties
var cliProperties = new CliProperties(actions, ConsoleStreams.Default)
{
    PrefixType = FlagPrefixType.Dash,
    SingleFlagOnly = true
};

// 3. Initialize and execute
using var cli = new Cli(cliProperties);

// Execute with command-line arguments
cli.Start("--build", "project.sln", "--configuration", "Release");
```

---

## Configuration (`CliProperties`)

`CliProperties` controls flag behavior, stream assignment, and error handling.

### Core Options

| Property | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `PrefixType` | `FlagPrefixType` | `FlagPrefixType.Dash` | Configures prefix syntax (`Dash`, `Slash`, `None`). |
| `SingleFlagOnly` | `bool` | `true` | Restricts execution to a single flag per invocation when set to `true`. |
| `Input` | `StreamReader` | *Derived* | Target standard input stream. |
| `Output` | `StreamWriter` | *Derived* | Target standard output stream. |
| `Error` | `StreamWriter` | *Derived* | Target standard error stream. |

### Error Customization Delegates

Exception factories can be overridden on `CliProperties` to match your application's error reporting standards:

```csharp
var properties = new CliProperties(actions, ConsoleStreams.Default)
{
    NoArgumentsError = () => new ArgumentException("Please supply at least one option. Use --help for usage details."),
    TooManyArgumentsError = (count) => new InvalidOperationException($"Only one flag is allowed per command; received {count}."),
    IncorrectPrefixError = (expected, actual) => new FormatException($"Invalid prefix '{actual}'. Expected '{expected}'."),
    DashPrefixError = () => new FormatException("Use '--' for multi-letter flags and '-' for single-letter flags.")
};
```

---

## Stream Helper Methods

The `Cli` class provides direct wrappers around configured input, output, and error streams:

```csharp
using var cli = new Cli(cliProperties);

// Standard Output
cli.Print("Processing...");
cli.PrintLn(" Done.");

// Standard Error
cli.Error("Failed to resolve dependencies.");

// Standard Input
char key = cli.Read();
string line = cli.ReadLn();
string content = cli.ReadAll();
```

---

## Unit Testing Example

Because console streams are abstracted through `ConsoleStreams`, testing CLI interactions requires no mock framework setup:

```csharp
using Xunit;
using ACLI;

public class CliTests
{
    [Fact]
    public void ArgTest()
    {
        // Arrange
        string[] buildParams = [];
        string[] runParams = [];
        string[] helpParams = [];

        IEnumerable<(string[], Action<string[]>)> actions = [
            (["b", "build"], (cli, parameters) => { buildParams = parameters; }),
            (["r", "run"],   (cli, parameters) => { runParams = parameters; }),
            (["h", "help"],  (cli, parameters) => { helpParams = parameters; })
        ];

        var cliProperties = new CliProperties(actions, ConsoleStreams.Dummy);
        using var cli = new Cli(cliProperties);

        // Act
        cli.Start("--build", "test.aero", "run.aero");

        // Assert
        Assert.Equal(["test.aero", "run.aero"], buildParams);
        Assert.Empty(runParams);
        Assert.Empty(helpParams);
    }
}
```

---

## License

This project is licensed under the [MIT License](LICENSE).
