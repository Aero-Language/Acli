# ACLI - Advanced Command Line Interface

[![NuGet Version](https://img.shields.io/nuget/v/AeroLang.ACLI.svg?style=flat-square)](https://www.nuget.org/packages/AeroLang.ACLI/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AeroLang.ACLI.svg?style=flat-square)](https://www.nuget.org/packages/AeroLang.ACLI/)
[![Framework](https://img.shields.io/badge/framework-.NET%2010.0-purple.svg?style=flat-square)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)

ACLI (Advanced Command Line Interface) is a modern, lightweight, and extensible C# / .NET 10 library designed to make building command-line applications straightforward. It provides stream handling, flag prefix validation, super/sub-argument routing, and custom exception handling.

---

## Features

- **Hierarchical Argument Parsing:** Define top-level `SuperArgument` commands along with optional nested `SubArgs`.
- **Typed Argument Payloads:** Callback actions receive the `Cli` instance and an array of `PassedArg` records containing parsed arguments and positional values.
- **Flexible Prefixes:** Supports `Dash` (`-`/`--`), `StrictSingleDash` (`-`), `Slash` (`/`), or `None` prefix options.
- **Customizable Exception Factories:** Override error handlers for missing arguments, prefix mismatches, and unknown super-arguments.
- **Mockable Streams:** Accepts custom `ConsoleStreams` (or `ConsoleStreams.Dummy`) to streamline testing.

---

## Installation

Install ACLI using the .NET CLI:

```bash
dotnet add package AeroLang.ACLI
```

Or via Package Manager Console:

```powershell
Install-Package AeroLang.ACLI
```

---

## Quick Start

The following example demonstrates setting up top-level `SuperArgument` options for `build`, `run`, and `help`:

```csharp
using ACLI;

// 1. Define variables to capture parameters
string[] buildParams = [];
string[] runParams = [];
string[] helpParams = [];

// 2. Define arguments using SuperArgument
IEnumerable<SuperArgument> arguments = [
    new SuperArgument(["b", "build"], (cli, args) => {
        buildParams = args.FirstOrDefault()?.Values ?? [];
    }),
    new SuperArgument(["r", "run"], (cli, args) => {
        runParams = args.FirstOrDefault()?.Values ?? [];
    }),
    new SuperArgument(["h", "help"], (cli, args) => {
        helpParams = args.FirstOrDefault()?.Values ?? [];
    })
];

// 3. Configure CLI properties
var cliProperties = new CliProperties(arguments, ConsoleStreams.Default)
{
    PrefixType = FlagPrefixType.Dash
};

// 4. Initialize and execute
using var cli = new Cli(cliProperties);
cli.Start("--build", "project.sln", "Release");
```

---

## Sub-Arguments Support

ACLI supports nested command structures by attaching `Argument[]` to a parent `SuperArgument`:

```csharp
var arguments = new SuperArgument[]
{
    new SuperArgument(
        Names: ["remote"],
        Command: (cli, args) => {
            cli.PrintLn("Remote command executed.");
        },
        SubArgs: [
            new Argument(["add"], (cli, args) => {
                cli.PrintLn("Adding remote target...");
            }),
            new Argument(["remove"], (cli, args) => {
                cli.PrintLn("Removing remote target...");
            })
        ]
    )
};
```

---

## Configuration (`CliProperties`)

`CliProperties` configures argument mappings, prefix styles, I/O streams, and exception delegates.

### Core Options

| Property     | Type             | Default               | Description                                                             |
|:-------------|:-----------------|:----------------------|:------------------------------------------------------------------------|
| `PrefixType` | `FlagPrefixType` | `FlagPrefixType.Dash` | Configures prefix syntax (`Dash`, `StrictSingleDash`, `Slash`, `None`). |
| `Input`      | `StreamReader`   | *Derived*             | Standard input stream reader.                                           |
| `Output`     | `StreamWriter`   | *Derived*             | Standard output stream writer.                                          |
| `Error`      | `StreamWriter`   | *Derived*             | Standard error stream writer.                                           |

### Flag Prefix Types

- `FlagPrefixType.None`: DotNet style tooling.
- `FlagPrefixType.Dash`: Unix/BSD style (`-b`, `--build`).
- `FlagPrefixType.StrictSingleDash`: Windows PowerShell style (`-B`, `-Build`).
- `FlagPrefixType.Slash`: Traditional Windows CMD style (`/b`, `/build`).

### Error Customization Delegates

Override exception delegates on `CliProperties` to customize error behavior:

```csharp
var properties = new CliProperties(arguments, ConsoleStreams.Default)
{
    NoArgumentsError = () => new ArgumentException("Please supply at least one option. Use --help for usage details."),
    IncorrectPrefixError = (expected, actual) => new FormatException($"Invalid prefix '{actual}'. Expected '{expected}'."),
    DashPrefixError = () => new FormatException("Use '--' for multi-letter flags and '-' for single-letter flags."),
    IncorrectSuperArgError = (arg) => new ArgumentException($"'{arg}' is not a valid argument.")
};
```

---

## Stream Helper Methods & Properties

The `Cli` class provides I/O wrappers around configured input, output, and error streams:

```csharp
using var cli = new Cli(cliProperties);

// Writing
cli.Print("Processing...");
cli.PrintLn(" Done.");
cli.Error("Failed to resolve dependencies.");

// Reading
char key = cli.Read();
string line = cli.ReadLn();
string content = cli.ReadAll();
bool isEnd = cli.IsReadEnd; // Checks if standard input stream is at end
```

---

## Unit Testing Example

Testing CLI interactions using `ConsoleStreams.Dummy`:

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

        IEnumerable<SuperArgument> arguments = [
            new SuperArgument(["b", "build"], (cli, args) => {
                buildParams = args.FirstOrDefault()?.Values ?? [];
            })
        ];

        var cliProperties = new CliProperties(arguments, ConsoleStreams.Dummy);
        using var cli = new Cli(cliProperties);

        // Act
        cli.Start("--build", "test.aero", "run.aero");

        // Assert
        Assert.Equal(["test.aero", "run.aero"], buildParams);
    }
}
```

---

## License

This project is licensed under the [MIT License](LICENSE).