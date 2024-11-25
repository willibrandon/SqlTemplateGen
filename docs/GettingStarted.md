# Getting Started with SqlTemplateGen

Welcome to the **SqlTemplateGen** library! This guide will help you get started with building SQL queries from templates in no time.

## Prerequisites

Make sure you have .NET 8 or later installed. If not, download and install the latest version of .NET from [here](https://dotnet.microsoft.com/download).

## Installation

To get started, you need to add **SqlTemplateGen** to your project. Use the following command to install it via the .NET CLI:

```bash
dotnet add package SqlTemplateGen
```

Alternatively, you can install it using NuGet Package Manager in Visual Studio.

## Create a Simple Query

The `SqlTemplateBuilder` class in **SqlTemplateGen** allows you to generate SQL queries from templates. Here's how you can use it to create a simple query.

### Example: Build a Query

```csharp
using SqlTemplateGen;

var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Id = {Id}");
builder.AddTemplate("Id", 123);

string sql = builder.BuildQuery();
Console.WriteLine(sql);
// Output: SELECT * FROM Users WHERE Id = 123
```

In this example, we:
1. Created a query template with a placeholder `{Id}`.
2. Added a parameter to replace `{Id}` with the value `123`.
3. Built the final query and printed it.

## Next Steps

For more advanced query-building scenarios, such as handling multiple parameters, optional parameters, and dynamic queries, check out [UsageExamples](https://github.com/willibrandon/SqlTemplateGen/blob/main/docs/UsageExamples.md).
