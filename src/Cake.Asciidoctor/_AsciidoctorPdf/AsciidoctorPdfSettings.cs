namespace Cake.Asciidoctor;

/// <summary>
/// Optional settings for Asciidoctor PDF
/// </summary>
/// <seealso href="https://docs.asciidoctor.org/pdf-converter/latest/">Asciidoctor PDF Documentation</seealso>
/// <seealso cref="AsciidoctorSettings"/>
public class AsciidoctorPdfSettings : AsciidoctorSettingsBase
{
    /// <summary>
    /// The document type to use for converting AsciiDoc documents (command line option <c>--doctype</c>)
    /// </summary>
    public AsciidoctorPdfDoctype? Doctype { get; set; }
}
