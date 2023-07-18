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
            .AppendSwitchQuotedIfNotNull("--backend", settings.Backend)
            .AppendSwitchQuotedIfNotNull("--doctype", settings.Doctype)
            .AppendSwitchQuotedIfNotNull("--template-engine", settings.TemplateEngine)
            .AppendIf("--no-header-footer", settings.NoHeaderFooter);

        return argumentsBuilder;
    }
}
