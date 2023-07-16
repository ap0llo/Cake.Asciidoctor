using System;
using System.Collections.Generic;
using System.Linq;
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


        yield return TestCase(
            id: "T04",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                BaseDirectory = "some-directory"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--base-dir=some-directory\"" }
        );

        foreach (var safeMode in Enum.GetValues<AsciidoctorSafeMode>())
        {
            yield return TestCase(
                id: $"T05-{safeMode}",
                inputFile: "input.adoc",
                settings: new AsciidoctorSettings()
                {
                    SafeMode = safeMode
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--safe-mode={safeMode.ToString().ToLower()}" }
            );
        }


        yield return TestCase(
            id: "T06",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Backend = "some-backend"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--backend=some-backend\"" }
        );

        foreach (var doctype in Enum.GetValues<AsciidoctorDoctype>())
        {
            yield return TestCase(
                id: $"T07-{doctype}",
                inputFile: "input.adoc",
                settings: new AsciidoctorSettings()
                {
                    Doctype = doctype
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--doctype={doctype.ToString().ToLower()}" }
            );
        }

        yield return TestCase(
            id: "T08",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                DestinationDirectory = "some-destination-directory"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--destination-dir=some-destination-directory\"" }
        );

        yield return TestCase(
            id: "T09",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                TemplateEngine = "my-template-engine"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--template-engine=my-template-engine\"" }
        );

        yield return TestCase(
            id: "T10",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Embedded = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--embedded" }
        );

        yield return TestCase(
            id: "T11",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                LoadPath = "some-load-path"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--load-path=some-load-path\"" }
        );

        yield return TestCase(
            id: "T12",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                SectionNumbers = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--section-numbers" }
        );

        yield return TestCase(
            id: "T13",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                OutFile = "directory/output.html"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--out-file=directory/output.html\"" }
        );

        yield return TestCase(
            id: "T14",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                SourceDirectory = "some-source-directory"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--source-dir=some-source-directory\"" }
        );

        yield return TestCase(
            id: "T15",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Require = new[] { "library1" }
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--require=library1\"" }
        );

        yield return TestCase(
            id: "T16",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Require = new[] { "library1", "library2" }
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--require=library1\"", "\"--require=library2\"" }
        );

        yield return TestCase(
            id: "T17",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                NoHeaderFooter = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--no-header-footer" }
        );

        yield return TestCase(
            id: "T18",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                TemplateDirectory = "some-template-dir"
            },
            expectedArguments: new[] { "\"input.adoc\"", "\"--template-dir=some-template-dir\"" }
        );

        foreach (var failureLevel in Enum.GetValues<AsciidoctorFailureLevel>())
        {
            yield return TestCase(
                id: $"T19-{failureLevel}",
                inputFile: "input.adoc",
                settings: new AsciidoctorSettings()
                {
                    FailureLevel = failureLevel
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--failure-level={failureLevel.ToString().ToUpper()}" }
            );
        }

        yield return TestCase(
            id: "T20",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Quiet = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--quiet" }
        );

        yield return TestCase(
            id: "T21",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Trace = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--trace" }
        );

        yield return TestCase(
            id: "T22",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Verbose = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--verbose" }
        );

        yield return TestCase(
            id: "T23",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Warnings = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--warnings" }
        );

        yield return TestCase(
            id: "T23",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Timings = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--timings" }
        );

        {
            var settings = new AsciidoctorSettings();
            settings.Attributes.Define("Some-Attribute");

            yield return TestCase(
                id: "T24",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[] { "\"input.adoc\"", "\"--attribute=Some-Attribute\"" }
            );
        }
        {
            var settings = new AsciidoctorSettings();
            settings.Attributes.Define("Some-Attribute");
            settings.Attributes.Define("Some-Other-Attribute");

            yield return TestCase(
                id: "T25",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[]
                    {
                    "\"input.adoc\"",
                    "\"--attribute=Some-Attribute\"" ,
                    "\"--attribute=Some-Other-Attribute\""
                }
            );
        }

        {
            var settings = new AsciidoctorSettings()
            {
                Attributes = new()
                {
                    { "Some-Attribute","Some Value" },
                    { "Another-Attribute",  "Another Value"}
                }
            };

            yield return TestCase(
                id: "T27",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[]
                {
                    "\"input.adoc\"",
                    "\"--attribute=Some-Attribute=\"Some Value\"\"" ,
                    "\"--attribute=Another-Attribute=\"Another Value\"\""
                }
            );
        }


        {
            var settings = new AsciidoctorSettings();
            settings.Attributes["Some-Attribute"] = "Some Value";
            settings.Attributes.Define("Some-Other-Attribute");
            settings.Attributes["Yet Another Attribute"] = "Value";

            yield return TestCase(
                id: "T27",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[]
                {
                    "\"input.adoc\"",
                    "\"--attribute=Some-Attribute=\"Some Value\"\"" ,
                    "\"--attribute=Some-Other-Attribute\"",
                    "\"--attribute=Yet Another Attribute=Value\""
                }
            );
        }

        {
            var settings = new AsciidoctorSettings();
            settings.Attributes.Unset("Some-Attribute");

            yield return TestCase(
                id: "T28",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[] { "\"input.adoc\"", "\"--attribute=Some-Attribute!\"" }
            );
        }

        {
            var settings = new AsciidoctorSettings();
            settings.Attributes.Add("Some-Attribute", "Value", AsciidoctorAttributeOptions.NoOverride);

            yield return TestCase(
                id: "T29",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[] { "\"input.adoc\"", "\"--attribute=Some-Attribute@=Value\"" }
            );
        }


        // Set "Require" to null (not recommended but the runner should still be able to handle it)
        yield return TestCase(
            id: "T30",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Require = null!
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        // Set "Attributes" to null (not recommended but the runner should still be able to handle it)
        yield return TestCase(
            id: "T30",
            inputFile: "input.adoc",
            settings: new AsciidoctorSettings()
            {
                Attributes = null!
            },
            expectedArguments: new[] { "\"input.adoc\"" }
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
