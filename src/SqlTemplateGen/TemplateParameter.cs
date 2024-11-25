namespace SqlTemplateGen;

/// <summary>
/// Represents a parameter in a SQL template, with a name and value.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TemplateParameter"/> class with the specified name and value.
/// </remarks>
/// <param name="name">The name of the parameter.</param>
/// <param name="value">The value of the parameter.</param>
public class TemplateParameter(string name, object value)
{
    /// <summary>
    /// Gets or sets the name of the parameter.
    /// </summary>
    /// <value>The name of the parameter.</value>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets the value of the parameter.
    /// </summary>
    /// <value>The value of the parameter.</value>
    public object Value { get; set; } = value;
}