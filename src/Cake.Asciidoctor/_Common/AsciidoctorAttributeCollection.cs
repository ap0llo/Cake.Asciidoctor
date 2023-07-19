using System;
using System.Collections;
using System.Collections.Generic;

namespace Cake.Asciidoctor;

/// <summary>
/// The collection of attributes being passed to Asciidoctor
/// </summary>
public class AsciidoctorAttributeCollection : IEnumerable<AsciidoctorAttribute>
{
    private readonly Dictionary<string, AsciidoctorAttribute> m_Attributes = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the value of the specified attribute.
    /// </summary>
    /// <remarks>
    /// Setting a value will implicitly add the attribute if it does not yet exist.
    /// Also, if we attribute was previously added as "unset attribute" via <see cref="Unset(string)"/>, setting a value will convert the attribute to a regular attribute.
    /// </remarks>
    public string? this[string name]
    {
        get => m_Attributes[name].Value;
        set
        {
            var attribute = GetOrAdd(name);
            attribute.Value = value;
            attribute.Unset = false;
        }
    }

    /// <inheritdoc />
    public IEnumerator<AsciidoctorAttribute> GetEnumerator() => m_Attributes.Values.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Adds the specified attribute with an empty value.
    /// This method is equivalent to <see cref="Define(string)"/>.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    public void Add(string name) => Define(name);

    /// <summary>
    /// Adds the specified attribute with the specified value.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    public void Add(string name, string value) => Add(name, value, AsciidoctorAttributeOptions.Default);

    /// <summary>
    /// Adds the specified attribute with the specified options.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="options">The options to set for the attribute.</param>
    public void Add(string name, string value, AsciidoctorAttributeOptions options)
    {
        var attribute = new AsciidoctorAttribute(name, value)
        {
            Options = options
        };
        m_Attributes.Add(name, attribute);
    }


    /// <summary>
    /// Defines an attribute with an empty value.
    /// </summary>
    /// <remarks>
    /// If the attribute was already added to this collection, the attribute's value is set to an empty value.
    /// Also, if the attribute was previously added as "unset attribute" via <see cref="Unset(string)"/>, it will be converted to a regular attribute.
    /// </remarks>
    /// <param name="name">The attribute name.</param>
    public void Define(string name)
    {
        var attribute = GetOrAdd(name);
        attribute.Value = null;
        attribute.Unset = false;
    }

    /// <summary>
    /// Unsets the specified attribute.
    /// </summary>
    /// <remarks>
    /// Marking an attribute as an "unset attribute" tells Asciidoctor to remove the attribute value in case it was already defined in the input file.
    /// For example, <c>Unset("Name")</c> will result in an <c>NAME!</c> commandline argument to be passed to Asciidoctor.
    /// <para>
    /// If the attribute was already added to this collection, unsetting the attribute will remove the attribute's value,
    /// and retset the attribute options to <see cref="AsciidoctorAttributeOptions.Default"/>.
    /// </para>
    /// </remarks>
    /// <param name="name">The name of the attribute to unset</param>
    public void Unset(string name)
    {
        var attribute = GetOrAdd(name);
        attribute.Value = null;
        attribute.Unset = true;
        attribute.Options = AsciidoctorAttributeOptions.Default;
    }

    /// <summary>
    /// Removes the attribute from the collection.
    /// </summary>
    /// <remarks>
    /// Note that <see cref="Remove"/> is different from <see cref="Unset"/>.<br />
    /// <list type="bullet">
    ///     <item><see cref="Remove"/> will remove the attribute from the collection and the attribute will not be passed to Asciidoctor at all.</item>
    ///     <item>
    ///     <see cref="Unset"/> will cause the attribute to be passed to Asciidoctor as "unset attribute", causing the attribute to be
    ///     removed while processing the source file even if it was set in the source document.
    ///     </item>
    /// </list>
    /// </remarks>
    public void Remove(string name) => m_Attributes.Remove(name);


    private AsciidoctorAttribute GetOrAdd(string name)
    {
        if (!m_Attributes.TryGetValue(name, out var attribute))
        {
            attribute = new AsciidoctorAttribute(name);
            m_Attributes.Add(name, attribute);
        }

        return attribute;
    }
}
