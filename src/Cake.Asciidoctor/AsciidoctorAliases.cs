// Disable nullable reference methods for the alias method, since
// the Cake.Scripting runner is currently not compatible with aliases that contains nullabilty annotations
// See https://github.com/cake-build/cake/issues/4197
#nullable disable

using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Asciidoctor;

/// <summary>
/// Provides functionality for interacting with Asciidoctor and Asciidoctor PDF
/// </summary>
[CakeAliasCategory("Asciidoctor")]
[CakeNamespaceImport("Cake.Asciidoctor")]
public static class AsciidoctorAliases
{
    /// <summary>
    /// Runs Asciidoctor to convert the specified input file with the default settings.
    /// </summary>
    /// <param name="context">The <see cref="ICakeContext"/>.</param>
    /// <param name="inputFile">The path of the AsciiDoc file to convert.</param>
    [CakeMethodAlias]
    public static void Asciidoctor(this ICakeContext context, FilePath inputFile) => context.Asciidoctor(inputFile, null);

    /// <summary>
    /// Runs Asciidoctor to convert the specified input file with the specified settings.
    /// </summary>
    /// <param name="context">The <see cref="ICakeContext"/>.</param>
    /// <param name="inputFile">The path of the AsciiDoc file to convert.</param>
    /// <param name="settings">Optional parameters specified as <see cref="AsciidoctorSettings"/>.</param>
    [CakeMethodAlias]
    public static void Asciidoctor(this ICakeContext context, FilePath inputFile, AsciidoctorSettings settings)
    {
        settings ??= new();
        var runner = new AsciidoctorRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        runner.Run(inputFile, settings);
    }

    /// <summary>
    /// Runs Asciidoctor PDF to convert the specified input file to PDF with the default settings.
    /// </summary>
    /// <param name="context">The <see cref="ICakeContext"/>.</param>
    /// <param name="inputFile">The path of the AsciiDoc file to convert.</param>
    [CakeMethodAlias]
    public static void AsciidoctorPdf(this ICakeContext context, FilePath inputFile) => context.AsciidoctorPdf(inputFile, null);

    /// <summary>
    /// Runs Asciidoctor PDF to convert the specified input file to PDF with the specified settings.
    /// </summary>
    /// <param name="context">The <see cref="ICakeContext"/>.</param>
    /// <param name="inputFile">The path of the AsciiDoc file to convert.</param>
    /// /// <param name="settings">Optional parameters specified as <see cref="AsciidoctorPdfSettings"/>.</param>
    [CakeMethodAlias]
    public static void AsciidoctorPdf(this ICakeContext context, FilePath inputFile, AsciidoctorPdfSettings settings)
    {
        settings ??= new();
        var runner = new AsciidoctorPdfRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        runner.Run(inputFile, settings);
    }
}
