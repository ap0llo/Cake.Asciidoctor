using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

/// <summary>
/// Base class for settings that are supported by both Asciidoctor and Asciidoctor PDF
/// </summary>
/// <seealso href="https://docs.asciidoctor.org/asciidoctor/latest/cli/man1/asciidoctor/">asciidoctor(1)</seealso>
public abstract class AsciidoctorSettingsBase : ToolSettings
{
    public bool RunWithBundler { get; set; }

    /// <summary>
    /// The base directory containing the input document and resources (command line option <c>--base-dir</c>)
    /// </summary>
    public DirectoryPath? BaseDirectory { get; set; }

    /// <summary>
    /// The "safe mode" to use when converting documents (command line option <c>--safe-mode</c>)
    /// </summary>
    public AsciidoctorSafeMode? SafeMode { get; set; }

    /// <summary>
    /// The attributes to set, override or unset when converting documents (command line option <c>--attribute</c>)
    /// </summary>
    public AsciidoctorAttributeCollection Attributes { get; set; } = new();

    /// <summary>
    /// The output directory for converted documents (command line option <c>--destination-dir</c>)
    /// </summary>
    public DirectoryPath? DestinationDirectory { get; set; }

    /// <summary>
    /// When set to <c>true</c>, generate an embeddable document which excludes content other than the document body (command line option <c>--embedded</c>)
    /// </summary>
    public bool Embedded { get; set; }

    /// <summary>
    /// Specifies additional path where extensions can be loaded from (command line option <c>--load-path</c>)
    /// </summary>
    public ICollection<DirectoryPath> LoadPaths { get; set; } = new HashSet<DirectoryPath>();

    /// <summary>
    /// When set to <c>true</c>, enables automatic numbering of sections (command line option <c>--section-numbers</c>).
    /// </summary>
    public bool SectionNumbers { get; set; }

    /// <summary>
    /// The outpath for the converted docments (command line option <c>--out-file</c>)
    /// </summary>
    public FilePath? OutFile { get; set; }

    /// <summary>
    /// The path of the source directory (command line option <c>--source-dir</c>)
    /// </summary>
    public DirectoryPath? SourceDirectory { get; set; }

    /// <summary>
    /// Specifies additional library to load before converting the input document (command line option <c>--require</c>)
    /// </summary>
    public ICollection<string> Require { get; set; } = new HashSet<string>();

    /// <summary>
    /// Specifies the paths of directories containing custom converted templates (command line option <c>--template-dir</c>)
    /// </summary>
    public ICollection<DirectoryPath> TemplateDirectories { get; set; } = new HashSet<DirectoryPath>();

    /// <summary>
    /// The minimual log level that causes the document conversion to fail (command line option <c>--failure-level</c>)
    /// </summary>
    public AsciidoctorFailureLevel? FailureLevel { get; set; }

    /// <summary>
    /// When set to <c>true</c>, disables log messages (command line option <c>--quiet</c>)
    /// </summary>
    public bool Quiet { get; set; }

    /// <summary>
    /// When set to <c>true</c>, includes additional information when logging errors in the console output (command line option <c>--trace</c>)
    /// </summary>
    public bool Trace { get; set; }

    /// <summary>
    /// When set to <c>true</c>, includes <c>INFO</c> and <c>DEBUF</c> messages in the log output (command line option <c>--verbose</c>)
    /// </summary>
    public bool Verbose { get; set; }

    /// <summary>
    /// When set to <c>true</c>, enables script warnings (command line option <c>--warnings</c>)
    /// </summary>
    public bool Warnings { get; set; }

    /// <summary>
    /// When set to <c>true</c>, prints out a timing report (command line option <c>--timings</c>)
    /// </summary>
    public bool Timings { get; set; }
}
