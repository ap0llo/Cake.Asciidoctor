using System.Runtime.InteropServices;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Asciidoctor.Test.TestHelpers;

public abstract class ToolTestBase
{
    protected readonly FakeEnvironment m_Environment;
    protected readonly FakeFileSystem m_FileSystem;
    protected readonly FakeProcessRunner m_ProcessRunner;
    protected readonly FakeToolLocator m_ToolLocator;


    public ToolTestBase()
    {
        m_Environment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? FakeEnvironment.CreateWindowsEnvironment()
            : FakeEnvironment.CreateUnixEnvironment();
        m_FileSystem = new FakeFileSystem(m_Environment);
        m_ProcessRunner = new FakeProcessRunner();
        m_ToolLocator = new FakeToolLocator(m_Environment);
    }

}
