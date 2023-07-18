using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

internal abstract class AsciidoctorTool<T> : Tool<T> where T : AsciidoctorSettingsBase
{
    protected abstract string ToolExecuteableName { get; }


    protected AsciidoctorTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
        : base(fileSystem, environment, processRunner, tools)
    { }


    public void Run(FilePath inputFile, T settings) => Run(settings, GetArguments(inputFile, settings));


    protected override IEnumerable<string> GetToolExecutableNames() => throw new NotImplementedException();

    protected override IEnumerable<string> GetToolExecutableNames(T settings)
    {
        return settings.RunWithBundler
            ? new[] { "bundle", "bundle.bat" }
            : new[] { ToolExecuteableName, $"{ToolExecuteableName}.bat" };
    }

    protected abstract ProcessArgumentBuilder GetArguments(FilePath inputFile, T settings);

    protected ProcessArgumentBuilder GetCommonArguments(FilePath inputFile, AsciidoctorSettingsBase settings)
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
            .AppendSwitchQuotedIfNotNull("--base-dir", settings.BaseDirectory)
            .AppendSwitchQuotedIfNotNull("--safe-mode", settings.SafeMode)
            .AppendSwitchQuotedIfNotNull("--destination-dir", settings.DestinationDirectory)
            .AppendIf("--embedded", settings.Embedded)
            .AppendSwitchQuotedIfNotNull("--load-path", settings.LoadPath)
            .AppendIf("--section-numbers", settings.SectionNumbers)
            .AppendSwitchQuotedIfNotNull("--out-file", settings.OutFile)
            .AppendSwitchQuotedIfNotNull("--source-dir", settings.SourceDirectory)
            .AppendSwitchQuotedIfNotNull("--template-dir", settings.TemplateDirectory)
            .AppendSwitchQuotedIfNotNull("--failure-level", settings.FailureLevel, upperCase: true)
            .AppendIf("--quiet", settings.Quiet)
            .AppendIf("--trace", settings.Trace)
            .AppendIf("--verbose", settings.Verbose)
            .AppendIf("--warnings", settings.Warnings)
            .AppendIf("--timings", settings.Timings);

        foreach (var library in settings.Require ?? Enumerable.Empty<string>())
        {
            argumentsBuilder.AppendSwitchQuoted("--require", library);
        }

        foreach (var attribute in settings.Attributes ?? Enumerable.Empty<AsciidoctorAttribute>())
        {
            argumentsBuilder.AppendSwitchQuoted("--attribute", attribute.ToString());
        }

        return argumentsBuilder;
    }
}
