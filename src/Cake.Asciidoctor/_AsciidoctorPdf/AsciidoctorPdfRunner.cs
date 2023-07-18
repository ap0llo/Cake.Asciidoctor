using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

internal class AsciidoctorPdfRunner : AsciidoctorTool<AsciidoctorPdfSettings>
{
    protected override string ToolExecuteableName => "asciidoctor-pdf";


    public AsciidoctorPdfRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
        : base(fileSystem, environment, processRunner, tools)
    { }


    protected override ProcessArgumentBuilder GetArguments(FilePath inputFile, AsciidoctorPdfSettings settings)
    {
        var arguments = GetCommonArguments(inputFile, settings);

        arguments.AppendSwitchQuotedIfNotNull("--doctype", settings.Doctype);

        return arguments;
    }

    protected override string GetToolName() => "Asciidoctor PDF";
}
