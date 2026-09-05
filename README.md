# Alci - Advanced Command Line Interface

[![Build & Test](https://github.com/Aero-Language/Acli/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Aero-Language/Acli/actions/workflows/dotnet.yml)
[![NuGet Version](https://img.shields.io/nuget/v/AeroLang.ACLI.svg?style=flat-square)](https://www.nuget.org/packages/AeroLang.ACLI/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AeroLang.ACLI.svg?style=flat-square)](https://www.nuget.org/packages/AeroLang.ACLI/)
[![Framework](https://img.shields.io/badge/framework-.NET%2010.0-purple.svg?style=flat-square)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)

Acli is a modern, lightweight, and extensible C# / .NET 10 library designed to make building command-line applications straightforward. It provides stream handling, flag prefix validation, super/sub-argument routing, and custom exception handling.

---

## Installation

Install ACLI using the .NET CLI:

```shell
dotnet add package AeroLang.ACLI
```

Or via Package Manager Console:

```powershell
Install-Package AeroLang.ACLI
```

---

## What the End-User sees
The Cli can be customized in multiple different ways:
```shell
acli -p --print   // Standard POSIX interface
acli /p /print    // DOS style
Acli -P -Print    // Powershell style
acli print        // Prefix-Less style (highly discouraged)
```


Here the POSIX interface is special in that it also supports argument merging.
So these act the same!
```shell
acli -p -v    // Print + Verbose
acli -pv
```

---

## What the Developer sees

Acli is designed to be very easy and with a specific layout in mind.
The first step to make your Cli with Acli, is to define the flags of your application.

```csharp
private record VerboseFlag() : Flag<VerboseFlag>(["v", "verbose"]);
private record PrintCommand() : CommandFlag<PrintCommand>(["p", "print"], Print);

static void Print(Flag[] flags, string[] values)
{
    bool isVerbose = flags.Contains(VerboseFlag.Instance);
    // ... Your code here
}
```
The *.Instance fields are generated automatically and 
are also the reason for the self-referencing generic parameter.
If you don't need them, you can use the Generic-Less version instead, they are the same thing without the *.Instance field.
But you can still check for the flag by constructing it new,
because of the record equality in C#.

Then we have to make CliProperties to pass our options to Acli.
Then we can finally construct the Cli.
```csharp
static readonly Flag[] Flags = 
[
    PrintCommand.Instance,
    VerboseFlag.Instance
];
static readonly CliProperties Properties = new(Flags);
static readonly Cli Cli = new(Properties);
```

You can then pass your actual string[] of arguments into Cli.Start() and let Acli 
parse them and invoke your configured commands!

```csharp
public class Programm
{
    // ... Setup from before
    
    public static void Main(string[] args)
    {
        Cli.Start(args);
    }
}
```


---

## License

This project is licensed under the [MIT License](LICENSE).
