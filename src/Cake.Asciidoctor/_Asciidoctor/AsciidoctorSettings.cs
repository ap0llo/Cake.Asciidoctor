using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Asciidoctor;

//TODO: Add documentation/reference to asciidoctor(1)
public class AsciidoctorSettings : AsciidoctorSettingsBase
{
    public string? Backend { get; set; }

    public AsciidoctorDoctype? Doctype { get; set; }

    public string? TemplateEngine { get; set; }

    public bool NoHeaderFooter { get; set; }

}
