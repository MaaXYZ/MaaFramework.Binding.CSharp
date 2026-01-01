using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MaaFramework.Binding.Interop.Native;

internal static class MaaMarshallingExtensions
{
    /// <inheritdoc cref="MaaInteroperationException.ThrowIf"/>
    internal static bool ThrowIfTrue(this bool condition, string message = "", [CallerArgumentExpression(nameof(condition))] string? operationExpression = null)
    {
        MaaInteroperationException.ThrowIf(condition, message, operationExpression);
        return false;
    }

    /// <inheritdoc cref="MaaInteroperationException.ThrowIfNot"/>
    internal static bool ThrowIfFalse(this bool condition, string message = "", [CallerArgumentExpression(nameof(condition))] string? operationExpression = null)
    {
        MaaInteroperationException.ThrowIfNot(condition, message, operationExpression);
        return true;
    }

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

    /// <inheritdoc cref="MaaMarshaller.ConvertToString(nint)"/>
    internal static string ToStringUtf8(this nint value)
        => MaaMarshaller.ConvertToString(value);

    /// <inheritdoc cref="MaaMarshaller.ConvertToString(nint, int)"/>
    internal static string ToStringUtf8(this nint value, int size)
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

    /// <inheritdoc cref="MaaMarshaller.ConvertToMaaOptionValue(nuint)"/>
    internal static byte[] ToMaaOptionValue(this nuint value)
        => MaaMarshaller.ConvertToMaaOptionValue(value);
}
