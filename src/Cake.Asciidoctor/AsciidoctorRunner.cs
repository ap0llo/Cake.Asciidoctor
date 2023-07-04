using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;


internal class AsciidoctorRunner : Tool<AsciidoctorSettings>
{
    private readonly AsciidoctorSettings m_Settings;

    public AsciidoctorRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, AsciidoctorSettings settings)
        : base(fileSystem, environment, processRunner, tools)
    {
        m_Settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }


    public void Run(FilePath inputFile) => Run(m_Settings, GetArguments(inputFile));



    protected override string GetToolName() => "Asciidoctor";

    protected override IEnumerable<string> GetToolExecutableNames()
    {
        return m_Settings.RunWithBundler
            ? new[] { "bundle", "bundle.bat" }
            : new[] { "asciidoctor", $"asciidoctor.bat" };
    }


    private ProcessArgumentBuilder GetArguments(FilePath inputFile)
    {
        var argumentsBuilder = new ProcessArgumentBuilder();

        if (m_Settings.RunWithBundler)
        {
            argumentsBuilder
                .Append("exec")
                .Append("asciidoctor");
        }

        argumentsBuilder.AppendQuoted(inputFile.ToString());

        return argumentsBuilder;
    }

}
