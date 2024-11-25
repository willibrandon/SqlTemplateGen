# SqlTemplateGen

SqlTemplateGen is a lightweight library for building SQL queries from templates. It simplifies the generation of SQL queries with dynamic parameters, reducing the need for manual query construction.

## Features

- Build SQL queries using a template-based approach.
- Replace placeholders in SQL templates with parameterized values.
- Simple, flexible, and easy-to-use API.

## Prerequisites

Make sure you have .NET 8 or later installed. If not, download and install the latest version of .NET from [here](https://dotnet.microsoft.com/download).

## Installation

To get started, add **SqlTemplateGen** to your project via the .NET CLI:

```bash
dotnet add package SqlTemplateGen
```

Alternatively, install it using NuGet Package Manager in Visual Studio.

## Example

Here is an example using **SqlTemplateGen** to build a SQL query from a template with parameters.

### Building a Simple Query

```csharp
using SqlTemplateGen;

var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Id = {Id}");
builder.AddParameter("Id", 123);

string sql = builder.BuildQuery();
Console.WriteLine(sql);
// Output: SELECT * FROM Users WHERE Id = 123
```

In this example:
- A query template with a placeholder `{Id}` is defined.
- The parameter `{Id}` is replaced with the value `123`.
- The final SQL query is built and printed.

## Next Steps

Once you’ve got the basics down, you can explore more complex query-building scenarios, such as handling multiple parameters, optional parameters, and dynamic queries.

Check out [GettingStarted.md](docs/GettingStarted.md) for installation instructions and a complete guide to get you up and running.

For more advanced scenarios, check out [UsageExamples.md](docs/UsageExamples.md).

## Contributing

Contributions are welcome! If you would like to contribute to **SqlTemplateGen**, please fork the repository, make your changes, and submit a pull request.
