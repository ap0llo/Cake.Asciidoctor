using System.Threading.Tasks;
using CliWrap;
using Xunit.Abstractions;

namespace Cake.Asciidoctor.Test;

internal static class CommandExtensions
{
    public static async Task<CommandResult> ExecuteWithTestOutputAsync(this Command command, ITestOutputHelper testOutputHelper)
    {
        return await command
            .WithStandardErrorPipe(PipeTarget.Merge(command.StandardErrorPipe, PipeTarget.ToDelegate(testOutputHelper.WriteLine)))
            .WithStandardOutputPipe(PipeTarget.Merge(command.StandardOutputPipe, PipeTarget.ToDelegate(testOutputHelper.WriteLine)))
            .ExecuteAsync();
    }
}
