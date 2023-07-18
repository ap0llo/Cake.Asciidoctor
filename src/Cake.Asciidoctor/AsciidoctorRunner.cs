﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

internal class AsciidoctorRunner : AsciidoctorTool<AsciidoctorSettings>
{
    protected override string ToolExecuteableName => "asciidoctor";


    public AsciidoctorRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
        : base(fileSystem, environment, processRunner, tools)
    { }


    protected override string GetToolName() => "Asciidoctor";


    protected override ProcessArgumentBuilder GetArguments(FilePath inputFile, AsciidoctorSettings settings)
    {
        var argumentsBuilder = GetCommonArguments(inputFile, settings);

        argumentsBuilder
            .AppendSwitchQuotedIfNotNull("--base-dir", settings.BaseDirectory)
            .AppendSwitchQuotedIfNotNull("--safe-mode", settings.SafeMode)
            .AppendSwitchQuotedIfNotNull("--backend", settings.Backend)
            .AppendSwitchQuotedIfNotNull("--doctype", settings.Doctype)
            .AppendSwitchQuotedIfNotNull("--destination-dir", settings.DestinationDirectory)
            .AppendSwitchQuotedIfNotNull("--template-engine", settings.TemplateEngine)
            .AppendIf("--embedded", settings.Embedded)
            .AppendSwitchQuotedIfNotNull("--load-path", settings.LoadPath)
            .AppendIf("--section-numbers", settings.SectionNumbers)
            .AppendSwitchQuotedIfNotNull("--out-file", settings.OutFile)
            .AppendSwitchQuotedIfNotNull("--source-dir", settings.SourceDirectory);

        foreach (var library in settings.Require ?? Enumerable.Empty<string>())
        {
            argumentsBuilder.AppendSwitchQuoted("--require", library);
        }

        argumentsBuilder
            .AppendIf("--no-header-footer", settings.NoHeaderFooter)
            .AppendSwitchQuotedIfNotNull("--template-dir", settings.TemplateDirectory)
            .AppendSwitchQuotedIfNotNull("--failure-level", settings.FailureLevel, upperCase: true)
            .AppendIf("--quiet", settings.Quiet)
            .AppendIf("--trace", settings.Trace)
            .AppendIf("--verbose", settings.Verbose)
            .AppendIf("--warnings", settings.Warnings)
            .AppendIf("--timings", settings.Timings);

        foreach (var attribute in settings.Attributes ?? Enumerable.Empty<AsciidoctorAttribute>())
        {
            argumentsBuilder.AppendSwitchQuoted("--attribute", attribute.ToString());
        }


        return argumentsBuilder;
    }
}
