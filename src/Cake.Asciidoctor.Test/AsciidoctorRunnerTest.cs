using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cake.Asciidoctor.Test.TestHelpers;
using Cake.Core.IO;
using Cake.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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


    public static IEnumerable<object[]> ArgumentTestCases()
    {
        object[] TestCase(string id, string inputFile, AsciidoctorSettings settings, IEnumerable<string> expectedArguments)
        {
            return new object[] { id, inputFile, settings, expectedArguments };
        }

        yield return TestCase(
            id: "T01",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings(),
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        yield return TestCase(
            id: "T02",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                RunWithBundler = false
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        yield return TestCase(
            id: "T03",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                RunWithBundler = true
            },
            expectedArguments: new[] { "exec", "asciidoctor", "\"input.adoc\"" }
        );

    }


    [Theory]
    [MemberData(nameof(ArgumentTestCases))]
    public void Run_starts_AsciiDoctor_with_expected_arguments(string id, string inputFile, AsciidoctorSettings settings, IEnumerable<string> expectedArguments)
    {
        // ARRANGE
        _ = id;
        var sut = new AsciidoctorRunner(m_FileSystem, m_Environment, m_ProcessRunner, m_ToolLocator);

        // ACT
        sut.Run(inputFile, settings);

        // ASSERT
        Assert.Collection(
            m_ProcessRunner.StartedProcesses,
            x =>
            {
                if (settings.RunWithBundler == true)
                {
                    Assert.Equal(m_BundlerPath, x.FilePath);
                }
                else
                {
                    Assert.Equal(m_AsciiDoctorPath, x.FilePath);
                }

                Assert.Collection(
                    x.Settings.Arguments.Select(x => x.Render()),
                    expectedArguments.Select<string, Action<string>>(expected => actual => Assert.Equal(expected, actual)).ToArray()
                );
            }
        );
    }
}
