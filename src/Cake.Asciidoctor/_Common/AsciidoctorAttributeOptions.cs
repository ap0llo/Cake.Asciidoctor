using System;

namespace Cake.Asciidoctor;

/// <summary>
/// Enumerates options for AsciiDoc attribites
/// </summary>
/// <seealso cref="AsciidoctorAttribute"/>
/// <seealso cref="AsciidoctorAttributeCollection.Add(string, string, Cake.Asciidoctor.AsciidoctorAttributeOptions)"/>
[Flags]
public enum AsciidoctorAttributeOptions
{
    /// <summary>
    /// Default options for AsciiDoct attributes.
    /// </summary>
    Default = 0x0,

    /// <summary>
    /// Do not override the value of an attribute if it is already defined in the input file.
    /// </summary>
    NoOverride = 0x1
}
