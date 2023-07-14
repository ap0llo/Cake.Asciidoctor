using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

//TODO: Add documentation/reference to asciidoctor(1)
public class AsciidoctorSettings : ToolSettings
{
    private readonly List<string> m_Require = new();
    private readonly AsciidoctorAttributeCollection m_Attributes = new();

    public bool RunWithBundler { get; set; }

    public DirectoryPath? BaseDirectory { get; set; }

    public AsciidoctorSafeMode? SafeMode { get; set; }

    public AsciidoctorAttributeCollection Attributes
    {
        get => m_Attributes;
        init => m_Attributes = value ?? new();
    }

    public string? Backend { get; set; }

    public AsciidoctorDoctype? Doctype { get; set; }

    public DirectoryPath? DestinationDirectory { get; set; }

    public string? TemplateEngine { get; set; }

    public bool Embedded { get; set; }

    public DirectoryPath? LoadPath { get; set; }

    public bool SectionNumbers { get; set; }

    public FilePath? OutFile { get; set; }

    public DirectoryPath? SourceDirectory { get; set; }

    public IReadOnlyList<string> Require => m_Require;

    public bool NoHeaderFooter { get; set; }

    public DirectoryPath? TemplateDirectory { get; set; }

    public AsciidoctorFailureLevel? FailureLevel { get; set; }

    public bool Quiet { get; set; }

    public bool Trace { get; set; }

    public bool Verbose { get; set; }

    public bool Warnings { get; set; }

    public bool Timings { get; set; }

    public AsciidoctorSettings AddRequire(string library)
    {
        if (String.IsNullOrWhiteSpace(library))
            throw new ArgumentException("Value must not be null or whitespace", nameof(library));

        m_Require.Add(library);
        return this;
    }
}
