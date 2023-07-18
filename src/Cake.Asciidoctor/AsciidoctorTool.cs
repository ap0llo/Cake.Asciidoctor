using System;
using System.Collections.Generic;
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



    protected override IEnumerable<string> GetToolExecutableNames() => throw new NotImplementedException();

    protected override IEnumerable<string> GetToolExecutableNames(T settings)
    {
        return settings.RunWithBundler
            ? new[] { "bundle", "bundle.bat" }
            : new[] { ToolExecuteableName, $"{ToolExecuteableName}.bat" };
    }

    public void Run(FilePath inputFile, T settings) => Run(settings, GetArguments(inputFile, settings));

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

        argumentsBuilder.AppendQuoted(inputFile.ToString());

        return argumentsBuilder;
    }

}
