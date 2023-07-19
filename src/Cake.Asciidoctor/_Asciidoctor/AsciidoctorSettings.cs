namespace Cake.Asciidoctor;

/// <summary>
/// Optional settings for Asciidoctor 
/// </summary>
/// <seealso href="https://docs.asciidoctor.org/asciidoctor/latest/cli/man1/asciidoctor/">asciidoctor(1)</seealso>
/// <seealso cref="AsciidoctorPdfSettings"/>
public class AsciidoctorSettings : AsciidoctorSettingsBase
{
    /// <summary>
    /// The output file format (command line option <c>--backend</c>)
    /// </summary>
    public string? Backend { get; set; }

    /// <summary>
    /// The document type to use for converting AsciiDoc documents (command line option <c>--doctype</c>)
    /// </summary>
    public AsciidoctorDoctype? Doctype { get; set; }

    /// <summary>
    /// The template engine to use for custom converter templates (command line option <c>--template-engine</c>)
    /// </summary>
    public string? TemplateEngine { get; set; }
}
