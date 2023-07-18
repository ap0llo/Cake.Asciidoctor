using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

//TODO: Add documentation/reference to asciidoctor(1)
public abstract class AsciidoctorSettingsBase : ToolSettings
{
    public bool RunWithBundler { get; set; }

    public DirectoryPath? BaseDirectory { get; set; }

    public AsciidoctorSafeMode? SafeMode { get; set; }

    public AsciidoctorAttributeCollection Attributes { get; set; } = new();

    public DirectoryPath? DestinationDirectory { get; set; }

    public bool Embedded { get; set; }

    public DirectoryPath? LoadPath { get; set; }

    public bool SectionNumbers { get; set; }

    public FilePath? OutFile { get; set; }

    public DirectoryPath? SourceDirectory { get; set; }

    public ICollection<string> Require { get; set; } = new HashSet<string>();

    public DirectoryPath? TemplateDirectory { get; set; }

    public AsciidoctorFailureLevel? FailureLevel { get; set; }

    public bool Quiet { get; set; }

    public bool Trace { get; set; }

    public bool Verbose { get; set; }

    public bool Warnings { get; set; }

    public bool Timings { get; set; }
}
