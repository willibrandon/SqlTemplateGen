namespace SqlTemplateGen.Tests;

public class SqlTemplateBuilderTests
{
    [Fact]
    public void AddParameter_ShouldAddParameterToBuilder()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");

        builder.AddParameter("Name", "John");
        var parameters = builder.GetParameters();

        Assert.Single(parameters);
        Assert.Equal("Name", parameters.First().Name);
        Assert.Equal("John", parameters.First().Value);
    }

    [Fact]
    public void BuildQuery_ShouldReplaceParameterWithValue()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
        builder.AddParameter("Name", "John")
               .AddParameter("Age", 30);

        var result = builder.BuildQuery();

        Assert.Equal("SELECT * FROM Users WHERE Name = 'John' AND Age = 30", result);
    }

    [Fact]
    public void BuildQuery_ShouldThrowException_WhenParametersDoNotMatchPlaceholders()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
        builder.AddParameter("Name", "John");

        var exception = Assert.Throws<InvalidOperationException>(builder.BuildQuery);
        Assert.Equal("Expected 2 parameters, but found 1.", exception?.InnerException?.Message);
    }

    [Fact]
    public void FormatParameter_ShouldFormatParameterForSql()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
        builder.AddParameter("Name", "John");

        var parameter = builder.GetParameters().First();
        var formattedValue = builder.FormatParameter(parameter);

        Assert.Equal("'John'", formattedValue);
    }

    [Fact]
    public void GetParameters_ShouldReturnAllAddedParameters()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
        builder.AddParameter("Name", "John")
               .AddParameter("Age", 30);

        var parameters = builder.GetParameters();

        Assert.Equal(2, parameters.Count);
        Assert.Contains(parameters, p => p.Name == "Name" && p.Value.Equals("John"));
        Assert.Contains(parameters, p => p.Name == "Age" && p.Value.Equals(30));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTemplateIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlTemplateBuilder(null!));
        Assert.Throws<ArgumentException>(() => new SqlTemplateBuilder(string.Empty));
        Assert.Throws<ArgumentException>(() => new SqlTemplateBuilder("   "));
    }

    [Fact]
    public void AddParameter_ShouldThrowException_WhenNameIsNullOrEmpty()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");

        Assert.Throws<ArgumentException>(() => builder.AddParameter("", "John"));
        Assert.Throws<ArgumentException>(() => builder.AddParameter(" ", "John"));
    }

    [Fact]
    public void AddParameter_ShouldThrowException_WhenValueIsNull()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");

        Assert.Throws<ArgumentNullException>(() => builder.AddParameter("Name", null!));
    }

    [Fact]
    public void BuildQuery_ShouldThrowException_WhenPlaceholderIsNotFound()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
        builder.AddParameter("Age", 30);

        var exception = Assert.Throws<InvalidOperationException>(builder.BuildQuery);
        Assert.Equal("Placeholder '{Age}' not found in the SQL template.", exception?.InnerException?.Message);
    }
}

