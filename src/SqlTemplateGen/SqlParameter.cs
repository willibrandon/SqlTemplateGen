namespace SqlTemplateGen;

/// <summary>
/// Represents a named SQL parameter with its corresponding value.
/// </summary>
public class SqlParameter
{
    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the value of the parameter.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlParameter"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter (e.g., @ParameterName).</param>
    /// <param name="value">The value to be assigned to this parameter.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the parameter name or value is null.
    /// </exception>
    public SqlParameter(string name, object value)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Parameter name cannot be null.");
        Value = value ?? throw new ArgumentNullException(nameof(value), "Parameter value cannot be null.");
    }
}
