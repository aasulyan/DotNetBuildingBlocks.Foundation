# DotNetBuildingBlocks.Serialization

## Purpose

`DotNetBuildingBlocks.Serialization` provides shared `System.Text.Json` configuration for reusable .NET building blocks.

The package gives one consistent place for default JSON options, converter registration, and small helper methods. It stays framework-neutral and avoids ASP.NET Core or MVC-specific registration.

## Features

- shared `JsonSerializerOptions` factory
- predictable default JSON settings
- camelCase property naming
- camelCase dictionary key naming
- string enum serialization
- case-insensitive property matching
- ignore null values during write
- reusable cloning/apply helpers
- framework-neutral design

## Installation

```bash
dotnet add package DotNetBuildingBlocks.Serialization
