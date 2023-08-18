using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Asciidoctor;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

return new CakeHost()
    .UseContext<BuildContext>()
    .Run(args);

public class BuildContext : FrostingContext
{
    public DirectoryPath SourceDirectory { get; }

    public DirectoryPath OutputDirectory { get; }

    public BuildContext(ICakeContext context) : base(context)
    {
        SourceDirectory = context.Environment.WorkingDirectory.Combine("src");
        OutputDirectory = context.Environment.WorkingDirectory.Combine("out");
    }
}


[TaskName("Default")]
[IsDependentOn(typeof(ConvertDocumentsTask))]
public class DefaultTask : FrostingTask
{ }

[TaskName("ConvertDocuments")]
[IsDependentOn(typeof(InstallAsciidoctorTask))]
public class ConvertDocumentsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (var document in context.GetFiles($"{context.SourceDirectory}/*.adoc"))
        {
            var relativePath = context.SourceDirectory.GetRelativePath(document);

            context.Information($"Converting '{relativePath}' to HTML");
            context.Asciidoctor(
                document,
                new AsciidoctorSettings() { DestinationDirectory = context.OutputDirectory }
            );

            context.Information($"Converting '{relativePath}' to PDF");
            context.AsciidoctorPdf(
                document,
                new AsciidoctorPdfSettings() { DestinationDirectory = context.OutputDirectory }
            );
        }
    }
}

[TaskName("InstallAsciidoctor")]
public class InstallAsciidoctorTask : FrostingTask<BuildContext>
{
    private static readonly IReadOnlyList<string> s_ToolsToInstall = new[]
    {
        "asciidoctor",
        "asciidoctor-pdf",
    };

    public override void Run(BuildContext context)
    {
        var gemHome = ConfigureGemHome(context);

        var gemToolPath = ResolveGem(context);

        foreach (var tool in s_ToolsToInstall)
        {
            GemInstall(context, gemToolPath, tool);
        }

        RegisterTools(context, gemHome);
    }


    private static DirectoryPath ConfigureGemHome(BuildContext context)
    {
        // Change GEM_HOME to install gems into the tools directory instead of installing them in the user directory
        var gemHome = context.Environment.WorkingDirectory.Combine("tools/gem").ToString();
        if (context.Environment.Platform.IsWindows())
        {
            gemHome = gemHome.Replace("/", "\\");
        }

        context.Information($"Installing ruby tools into directory '{gemHome}'");
        Environment.SetEnvironmentVariable("GEM_HOME", gemHome);

        return gemHome;
    }

    private static FilePath ResolveGem(BuildContext context)
    {
        var gemToolName = context.Environment.Platform.Family == PlatformFamily.Windows
            ? "gem.cmd"
            : "gem";

        var gemToolPath = context.Tools.Resolve(gemToolName);

        if (gemToolPath is null)
        {
            throw new CakeException($"Could not find '{gemToolName}'. Ensure that ruby is installed.");
        }

        context.Information($"Using gem executable at '{gemToolPath}'");
        return gemToolPath;
    }

    private static void GemInstall(BuildContext context, FilePath gemToolPath, string tool)
    {
        context.Information($"Installing '{tool}'");

        var settings = new ProcessSettings()
        {
            Arguments = new ProcessArgumentBuilder()
                .Append("install")
                .Append("--no-document")
                .Append(tool)
        };

        context.StartProcess(gemToolPath, settings);
    }

    private static void RegisterTools(BuildContext context, DirectoryPath gemHome)
    {
        var extension = context.Environment.Platform.Family == PlatformFamily.Windows ? ".bat" : "";
        var scripts = context
            .GetFiles($"{gemHome.Combine("bin")}/*")
            .Where(x => x.GetExtension()?.Equals(extension, StringComparison.OrdinalIgnoreCase) == true);

        foreach (var scriptPath in scripts)
        {
            context.Tools.RegisterFile(scriptPath);
        }
    }
}
