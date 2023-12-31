﻿using System;
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

    protected override IEnumerable<string> GetToolExecutableNames(T settings) => new[] { $"{ToolExecuteableName}.bat", $"{ToolExecuteableName}.cmd", ToolExecuteableName };

    protected abstract ProcessArgumentBuilder GetArguments(FilePath inputFile, T settings);

    protected ProcessArgumentBuilder GetCommonArguments(FilePath inputFile, AsciidoctorSettingsBase settings)
    {
        var argumentsBuilder = new ProcessArgumentBuilder();

        argumentsBuilder
            .AppendQuoted(inputFile.ToString())
            .AppendSwitchQuotedIfNotNull("--base-dir", settings.BaseDirectory)
            .AppendSwitchQuotedIfNotNull("--safe-mode", settings.SafeMode)
            .AppendSwitchQuotedIfNotNull("--destination-dir", settings.DestinationDirectory)
            .AppendIf("--embedded", settings.Embedded)
            .AppendIf("--section-numbers", settings.SectionNumbers)
            .AppendSwitchQuotedIfNotNull("--out-file", settings.OutFile)
            .AppendSwitchQuotedIfNotNull("--source-dir", settings.SourceDirectory)
            .AppendSwitchQuotedIfNotNull("--failure-level", settings.FailureLevel, upperCase: true)
            .AppendIf("--quiet", settings.Quiet)
            .AppendIf("--trace", settings.Trace)
            .AppendIf("--verbose", settings.Verbose)
            .AppendIf("--warnings", settings.Warnings)
            .AppendIf("--timings", settings.Timings);


        foreach (var templateDirectory in settings.TemplateDirectories?.Distinct() ?? Enumerable.Empty<DirectoryPath>())
        {
            argumentsBuilder.AppendSwitchQuoted("--template-dir", templateDirectory);
        }

        foreach (var loadPath in settings.LoadPaths?.Distinct() ?? Enumerable.Empty<DirectoryPath>())
        {
            argumentsBuilder.AppendSwitchQuoted("--load-path", loadPath);
        }

        foreach (var library in settings.Require?.Distinct(StringComparer.OrdinalIgnoreCase) ?? Enumerable.Empty<string>()) //TODO: unique
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
