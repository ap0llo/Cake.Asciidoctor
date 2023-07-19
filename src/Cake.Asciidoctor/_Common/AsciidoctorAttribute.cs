using System;
using System.Linq;

namespace Cake.Asciidoctor;

/// <summary>
/// Represents an document attribute being set when converting an AsciiDoc document.
/// </summary>
public class AsciidoctorAttribute
{
    /// <summary>
    /// Gets the name of the attribute
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the attribute's value or <c>null</c> if the attribute has no value.
    /// </summary>
    public string? Value { get; internal set; }

    /// <summary>
    /// Gets additional options for the attribute
    /// </summary>
    public AsciidoctorAttributeOptions Options { get; internal set; }

    /// <summary>
    /// Gets or sets whether the attribute is an "unset attribute".
    /// <para>
    /// When an attribute is "unset", the attribute is passed to Asciidoctor wihtout a value and the <c>!</c> option.
    /// This causes the attribute value to be removed even if it is defined in the input document.
    /// </para>
    /// </summary>
    public bool Unset { get; internal set; } = false;


    internal AsciidoctorAttribute(string name) : this(name, null)
    { }

    internal AsciidoctorAttribute(string name, string? value)
    {
        Name = name;
        Value = value;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (Unset)
        {
            return $"{Name}!";
        }

        if (String.IsNullOrEmpty(Value))
        {
            return Name;
        }

        var formattedValue = Value;
        if (formattedValue.Any(char.IsWhiteSpace))
        {
            formattedValue = @$"""{formattedValue}""";
        }

        if (Options.HasFlag(AsciidoctorAttributeOptions.NoOverride))
        {
            return $"{Name}@={formattedValue}";
        }
        else
        {
            return $"{Name}={formattedValue}";
        }
    }
}
