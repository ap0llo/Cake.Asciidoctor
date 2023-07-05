using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Asciidoctor;

internal static class ProcessArgumentBuilderExtensions
{
    public static ProcessArgumentBuilder AppendOption(this ProcessArgumentBuilder argumentsBuilder, string name, string value) => argumentsBuilder.Append($"--{name}={value}");

    public static ProcessArgumentBuilder AppendOptionQuoted(this ProcessArgumentBuilder argumentsBuilder, string name, string value) => argumentsBuilder.AppendQuoted($"--{name}={value}");

    public static ProcessArgumentBuilder AppendOptionQuoted(this ProcessArgumentBuilder argumentsBuilder, string name, Path value) => argumentsBuilder.AppendQuoted($"--{name}={value}");

    public static ProcessArgumentBuilder AppendOptionIfNotNull(this ProcessArgumentBuilder argumentsBuilder, string name, string? value) =>
        value is null ? argumentsBuilder : argumentsBuilder.AppendOptionQuoted(name, value);

    public static ProcessArgumentBuilder AppendOptionIfNotNull(this ProcessArgumentBuilder argumentsBuilder, string name, Path? value) =>
        value is null ? argumentsBuilder : argumentsBuilder.AppendOptionQuoted(name, value);

    public static ProcessArgumentBuilder AppendOptionIfNotNull<T>(this ProcessArgumentBuilder argumentsBuilder, string name, Nullable<T> value, bool upperCase = false) where T : struct, Enum
    {
        if (value.HasValue)
        {
            var stringValue = value.Value.ToString();
            stringValue = upperCase ? stringValue.ToUpper() : stringValue.ToLower();
            return argumentsBuilder.AppendOption(name, stringValue);
        }

        return argumentsBuilder;
    }

    public static ProcessArgumentBuilder AppendSwitchIf(this ProcessArgumentBuilder argumentsBuilder, string name, bool condition)
    {
        if (condition)
        {
            return argumentsBuilder.Append($"--{name}");
        }

        return argumentsBuilder;
    }
}
