using System;
using System.Linq;

namespace Cake.Asciidoctor;

public class AsciidoctorAttribute
{
    public string Name { get; }

    public string? Value { get; internal set; }

    public AsciidoctorAttributeOptions Options { get; internal set; }

    public bool Unset { get; internal set; } = false;


    public AsciidoctorAttribute(string name) : this(name, null)
    { }

    public AsciidoctorAttribute(string name, string? value)
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
