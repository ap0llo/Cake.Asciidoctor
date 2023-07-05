using Cake.Core;
using Cake.Core.IO;

namespace Cake.Asciidoctor;

public static class AsciidoctorAliases
{
    public static void Asciidoctor(this ICakeContext context, FilePath file) => context.Asciidoctor(file, null);

    public static void Asciidoctor(this ICakeContext context, FilePath file, AsciidoctorSettings? settings)
    {
        settings ??= new();
        var runner = new AsciidoctorRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        runner.Run(file, settings);
    }
}
