using Cake.Core.Tooling;

namespace Cake.Asciidoctor;

public abstract class AsciidoctorSettingsBase : ToolSettings
{
    public bool RunWithBundler { get; set; }
}
