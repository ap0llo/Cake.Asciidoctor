using System;
using System.Collections.Generic;
using Cake.Asciidoctor.Test.TestHelpers;
using Cake.Core.IO;

namespace Cake.Asciidoctor.Test;

public abstract class AsciidoctorRunnerTestBase<T> : ToolTestBase where T : AsciidoctorSettingsBase, new()
{
    public static IEnumerable<object[]> CommonArgumentTestCases()
    {
        object[] TestCase(string id, string inputFile, T settings, IEnumerable<string> expectedArguments) => new object[] { id, inputFile, settings, expectedArguments };

        // 
        // Input file only, no options
        // 
        yield return TestCase(
            id: "T101",
            inputFile: "input.adoc",
            settings: new(),
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        //
        // Run with bundler
        //
        yield return TestCase(
            id: "T102",
            inputFile: "input.adoc",
            settings: new()
            {
                RunWithBundler = false
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        yield return TestCase(
            id: "T103",
            inputFile: "input.adoc",
            settings: new()
            {
                RunWithBundler = true
            },
            expectedArguments: new[] { "exec", "asciidoctor", "\"input.adoc\"" }
        );


        //
        // --base-dir
        //
        yield return TestCase(
            id: "T104",
            inputFile: "input.adoc",
            settings: new()
            {
                BaseDirectory = "some-directory"
            },
            expectedArguments: new[] { "\"input.adoc\"", "--base-dir \"some-directory\"" }
        );


        //
        // --safe-mode
        //
        foreach (var safeMode in Enum.GetValues<AsciidoctorSafeMode>())
        {
            yield return TestCase(
                id: $"T105-{safeMode}",
                inputFile: "input.adoc",
                settings: new()
                {
                    SafeMode = safeMode
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--safe-mode {safeMode.ToString().ToLower()}" }
            );
        }

        //
        // --destination-dir
        //
        yield return TestCase(
            id: "T106",
            inputFile: "input.adoc",
            settings: new()
            {
                DestinationDirectory = "some-destination-directory"
            },
            expectedArguments: new[] { "\"input.adoc\"", "--destination-dir \"some-destination-directory\"" }
        );

        //
        // --embedded
        //
        yield return TestCase(
            id: "T107",
            inputFile: "input.adoc",
            settings: new()
            {
                Embedded = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--embedded" }
        );

        //
        // --load-path
        //
        {
            var settings = new T();
            settings.LoadPaths.Add("some-load-path");

            yield return TestCase(
                id: "T108",
                inputFile: "input.adoc",
                settings,
                expectedArguments: new[] { "\"input.adoc\"", "--load-path \"some-load-path\"" }
            );

        }

        yield return TestCase(
            id: "T109",
            inputFile: "input.adoc",
            settings: new()
            {
                LoadPaths = new DirectoryPath[]
                {
                    "some-load-path"
                }
            },
            expectedArguments: new[] { "\"input.adoc\"", "--load-path \"some-load-path\"" }
        );


        yield return TestCase(
            id: "T110",
            inputFile: "input.adoc",
            settings: new()
            {
                LoadPaths = new DirectoryPath[]
                {
                    "some-load-path",
                    "another-load-path"
                }
            },
            expectedArguments: new[] { "\"input.adoc\"", "--load-path \"some-load-path\"", "--load-path \"another-load-path\"" }
        );

        // Set "LoadPaths" to null (not recommended but the runner should still be able to handle it)


        yield return TestCase(
            id: "T111",
            inputFile: "input.adoc",
            settings: new()
            {
                LoadPaths = null!
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );


        //
        // --section-numbers
        //
        yield return TestCase(
            id: "T112",
            inputFile: "input.adoc",
            settings: new()
            {
                SectionNumbers = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--section-numbers" }
        );

        //
        // --out-file
        //
        yield return TestCase(
            id: "T113",
            inputFile: "input.adoc",
            settings: new()
            {
                OutFile = "directory/output.html"
            },
            expectedArguments: new[] { "\"input.adoc\"", "--out-file \"directory/output.html\"" }
        );

        //
        // --source-dir
        //
        yield return TestCase(
            id: "T114",
            inputFile: "input.adoc",
            settings: new()
            {
                SourceDirectory = "some-source-directory"
            },
            expectedArguments: new[] { "\"input.adoc\"", "--source-dir \"some-source-directory\"" }
        );

        //
        // --require
        //
        yield return TestCase(
            id: "T115",
            inputFile: "input.adoc",
            settings: new()
            {
                Require = new[] { "library1" }
            },
            expectedArguments: new[] { "\"input.adoc\"", "--require \"library1\"" }
        );

        yield return TestCase(
            id: "T116",
            inputFile: "input.adoc",
            settings: new()
            {
                Require = new[] { "library1", "library2" }
            },
            expectedArguments: new[] { "\"input.adoc\"", "--require \"library1\"", "--require \"library2\"" }
        );

        // Set "Require" to null (not recommended but the runner should still be able to handle it)
        yield return TestCase(
            id: "T117",
            inputFile: "input.adoc",
            settings: new()
            {
                Require = null!
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        //
        // --template-dir
        //
        yield return TestCase(
            id: "T118",
            inputFile: "input.adoc",
            settings: new()
            {
                TemplateDirectories = new DirectoryPath[] { "some-template-dir" }
            },
            expectedArguments: new[] { "\"input.adoc\"", "--template-dir \"some-template-dir\"" }
        );

        {
            var settings = new T();
            settings.TemplateDirectories.Add("some-template-dir");
            settings.TemplateDirectories.Add("some-other-template-dir");

            yield return TestCase(
                id: "T119",
                inputFile: "input.adoc",
                settings,
                expectedArguments: new[] { "\"input.adoc\"", "--template-dir \"some-template-dir\"", "--template-dir \"some-other-template-dir\"" }
            );
        }

        // Set "TemplateDirectories" to null (not recommended but the runner should still be able to handle it)
        yield return TestCase(
            id: "T120",
            inputFile: "input.adoc",
            settings: new()
            {
                TemplateDirectories = null!
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );

        //
        // --failure-level
        //
        foreach (var failureLevel in Enum.GetValues<AsciidoctorFailureLevel>())
        {
            yield return TestCase(
                id: $"T121-{failureLevel}",
                inputFile: "input.adoc",
                settings: new()
                {
                    FailureLevel = failureLevel
                },
                expectedArguments: new[] { "\"input.adoc\"", $"--failure-level {failureLevel.ToString().ToUpper()}" }
            );
        }

        //
        // --quiet
        //
        yield return TestCase(
            id: "T122",
            inputFile: "input.adoc",
            settings: new()
            {
                Quiet = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--quiet" }
        );

        //
        // --trace
        //
        yield return TestCase(
            id: "T123",
            inputFile: "input.adoc",
            settings: new()
            {
                Trace = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--trace" }
        );


        //
        // --verbose
        //
        yield return TestCase(
            id: "T124",
            inputFile: "input.adoc",
            settings: new()
            {
                Verbose = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--verbose" }
        );

        //
        // --warnings
        //
        yield return TestCase(
            id: "T125",
            inputFile: "input.adoc",
            settings: new()
            {
                Warnings = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--warnings" }
        );

        //
        // --timings
        //
        yield return TestCase(
            id: "T126",
            inputFile: "input.adoc",
            settings: new()
            {
                Timings = true
            },
            expectedArguments: new[] { "\"input.adoc\"", "--timings" }
        );


        //
        // --attribute
        //
        {
            var settings = new T();
            settings.Attributes.Define("Some-Attribute");

            yield return TestCase(
                id: "T127",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[] { "\"input.adoc\"", "--attribute \"Some-Attribute\"" }
            );
        }

        {
            var settings = new T();
            settings.Attributes.Define("Some-Attribute");
            settings.Attributes.Define("Some-Other-Attribute");

            yield return TestCase(
                id: "T128",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[]
                    {
                    "\"input.adoc\"",
                    "--attribute \"Some-Attribute\"" ,
                    "--attribute \"Some-Other-Attribute\""
                }
            );
        }

        {
            var settings = new T()
            {
                Attributes = new()
                {
                    { "Some-Attribute","Some Value" },
                    { "Another-Attribute",  "Another Value"}
                }
            };

            yield return TestCase(
                id: "T129",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[]
                {
                    "\"input.adoc\"",
                    "--attribute \"Some-Attribute=\"Some Value\"\"" ,
                    "--attribute \"Another-Attribute=\"Another Value\"\""
                }
            );
        }

        {
            var settings = new T();
            settings.Attributes["Some-Attribute"] = "Some Value";
            settings.Attributes.Define("Some-Other-Attribute");
            settings.Attributes["Yet Another Attribute"] = "Value";

            yield return TestCase(
                id: "T130",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[]
                {
                    "\"input.adoc\"",
                    "--attribute \"Some-Attribute=\"Some Value\"\"" ,
                    "--attribute \"Some-Other-Attribute\"",
                    "--attribute \"Yet Another Attribute=Value\""
                }
            );
        }

        {
            var settings = new T();
            settings.Attributes.Unset("Some-Attribute");

            yield return TestCase(
                id: "T131",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[] { "\"input.adoc\"", "--attribute \"Some-Attribute!\"" }
            );
        }

        {
            var settings = new T();
            settings.Attributes.Add("Some-Attribute", "Value", AsciidoctorAttributeOptions.NoOverride);

            yield return TestCase(
                id: "T132",
                inputFile: "input.adoc",
                settings: settings,
                expectedArguments: new[] { "\"input.adoc\"", "--attribute \"Some-Attribute@=Value\"" }
            );
        }

        // Set "Attributes" to null (not recommended but the runner should still be able to handle it)
        yield return TestCase(
            id: "T133",
            inputFile: "input.adoc",
            settings: new()
            {
                Attributes = null!
            },
            expectedArguments: new[] { "\"input.adoc\"" }
        );
    }
}
