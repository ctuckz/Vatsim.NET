# Vatsim.NET
.NET Vatsim API Library

Master: [![Build Status](https://travis-ci.org/ctuckz/Vatsim.NET.svg?branch=master)](https://travis-ci.org/ctuckz/Vatsim.NET)

## Distribution

The Vatsim.NET libraries can be easily referenced in your project through [Nuget](https://www.nuget.org/packages/Vatsim.NET/). 

__Visual Studio:__

`Install-Package Vatsim.NET -Version 1.0.3`

__.NET Core:__

`dotnet add package Vatsim.NET --version 1.0.3`

## Usage
Get an `IVatsim` instance by calling `VatsimAPI.GetModule()`. The methods off of `IVatsim` allow you to interact with the rest of the API.

## Framework
Vatsim.NET targets .NET Standard 1.5. It is compatible with .NET Framework 4.6.2, .NET Core 1.0, and Mono 4.6. See [here](https://docs.microsoft.com/en-us/dotnet/standard/library) for the full compatibility matrix.