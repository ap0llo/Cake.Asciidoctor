using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Asciidoctor;

internal static class ProcessArgumentBuilderExtensions
{
    public static ProcessArgumentBuilder AppendIf(this ProcessArgumentBuilder argumentsBuilder, string name, bool condition)
    {
        if (condition)
        {
            return argumentsBuilder.Append(name);
        }

        return argumentsBuilder;
    }

    public static ProcessArgumentBuilder AppendSwitchQuoted(this ProcessArgumentBuilder argumentsBuilder, string name, Path value) =>
        argumentsBuilder.AppendSwitchQuoted(name, value.ToString());

    public static ProcessArgumentBuilder AppendSwitchQuotedIfNotNull(this ProcessArgumentBuilder argumentsBuilder, string name, Path? value) =>
        value is null ? argumentsBuilder : argumentsBuilder.AppendSwitchQuoted(name, value);

    public static ProcessArgumentBuilder AppendSwitchQuotedIfNotNull(this ProcessArgumentBuilder argumentsBuilder, string name, string? value) =>
        value is null ? argumentsBuilder : argumentsBuilder.AppendSwitchQuoted(name, value);

    public static ProcessArgumentBuilder AppendSwitchQuotedIfNotNull<T>(this ProcessArgumentBuilder argumentsBuilder, string name, T? value, bool upperCase = false) where T : struct, Enum
    {
        if (value.HasValue)
        {
            var stringValue = value.Value.ToString();
            stringValue = upperCase ? stringValue.ToUpper() : stringValue.ToLower();
            return argumentsBuilder.AppendSwitch(name, stringValue);
        }

        return argumentsBuilder;
    }
}
