using Cake.Core;
using Cake.Core.IO;

namespace Cake.Asciidoctor;

//TODO: Add Cake alias attributes
public static class AsciidoctorAliases
{
    public static void Asciidoctor(this ICakeContext context, FilePath file) => context.Asciidoctor(file, null);

    public static void Asciidoctor(this ICakeContext context, FilePath file, AsciidoctorSettings? settings)
    {
        settings ??= new();
        var runner = new AsciidoctorRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        runner.Run(file, settings);
    }

    public static void AsciidoctorPdf(this ICakeContext context, FilePath file) => context.AsciidoctorPdf(file, null);

    public static void AsciidoctorPdf(this ICakeContext context, FilePath file, AsciidoctorPdfSettings? settings)
    {
        settings ??= new();
        var runner = new AsciidoctorPdfRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        runner.Run(file, settings);
    }
}
