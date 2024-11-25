namespace SqlTemplateGen;

/// <summary>
///  Provides functionality to build a SQL query from a template, with named parameters that are automatically replaced.
/// </summary>
public class SqlTemplateBuilder
{
    private string _template;
    private readonly List<TemplateParameter> _parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlTemplateBuilder"/> class.
    /// </summary>
    /// <param name="template">The SQL template string containing placeholders.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="template"/> is null or whitespace.</exception>
    public SqlTemplateBuilder(string template)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(template, nameof(template));

        _template = template;
        _parameters = [];
    }

    /// <summary>
    /// Adds a parameter to the SQL template with a specified name and value.
    /// </summary>
    /// <param name="name">The name of the parameter (e.g., {ParameterName}).</param>
    /// <param name="value">The value to be used for this parameter.</param>
    /// <returns>The current <see cref="SqlTemplateBuilder"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="placeholder"/> is null or whitespace.</exception>
    public SqlTemplateBuilder AddParameter(string name, object value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        _parameters.Add(new TemplateParameter(name, value));
        return this;
    }

    /// <summary>
    /// Builds the final SQL query by replacing placeholders in the template with their corresponding parameter values.
    /// </summary>
    /// <returns>A string representing the fully built SQL query.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the number of parameters doesn't match the placeholders in the SQL template,
    /// or when a parameter is not found in the template.
    /// </exception>
    public string BuildQuery()
    {
        try
        {
            // Ensure all parameters match placeholders in the template.
            var placeholderCount = _template.Split('{').Length - 1;
            if (_parameters.Count != placeholderCount)
            {
                throw new InvalidOperationException($"Expected {placeholderCount} parameters, but found {_parameters.Count}.");
            }

            // Replace placeholders with actual parameter values.
            foreach (var param in _parameters)
            {
                var placeholder = $"{{{param.Name}}}";
                if (!_template.Contains(placeholder))
                {
                    throw new InvalidOperationException($"Placeholder '{placeholder}' not found in the SQL template.");
                }

                _template = _template.Replace(placeholder, FormatValueForSql(param.Value));
            }

            return _template;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error occurred while building the SQL query.", ex);
        }
    }

    /// <summary>
    /// Formats a single parameter value as a valid SQL literal string.
    /// </summary>
    /// <param name="param">The <see cref="TemplateParameter"/> object to format.</param>
    /// <returns>A string representing the formatted parameter value for SQL insertion.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>"
    public static string? FormatParameter(TemplateParameter param)
    {
        ArgumentNullException.ThrowIfNull(param, nameof(param));

        return FormatValueForSql(param.Value);
    }

    /// <summary>
    /// Formats the parameter value as a valid SQL literal string.
    /// </summary>
    /// <param name="value">The parameter value to format.</param>
    /// <returns>A string representing the formatted value for SQL insertion.</returns>
    public static string? FormatValueForSql(object? value)
    {
        // Handle null values.
        if (value == null)
        {
            return "NULL";
        }

        // Handle DateTime formatting.
        if (value is DateTime dateTime)
        {
            // SQL standard datetime format: 'YYYY-MM-DD HH:MM:SS'.
            return $"'{dateTime:yyyy-MM-dd HH:mm:ss}'";
        }

        // Handle DateTimeOffset formatting.
        if (value is DateTimeOffset dateTimeOffset)
        {
            // Format DateTimeOffset as 'YYYY-MM-DD HH:MM:SS +00:00'.
            return $"'{dateTimeOffset:yyyy-MM-dd HH:mm:ss zzz}'";
        }

        // Handle string formatting.
        if (value is string str)
        {
            // Escape single quotes in strings to avoid SQL injection.
            return $"'{str.Replace("'", "''")}'";
        }

        // Handle boolean formatting.
        if (value is bool boolean)
        {
            // SQL representation of true/false.
            return boolean ? "1" : "0";
        }

        // Handle Guid formatting.
        if (value is Guid guid)
        {
            // Format as string.
            return $"'{guid}'";
        }

        // Handle byte array (for binary data).
        if (value is byte[] byteArray)
        {
            // SQL binary data format (e.g., '0x1234...').
            return $"0x{Convert.ToHexString(byteArray)}";
        }

        // Handle decimal, float, double, int, long, short (numeric types).
        if (value is decimal || value is double || value is float ||
            value is int || value is long || value is short)
        {
            // These are used directly as numbers in SQL.
            return value.ToString();
        }

        // Handle TimeSpan formatting (for time intervals).
        if (value is TimeSpan timeSpan)
        {
            // Represent as a string in 'HH:mm:ss' format, useful for time intervals.
            return $"'{timeSpan:hh\\:mm\\:ss}'";
        }

        // Handle nullable types (e.g., Nullable<int>, Nullable<DateTime>, etc.).
        if (Nullable.GetUnderlyingType(value.GetType()) != null)
        {
            // Recursively format the value of the nullable type.
            return FormatValueForSql(value.GetType().GetProperty("Value")?.GetValue(value, null));
        }

        // If the type is unsupported, just use its ToString() representation.
        return value.ToString();
    }

    /// <summary>
    /// Gets all parameters added to the template.
    /// </summary>
    /// <returns>A list of <see cref="TemplateParameter"/> objects representing the parameters added to the template.</returns>
    public List<TemplateParameter> GetParameters()
        => _parameters;
}
