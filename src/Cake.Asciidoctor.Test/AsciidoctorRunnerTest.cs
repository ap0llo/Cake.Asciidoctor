using System.Runtime.InteropServices;
using Cake.Asciidoctor.Test.TestHelpers;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Asciidoctor.Test;

/// <summary>
/// Tests for <see cref="AsciidoctorRunner"/>
/// </summary>
public class AsciidoctorRunnerTest
{
    private readonly FakeEnvironment m_Environment;
    private readonly FakeFileSystem m_FileSystem;
    private readonly FakeProcessRunner m_ProcessRunner;
    private readonly FakeToolLocator m_ToolLocator;

    private readonly FilePath m_AsciiDoctorPath;
    private readonly FilePath m_BundlerPath;


    public AsciidoctorRunnerTest()
    {
        m_Environment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? FakeEnvironment.CreateWindowsEnvironment()
            : FakeEnvironment.CreateUnixEnvironment();
        m_FileSystem = new FakeFileSystem(m_Environment);
        m_ProcessRunner = new FakeProcessRunner();
        m_ToolLocator = new FakeToolLocator(m_Environment);

        m_AsciiDoctorPath = m_FileSystem.CreateFile(m_Environment.WorkingDirectory.CombineWithFilePath("asciidoctor")).Path;
        m_ToolLocator.RegisterFile(m_AsciiDoctorPath);

        m_BundlerPath = m_FileSystem.CreateFile(m_Environment.WorkingDirectory.CombineWithFilePath("bundle")).Path;
        m_ToolLocator.RegisterFile(m_BundlerPath);
    }


    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    [InlineData(false)]
    public void Run_starts_AsciiDoctor_with_expected_arguments(bool? runWithBundler)
    {
        // ARRANGE
        var settings = new AsciidoctorSettings();
        if (runWithBundler.HasValue)
        {
            settings.RunWithBundler = runWithBundler.Value;
        }
        var sut = new AsciidoctorRunner(m_FileSystem, m_Environment, m_ProcessRunner, m_ToolLocator, settings);

        // ACT
        sut.Run("input.adoc");

        // ASSERT
        Assert.Collection(
            m_ProcessRunner.StartedProcesses,
            x =>
            {
                if (runWithBundler == true)
                {
                    Assert.Equal(m_BundlerPath, x.FilePath);
                    Assert.Equal("exec asciidoctor \"input.adoc\"", x.Settings.Arguments.Render());
                }
                else
                {
                    Assert.Equal(m_AsciiDoctorPath, x.FilePath);
                    Assert.Equal("\"input.adoc\"", x.Settings.Arguments.Render());
                }
            }
        );
    }
}
