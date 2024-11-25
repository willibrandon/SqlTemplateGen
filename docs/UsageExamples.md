# Usage Examples for `SqlTemplateGen`

This document provides examples of how to use the `SqlTemplateGen` library to create and generate SQL queries from templates.

## Basic Usage

### Creating a Template and Adding Parameters

You can create a new SQL template by initializing a `SqlTemplateBuilder` with a template string that contains placeholders. Then, add parameters using the `AddParameter` method.

```csharp
using SqlTemplateGen;

// Create a new SQL template with placeholders
var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");

// Add parameters to the template
builder.AddTemplate("Name", "John")
       .AddTemplate("Age", 30);

// Build the final SQL query
string query = builder.BuildQuery();

// Output the built query
Console.WriteLine(query);
// Output: SELECT * FROM Users WHERE Name = 'John' AND Age = 30
```

### Handling Multiple Parameters

If you have multiple parameters in your template, you can add them all before building the query.

```csharp
var builder = new SqlTemplateBuilder("SELECT * FROM Orders WHERE CustomerId = {CustomerId} AND OrderDate > {OrderDate}");

builder.AddTemplate("CustomerId", 123)
       .AddTemplate("OrderDate", new DateTime(2023, 1, 1));

string query = builder.BuildQuery();

Console.WriteLine(query);
// Output: SELECT * FROM Orders WHERE CustomerId = 123 AND OrderDate > '2023-01-01'
```

## Error Handling

The `SqlTemplateBuilder` will throw exceptions if the parameters do not match the placeholders in the template.

### Example: Missing Parameters

```csharp
var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
builder.AddTemplate("Name", "Alice");

try
{
    string query = builder.BuildQuery(); // This will throw an exception
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Expected 2 parameters, but found 1.
}
```

### Example: Invalid Parameter Placeholder

```csharp
var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
builder.AddTemplate("Age", 30);

try
{
    string query = builder.BuildQuery(); // This will throw an exception.
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Placeholder '{Age}' not found in the SQL template.
}
```

## Formatting Parameters for SQL

You can format a single parameter value using the `FormatParameter` method.

```csharp
var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
builder.AddTemplate("Name", "John");

var parameter = builder.GetParameters().First();
string formattedValue = builder.FormatParameter(parameter);

Console.WriteLine(formattedValue);
// Output: 'John'
```

## Retrieving Parameters

The `GetParameters` method allows you to retrieve the list of parameters added to the builder.

```csharp
var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
builder.AddTemplate("Name", "John")
       .AddTemplate("Age", 30);

var parameters = builder.GetParameters();
foreach (var param in parameters)
{
    Console.WriteLine($"Parameter: {param.Name} = {param.Value}");
}
// Output:
// Parameter: Name = John
// Parameter: Age = 30
```

## Advanced Example: Complex Query

```csharp
var builder = new SqlTemplateBuilder("INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) VALUES ({CustomerId}, {OrderDate}, {TotalAmount})");

builder.AddTemplate("CustomerId", 123)
       .AddTemplate("OrderDate", new DateTime(2023, 5, 1))
       .AddTemplate("TotalAmount", 99.99);

string query = builder.BuildQuery();

Console.WriteLine(query);
// Output: INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) VALUES (123, '2023-05-01', 99.99)
```

## Handling Special Characters in String Values

The library automatically escapes single quotes in string values to ensure proper SQL syntax.

```csharp
var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
builder.AddTemplate("Name", "O'Reilly");

string query = builder.BuildQuery();

Console.WriteLine(query);
// Output: SELECT * FROM Users WHERE Name = 'O''Reilly'
```

## Powerful Template-Based SQL Generation

With `SqlTemplateGen`, you can easily build SQL queries from templates with named parameters. The library automatically handles parameter substitution, escaping, and error handling, making it a powerful tool for generating dynamic SQL queries in your .NET applications.