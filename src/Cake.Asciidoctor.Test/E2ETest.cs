using System;
using System.Text;
using System.Threading.Tasks;
using Cake.Asciidoctor.Test.TestHelpers;
using CliWrap;
using Grynwald.Utilities.IO;
using Xunit;
using Xunit.Abstractions;

namespace Cake.Asciidoctor.Test;

public class E2ETest
{
    private readonly ITestOutputHelper m_TestOutputHelper;

    public E2ETest(ITestOutputHelper testOutputHelper)
    {
        m_TestOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
    }

    [Theory]
    [InlineData("Asciidoctor", "asciidoctor.bat")]
    [InlineData("AsciidoctorPdf", "asciidoctor-pdf.bat")]
    public async Task Asciidoctor_alias_can_be_used_in_Cake_script(string aliasName, string toolScriptName)
    {
        // ARRANGE
        using var temporaryDirectory = CreateTestDirectory();

        temporaryDirectory.AddFile(
            "build.cake",
            $$"""
            #reference "{{typeof(AsciidoctorAliases).Assembly.Location}}"

            var target = Argument("target", "Default");

            Task("Default").Does(() =>
            {            
                {{aliasName}}("document.adoc");
            });

            RunTarget(target);

            """);

        temporaryDirectory.AddFile(
            ".config/dotnet-tools.json",
            """
            {
              "version": 1,
              "isRoot": true,
              "tools": {
                "cake.tool": {
                  "version": "3.0.0",
                  "commands": [
                    "dotnet-cake"
                  ]
                }
              }
            }
            """);

        // Add mock script so the test is independet of whether Asciidoctor or Asciidoctor PDF is actually is installed
        var id = Guid.NewGuid().ToString();
        temporaryDirectory.AddFile(
            $"tools/{toolScriptName}",
            @$"echo ""Script Output {id}"""
        );

        // Restore "dotnet cake" tool 
        await Cli.Wrap("dotnet")
            .WithArguments("tool restore")
            .WithWorkingDirectory(temporaryDirectory)
            .WithValidation(CommandResultValidation.ZeroExitCode)
            .ExecuteWithTestOutputAsync(m_TestOutputHelper);

        // ACT
        var stdOut = new StringBuilder();
        var result = await Cli.Wrap("dotnet")
            .WithArguments("cake")
            .WithWorkingDirectory(temporaryDirectory)
            .WithValidation(CommandResultValidation.ZeroExitCode)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(x => stdOut.AppendLine(x)))
            .ExecuteWithTestOutputAsync(m_TestOutputHelper);

        // ASSERT
        // Check whether Asciidoctor was started properly by searching for the unqiue id of the generated batch script in the Cake build output
        Assert.Contains($"Script Output {id}", stdOut.ToString());
    }

    [Theory]
    [InlineData("Asciidoctor", "asciidoctor.bat")]
    [InlineData("AsciidoctorPdf", "asciidoctor-pdf.bat")]
    public async Task Asciidoctor_alias_can_be_used_in_Cake_Frosting_project(string aliasName, string toolScriptName)
    {
        // ARRANGE
        using var temporaryDirectory = CreateTestDirectory();

        temporaryDirectory.AddFile(
            "Build.csproj",
            $$"""
            <Project Sdk="Microsoft.NET.Sdk">

                <PropertyGroup>
                    <OutputType>Exe</OutputType>
                    <TargetFramework>net7.0</TargetFramework>
                    <RunWorkingDirectory>$(MSBuildProjectDirectory)</RunWorkingDirectory>
                </PropertyGroup>

                <ItemGroup>
                    <PackageReference Include="Cake.Frosting" Version="3.0.0" />
                    <Reference Include="{{typeof(AsciidoctorAliases).Assembly.Location}}" />
                </ItemGroup>

            </Project>
            """);

        temporaryDirectory.AddFile(
            "Program.cs",
            $$"""
            using Cake.Core;
            using Cake.Frosting;
            using Cake.Asciidoctor;

            return new CakeHost()                
                .UseContext<BuildContext>()                
                .Run(args);

            public class BuildContext : FrostingContext
            {
                public BuildContext(ICakeContext context) : base(context)
                { }
            }

            [TaskName("Default")]
            public class DefaultTask : FrostingTask<BuildContext>
            {
                public override void Run(BuildContext context)
                {
                    context.{{aliasName}}("document.adoc");
                }
            }
            """);


        // Add mock script so the test is independet of whether Asciidoctor or Asciidoctor PDF is actually is installed
        var id = Guid.NewGuid().ToString();
        temporaryDirectory.AddFile(
            $"tools/{toolScriptName}",
            @$"echo ""Script Output {id}"""
        );

        // ACT
        var stdOut = new StringBuilder();
        var result = await Cli.Wrap("dotnet")
            .WithArguments("run")
            .WithWorkingDirectory(temporaryDirectory)
            .WithValidation(CommandResultValidation.ZeroExitCode)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(x => stdOut.AppendLine(x)))
            .ExecuteWithTestOutputAsync(m_TestOutputHelper);

        // ASSERT
        // Check whether Asciidoctor was started properly by searching for the unqiue id of the generated batch script in the Cake build output
        Assert.Contains($"Script Output {id}", stdOut.ToString());
    }


    private static TemporaryDirectory CreateTestDirectory()
    {
        var temporaryDirectory = new TemporaryDirectory();

        temporaryDirectory.AddFile(
            "nuget.config",
            """
            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
                <packageSources>
                    <clear />
                    <add key="nuget" value="https://api.nuget.org/v3/index.json" />
                </packageSources>
            </configuration>
            """);

        return temporaryDirectory;
    }
}
