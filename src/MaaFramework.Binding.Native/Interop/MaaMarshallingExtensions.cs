using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MaaFramework.Binding.Interop.Native;

internal static class MaaMarshallingExtensions
{
    /// <inheritdoc cref="MaaInteroperationException.ThrowIf"/>
    internal static void ThrowIfTrue(this bool condition, string message = "", [CallerArgumentExpression(nameof(condition))] string? operationExpression = null)
        => MaaInteroperationException.ThrowIf(condition, message, operationExpression);

    /// <inheritdoc cref="MaaInteroperationException.ThrowIfNot"/>
    internal static void ThrowIfFalse(this bool condition, string message = "", [CallerArgumentExpression(nameof(condition))] string? operationExpression = null)
        => MaaInteroperationException.ThrowIfNot(condition, message, operationExpression);

    /// <inheritdoc cref="MaaInteroperationException.ThrowIfNull{T}"/>
    internal static T ThrowIfNull<T>([NotNull] this T? operation, string message = "", [CallerArgumentExpression(nameof(operation))] string? operationExpression = null)
    {
        MaaInteroperationException.ThrowIfNull(operation, message, operationExpression);
        return operation;
    }

    /// <inheritdoc cref="MaaInteroperationException.ThrowIf"/>
    internal static T ThrowIfEquals<T>(this T value, T other, string message = "", [CallerArgumentExpression(nameof(value))] string? operationExpression = null, [CallerArgumentExpression(nameof(other))] string? otherOperationExpression = null)
        where T : IEquatable<T>
    {
        operationExpression = $"'{operationExpression}' equals '{otherOperationExpression}'";
        MaaInteroperationException.ThrowIf(value.Equals(other), message, operationExpression);
        return value;
    }

    /// <inheritdoc cref="MaaInteroperationException.ThrowIfNot"/>
    internal static T ThrowIfNotEquals<T>(this T value, T other, string message = "", [CallerArgumentExpression(nameof(value))] string? operationExpression = null, [CallerArgumentExpression(nameof(other))] string? otherOperationExpression = null)
        where T : IEquatable<T>
    {
        operationExpression = $"'{operationExpression}' not equals '{otherOperationExpression}'";
        MaaInteroperationException.ThrowIfNot(value.Equals(other), message, operationExpression);
        return value;
    }

    /// <inheritdoc cref="MaaMarshaller.ConvertToString"/>
    internal static string ToStringUtf8(this nint value, MaaSize size = MaaSize.MinValue)
        => MaaMarshaller.ConvertToString(value, size);

    /// <inheritdoc cref="MaaMarshaller.ConvertToMaaOptionValue(int)"/>
    internal static byte[] ToMaaOptionValue(this int value)
        => MaaMarshaller.ConvertToMaaOptionValue(value);

    /// <inheritdoc cref="MaaMarshaller.ConvertToMaaOptionValue(bool)"/>
    internal static byte[] ToMaaOptionValue(this bool value)
        => MaaMarshaller.ConvertToMaaOptionValue(value);

    /// <inheritdoc cref="MaaMarshaller.ConvertToMaaOptionValue(string)"/>
    internal static byte[] ToMaaOptionValue(this string value)
        => MaaMarshaller.ConvertToMaaOptionValue(value);
}
