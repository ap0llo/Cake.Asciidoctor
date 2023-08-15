using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Asciidoctor.Test;

/// <summary>
/// Tests for <see cref="AsciidoctorRunner"/>
/// </summary>
public class AsciidoctorRunnerTest : AsciidoctorRunnerTestBase<AsciidoctorSettings>
{
    private readonly FilePath m_AsciiDoctorPath;
    private readonly FilePath m_BundlerPath;


    public AsciidoctorRunnerTest()
    {
        m_AsciiDoctorPath = m_FileSystem.CreateFile(m_Environment.WorkingDirectory.CombineWithFilePath("asciidoctor")).Path;
        m_ToolLocator.RegisterFile(m_AsciiDoctorPath);

        m_BundlerPath = m_FileSystem.CreateFile(m_Environment.WorkingDirectory.CombineWithFilePath("bundle")).Path;
        m_ToolLocator.RegisterFile(m_BundlerPath);
    }


    public static IEnumerable<object[]> AsciidoctorArgumentTestCases()
    {
        object[] TestCase(string id, string inputFile, AsciidoctorSettings settings, IEnumerable<string> expectedArguments) => new object[] { id, inputFile, settings, expectedArguments };

        //
        // --backend
        //
        yield return TestCase(
            id: "T201",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Backend = "some-backend"
            },
            expectedArguments: new[] { "\"input.adoc\"", "--backend \"some-backend\"" }
        );

        //
        // --doctype
        //
        foreach (var doctype in Enum.GetValues<AsciidoctorDoctype>())
        {
            yield return TestCase(
                id: $"T202-{doctype}",
                inputFile: "input.adoc",
                settings: new()
                {
                    Doctype = doctype
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--doctype {doctype.ToString().ToLower()}" }
            );
        }

        //
        // --template-engine
        //
        yield return TestCase(
            id: "T203",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                TemplateEngine = "my-template-engine"
            },
            expectedArguments: new[] { "\"input.adoc\"", "--template-engine \"my-template-engine\"" }
        );
    }


    [Theory]
    [MemberData(nameof(CommonArgumentTestCases))]
    [MemberData(nameof(AsciidoctorArgumentTestCases))]
    public void Run_starts_Asciidoctor_with_expected_arguments(string id, string inputFile, AsciidoctorSettings settings, IEnumerable<string> expectedArguments)
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
                Assert.Equal(m_AsciiDoctorPath, x.FilePath);
                Assert.Collection(
                    x.Settings.Arguments.Select(x => x.Render()),
                    expectedArguments.Select<string, Action<string>>(expected => actual => Assert.Equal(expected, actual)).ToArray()
                );
            }
        );
    }
}
