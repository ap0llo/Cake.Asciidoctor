using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Asciidoctor.Test.TestHelpers;

internal class FakeProcess : IProcess
{
    public void Dispose()
    { }

    public int GetExitCode() => 0;

    public IEnumerable<string> GetStandardError() => Enumerable.Empty<string>();

    public IEnumerable<string> GetStandardOutput() => Enumerable.Empty<string>();

    public void Kill()
    { }

    public void WaitForExit()
    { }

    public bool WaitForExit(int milliseconds) => true;
}
