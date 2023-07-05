using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

internal class AsciidoctorRunner : Tool<AsciidoctorSettings>
{
    public AsciidoctorRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
        : base(fileSystem, environment, processRunner, tools)
    { }


    public void Run(FilePath inputFile, AsciidoctorSettings settings) => Run(settings, GetArguments(inputFile, settings));


    protected override string GetToolName() => "Asciidoctor";

    protected override IEnumerable<string> GetToolExecutableNames() => throw new NotImplementedException();

    protected override IEnumerable<string> GetToolExecutableNames(AsciidoctorSettings settings)
    {
        return settings.RunWithBundler
            ? new[] { "bundle", "bundle.bat" }
            : new[] { "asciidoctor", $"asciidoctor.bat" };
    }


    private ProcessArgumentBuilder GetArguments(FilePath inputFile, AsciidoctorSettings settings)
    {
        var argumentsBuilder = new ProcessArgumentBuilder();

        if (settings.RunWithBundler)
        {
            argumentsBuilder
                .Append("exec")
                .Append("asciidoctor");
        }

        argumentsBuilder
            .AppendQuoted(inputFile.ToString())
            .AppendOptionIfNotNull("base-dir", settings.BaseDirectory)
            .AppendOptionIfNotNull("safe-mode", settings.SafeMode)
            .AppendOptionIfNotNull("backend", settings.Backend)
            .AppendOptionIfNotNull("doctype", settings.Doctype)
            .AppendOptionIfNotNull("destination-dir", settings.DestinationDirectory)
            .AppendOptionIfNotNull("template-engine", settings.TemplateEngine)
            .AppendSwitchIf("embedded", settings.Embedded)
            .AppendOptionIfNotNull("load-path", settings.LoadPath)
            .AppendSwitchIf("section-numbers", settings.SectionNumbers)
            .AppendOptionIfNotNull("out-file", settings.OutFile)
            .AppendOptionIfNotNull("source-dir", settings.SourceDirectory);

        foreach (var library in settings.Require)
        {
            argumentsBuilder.AppendOptionQuoted("require", library);
        }

        argumentsBuilder
            .AppendSwitchIf("no-header-footer", settings.NoHeaderFooter)
            .AppendOptionIfNotNull("template-dir", settings.TemplateDirectory)
            .AppendOptionIfNotNull("failure-level", settings.FailureLevel, upperCase: true)
            .AppendSwitchIf("quiet", settings.Quiet)
            .AppendSwitchIf("trace", settings.Trace)
            .AppendSwitchIf("verbose", settings.Verbose)
            .AppendSwitchIf("warnings", settings.Warnings)
            .AppendSwitchIf("timings", settings.Timings);

        foreach (var attribute in settings.Attributes)
        {
            argumentsBuilder.AppendOptionQuoted("attribute", attribute.ToString());
        }


        return argumentsBuilder;
    }
}
