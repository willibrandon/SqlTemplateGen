# Getting Started with SqlTemplateGen

Welcome to the **SqlTemplateGen** library! This guide will help you get started with building SQL queries from templates in no time.

## Prerequisites

Make sure you have .NET 6 or later installed. If not, download and install the latest version of .NET from [here](https://dotnet.microsoft.com/download).

## Installation

To get started, you need to add **SqlTemplateGen** to your project. Use the following command to install it via the .NET CLI:

```bash
dotnet add package SqlTemplateGen
```

Alternatively, you can install it using NuGet Package Manager in Visual Studio.

## Create a Simple Query

The `QueryBuilder` class in **SqlTemplateGen** allows you to generate SQL queries from templates. Here's how you can use it to create a simple query.

### Example: Build a Query

```csharp
using SqlTemplateGen;

var queryBuilder = new QueryBuilder("SELECT * FROM Users WHERE Id = {Id}");
queryBuilder.AddParameter("{Id}", 123);

string sql = queryBuilder.BuildQuery();
Console.WriteLine(sql); // Output: SELECT * FROM Users WHERE Id = 123
```

In this example, we:
1. Created a query template with a placeholder `{Id}`.
2. Added a parameter to replace `{Id}` with the value `123`.
3. Built the final query and printed it.

## Next Steps

Now that you've installed **SqlTemplateGen** and seen a basic example, you can explore more advanced scenarios, such as handling multiple parameters or building complex queries.

For more examples, check out the [UsageExamples.md](UsageExamples.md) file.