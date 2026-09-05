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

```sh
dotnet add package AeroLang.ACLI
```

Or via Package Manager Console:

```powershell
Install-Package AeroLang.ACLI
```

---

## What the End-User sees
The Cli can be customized in multiple different ways:
```sh
acli -p --print   // Standard POSIX interface
acli /p /print    // DOS style
Acli -P -Print    // Powershell style
acli print        // Prefix-Less style (highly discouraged)
```


Here the POSIX interface is special in that it also supports argument merging.
So these act the same!
```sh
acli -p -v    // Print + Verbose
acli -pv
```

---

## What the Developer sees

---

## License

This project is licensed under the [MIT License](LICENSE).
