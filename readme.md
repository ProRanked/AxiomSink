[![Publish to GitHub Packages](https://github.com/ProRanked/AxiomSink/actions/workflows/publishgithubpackages.yml/badge.svg)](https://github.com/ProRanked/AxiomSink/actions/workflows/publishgithubpackages.yml)

## Serilog C# Implementation for Axiom in .NET8

Requirements: Create a Dataset in Axiom and get a token to use.

## How to Use

Download the nuget from Github Packages and Download Serilog, you then configure in your startup.cs

```c#

using ProRanked.Axiom.Sinks;

_logger = new LoggerConfiguration()
    .WriteTo.Axiom("your-dataset", "your-token")
    .CreateLogger();

_logger.Information("Here goes your information log");
_logger.Error("Here goes your Errors");

`````
Author: Devs from ProRanked
Enjoy.

