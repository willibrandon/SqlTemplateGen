namespace SqlTemplateGen.Tests;

public class SqlTemplateBuilderTests
{
    [Fact]
    public void AddTemplate_ShouldAddParameterToBuilder()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");

        builder.AddTemplate("Name", "John");
        var parameters = builder.GetParameters();

        Assert.Single(parameters);
        Assert.Equal("Name", parameters.First().Name);
        Assert.Equal("John", parameters.First().Value);
    }

    [Fact]
    public void AddTemplate_ShouldNotThrowException_WhenValueIsNull()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");

        var exception = Record.Exception(() => builder.AddTemplate("Name", null!));

        Assert.Null(exception);
    }

    [Fact]
    public void AddTemplate_ShouldThrowException_WhenPlaceholderIsNullOrEmptyOrWhitespace()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");

        Assert.Throws<ArgumentException>(() => builder.AddTemplate(null!, "John"));
        Assert.Throws<ArgumentException>(() => builder.AddTemplate("", "John"));
        Assert.Throws<ArgumentException>(() => builder.AddTemplate(" ", "John"));
    }

    [Theory]
    [InlineData(true, "1")]
    [InlineData(false, "0")]
    public void BooleanValues_AreFormattedCorrectly(bool input, string expected)
    {
        var builder = new SqlTemplateBuilder("SELECT {Value}");

        builder.AddTemplate("Value", input);
        var result = builder.BuildQuery();

        Assert.Equal($"SELECT {expected}", result);
    }

    [Fact]
    public void BuildQuery_ShouldReplaceParameterWithValue()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
        builder.AddTemplate("Name", "John")
               .AddTemplate("Age", 30);

        var result = builder.BuildQuery();

        Assert.Equal("SELECT * FROM Users WHERE Name = 'John' AND Age = 30", result);
    }

    [Fact]
    public void BuildQuery_ShouldThrowException_WhenParametersDoNotMatchPlaceholders()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
        builder.AddTemplate("Name", "John");

        var exception = Assert.Throws<InvalidOperationException>(builder.BuildQuery);
        Assert.Equal("Expected 2 parameters, but found 1.", exception?.InnerException?.Message);
    }

    [Fact]
    public void BuildQuery_ShouldThrowException_WhenPlaceholderIsNotFound()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
        builder.AddTemplate("Age", 30);

        var exception = Assert.Throws<InvalidOperationException>(builder.BuildQuery);
        Assert.Equal("Placeholder '{Age}' not found in the SQL template.", exception?.InnerException?.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTemplateIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlTemplateBuilder(null!));
        Assert.Throws<ArgumentException>(() => new SqlTemplateBuilder(string.Empty));
        Assert.Throws<ArgumentException>(() => new SqlTemplateBuilder("   "));
    }

    [Fact]
    public void DateTimeTypes_AreFormattedCorrectly()
    {
        var builder = new SqlTemplateBuilder("VALUES ({DateTime}, {DateTimeOffset}, {TimeSpan})");
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        var dateTimeOffset = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.FromHours(1));
        var timeSpan = TimeSpan.FromHours(2);

        builder.AddTemplate("DateTime", dateTime)
               .AddTemplate("DateTimeOffset", dateTimeOffset)
               .AddTemplate("TimeSpan", timeSpan);
        var result = builder.BuildQuery();

        Assert.Contains("'2024-01-01 12:00:00'", result);
        Assert.Contains("'2024-01-01 12:00:00 +01:00'", result);
        Assert.Contains("'02:00:00'", result);
    }

    [Fact]
    public void EmptyString_IsHandledCorrectly()
    {
        var builder = new SqlTemplateBuilder("SELECT {Value}");

        builder.AddTemplate("Value", string.Empty);
        var result = builder.BuildQuery();

        Assert.Equal("SELECT ''", result);
    }

    [Fact]
    public void FloatingPointTypes_AreFormattedCorrectly()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Table WHERE Float={Float} AND Double={Double} AND Decimal={Decimal}");

        builder.AddTemplate("Float", 3.14f)
               .AddTemplate("Double", 3.14159265359d)
               .AddTemplate("Decimal", 123.456m);
        var result = builder.BuildQuery();

        Assert.Contains("Float=3.14", result);
        Assert.Contains("Double=3.14159265359", result);
        Assert.Contains("Decimal=123.456", result);
    }

    [Fact]
    public void FormatParameter_ShouldFormatParameterForSql()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name}");
        builder.AddTemplate("Name", "John");

        var parameter = builder.GetParameters().First();
        var formattedValue = SqlTemplateBuilder.FormatParameter(parameter);

        Assert.Equal("'John'", formattedValue);
    }

    [Fact]
    public void GetParameters_ShouldReturnAllAddedParameters()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Users WHERE Name = {Name} AND Age = {Age}");
        builder.AddTemplate("Name", "John")
               .AddTemplate("Age", 30);

        var parameters = builder.GetParameters();

        Assert.Equal(2, parameters.Count);
        Assert.Contains(parameters, p => p.Name == "Name" && p.Value.Equals("John"));
        Assert.Contains(parameters, p => p.Name == "Age" && p.Value.Equals(30));
    }

    [Fact]
    public void IntegerTypes_AreFormattedCorrectly()
    {
        var builder = new SqlTemplateBuilder("SELECT * FROM Table WHERE Int={Int} AND Long={Long} AND Short={Short}");

        builder.AddTemplate("Int", 42)
               .AddTemplate("Long", 9223372036854775807L)
               .AddTemplate("Short", (short)32767);
        var result = builder.BuildQuery();

        Assert.Equal("SELECT * FROM Table WHERE Int=42 AND Long=9223372036854775807 AND Short=32767", result);
    }

    [Fact]
    public void NullableTypes_AreFormattedCorrectly()
    {
        var builder = new SqlTemplateBuilder("VALUES ({NullInt}, {NonNullInt}, {NullDateTime})");
        int? nullInt = null;
        int? nonNullInt = 42;
        DateTime? nullDateTime = null;

        builder.AddTemplate("NullInt", nullInt!)
               .AddTemplate("NonNullInt", nonNullInt)
               .AddTemplate("NullDateTime", nullDateTime!);
        var result = builder.BuildQuery();

        Assert.Contains("NULL", result);
        Assert.Contains("42", result);
    }

    [Fact]
    public void NumericEdgeCases_AreHandledCorrectly()
    {
        var builder = new SqlTemplateBuilder("VALUES ({Min}, {Max}, {Zero})");

        builder.AddTemplate("Min", int.MinValue)
               .AddTemplate("Max", int.MaxValue)
               .AddTemplate("Zero", 0);
        var result = builder.BuildQuery();

        Assert.Contains($"{int.MinValue}", result);
        Assert.Contains($"{int.MaxValue}", result);
        Assert.Contains("0", result);
    }

    [Fact]
    public void SpecialTypes_AreFormattedCorrectly()
    {
        var builder = new SqlTemplateBuilder("SELECT {Guid}, {Binary}, {Enum}");
        var guid = Guid.NewGuid();
        var binary = new byte[] { 0x12, 0x34, 0x56 };
        var enumValue = DayOfWeek.Monday;

        builder.AddTemplate("Guid", guid)
               .AddTemplate("Binary", binary)
               .AddTemplate("Enum", enumValue);
        var result = builder.BuildQuery();

        Assert.Contains($"'{guid}'", result);
        Assert.Contains("0x123456", result);
        Assert.Contains("Monday", result);
    }

    [Fact]
    public void StringWithSpecialCharacters_IsEscapedCorrectly()
    {
        var builder = new SqlTemplateBuilder("SELECT {Text}");
        var text = "O'Neill's; DROP TABLE Students;--";

        builder.AddTemplate("Text", text);
        var result = builder.BuildQuery();

        Assert.Equal("SELECT 'O''Neill''s; DROP TABLE Students;--'", result);
    }
}
