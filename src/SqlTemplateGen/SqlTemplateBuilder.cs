namespace SqlTemplateGen;

/// <summary>
/// Provides functionality to build a SQL query from a template, with named parameters that are automatically replaced.
/// </summary>
public class SqlTemplateBuilder
{
    private string _template;
    private readonly List<TemplateParameter> _parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlTemplateBuilder"/> class.
    /// </summary>
    /// <param name="template">The SQL template with named parameters (e.g., {ParameterName}).</param>
    /// <exception cref="ArgumentException">Thrown when the SQL template is null or empty.</exception>
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
    /// <exception cref="ArgumentException">Thrown when the parameter name is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the parameter value is null.</exception>
    public SqlTemplateBuilder AddParameter(string name, object value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

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
            // Ensure all parameters match placeholders in the template
            var placeholderCount = _template.Split('{').Length - 1;
            if (_parameters.Count != placeholderCount)
            {
                throw new InvalidOperationException($"Expected {placeholderCount} parameters, but found {_parameters.Count}.");
            }

            // Replace placeholders with actual parameter values,
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
    /// Formats the parameter value as a valid SQL literal string.
    /// </summary>
    /// <param name="value">The parameter value to format.</param>
    /// <returns>A string representing the formatted value for SQL insertion.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    private string FormatValueForSql(object value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        // Ensure we escape string values correctly to prevent SQL injection.
        if (value is string stringValue)
        {
            // Handle SQL string escaping.
            return $"'{stringValue.Replace("'", "''")}'";
        }

        // Add logic for other types as needed (e.g., DateTime, int).
        return value.ToString() ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Retrieves all the parameters added to the builder.
    /// </summary>
    /// <returns>A list of <see cref="SqlParameter"/> objects representing the parameters added to the template.</returns>
    public List<TemplateParameter> GetParameters()
        => _parameters;

    /// <summary>
    /// Formats a single parameter value as a valid SQL literal string.
    /// </summary>
    /// <param name="param">The <see cref="SqlParameter"/> object to format.</param>
    /// <returns>A string representing the formatted parameter value for SQL insertion.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>"
    public string FormatParameter(TemplateParameter param)
    {
        ArgumentNullException.ThrowIfNull(param, nameof(param));

        return FormatValueForSql(param.Value);
    }
}
