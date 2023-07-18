using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Asciidoctor.Test.TestHelpers;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Asciidoctor.Test;

/// <summary>
/// Tests for <see cref="AsciidoctorPdfRunner"/>
/// </summary>
public class AsciidoctorPdfRunnerTest : AsciidoctorRunnerTestBase<AsciidoctorPdfSettings>
{
    private readonly FilePath m_AsciiDoctorPdfPath;
    private readonly FilePath m_BundlerPath;


    public AsciidoctorPdfRunnerTest()
    {
        m_AsciiDoctorPdfPath = m_FileSystem.CreateFile(m_Environment.WorkingDirectory.CombineWithFilePath("asciidoctor-pdf")).Path;
        m_ToolLocator.RegisterFile(m_AsciiDoctorPdfPath);

        m_BundlerPath = m_FileSystem.CreateFile(m_Environment.WorkingDirectory.CombineWithFilePath("bundle")).Path;
        m_ToolLocator.RegisterFile(m_BundlerPath);
    }


    public static IEnumerable<object[]> AsciidoctorPdfArgumentTestCases()
    {
        object[] TestCase(string id, string inputFile, AsciidoctorPdfSettings settings, IEnumerable<string> expectedArguments) => new object[] { id, inputFile, settings, expectedArguments };

        //
        // --doc-type
        //
        foreach (var doctype in Enum.GetValues<AsciidoctorPdfDoctype>())
        {
            yield return TestCase(
                id: $"T07-{doctype}",
                inputFile: "input.adoc",
                settings: new()
                {
                    Doctype = doctype
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--doctype {doctype.ToString().ToLower()}" }
            );
        }
    }


    [Theory]
    [MemberData(nameof(CommonArgumentTestCases))]
    [MemberData(nameof(AsciidoctorPdfArgumentTestCases))]
    public void Run_starts_AsciidoctorPdf_with_expected_arguments(string id, string inputFile, AsciidoctorPdfSettings settings, IEnumerable<string> expectedArguments)
    {
        // ARRANGE
        _ = id;
        var sut = new AsciidoctorPdfRunner(m_FileSystem, m_Environment, m_ProcessRunner, m_ToolLocator);

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
                    Assert.Equal(m_AsciiDoctorPdfPath, x.FilePath);
                }

                Assert.Collection(
                    x.Settings.Arguments.Select(x => x.Render()),
                    expectedArguments.Select<string, Action<string>>(expected => actual => Assert.Equal(expected, actual)).ToArray()
                );
            }
        );
    }
}
