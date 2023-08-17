using System.IO;
using Grynwald.Utilities.IO;

namespace Cake.Asciidoctor.Test.TestHelpers;

internal static class TemporaryDirectoryExtensions
{
    public static void AddFile(this TemporaryDirectory directory, string relativePath, string contents)
    {
        var path = Path.Combine(directory, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, contents);
    }
}
