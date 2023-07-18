using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Asciidoctor.Test.TestHelpers;

public class FakeToolLocator : IToolLocator
{
    private readonly StringComparer m_FileNameComparer;
    private readonly List<FilePath> m_Tools = new();

    public FakeToolLocator(ICakeEnvironment environment)
    {
        m_FileNameComparer = environment.Platform.Family switch
        {
            PlatformFamily.Windows => StringComparer.OrdinalIgnoreCase,
            _ => StringComparer.Ordinal
        };
    }


    public void RegisterFile(FilePath path) => m_Tools.Add(path);

    public FilePath? Resolve(string tool) => m_Tools.FirstOrDefault(path => m_FileNameComparer.Equals(path.GetFilename().ToString(), tool));

    public FilePath? Resolve(IEnumerable<string> toolExeNames) => toolExeNames.Select(Resolve).FirstOrDefault(x => x is not null);
}
