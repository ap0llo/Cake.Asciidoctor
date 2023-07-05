using System;
using System.Collections.Generic;
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

        argumentsBuilder.AppendQuoted(inputFile.ToString());

        return argumentsBuilder;
    }

}
