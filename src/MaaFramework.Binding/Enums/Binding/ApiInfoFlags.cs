namespace MaaFramework.Binding;

#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes

/// <summary>
///     Represents information about binding interoperable API.
/// </summary>
[Flags]
public enum ApiInfoFlags
{
    /// <summary>
    ///     No flags.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Indicates that the API is in MaaFramework context.
    /// </summary>
    InFrameworkContext = 1,

    /// <summary>
    ///     Indicates that the API is in MaaAgentServer context.
    /// </summary>
    InAgentServerContext = 2,

    /// <summary>
    ///    Indicates that the API uses the default resolver.
    /// </summary>
    UseDefaultResolver = 1 << 8,

    /// <summary>
    ///    Indicates that the API uses the resolver from binding.
    /// </summary>
    UseBindingResolver = 2 << 8,
}

internal static class ApiInfoFlagsExtensions
{
    internal const ApiInfoFlags ContextMask = ApiInfoFlags.InFrameworkContext | ApiInfoFlags.InAgentServerContext;
    internal const ApiInfoFlags ResolverMask = ApiInfoFlags.UseDefaultResolver | ApiInfoFlags.UseBindingResolver;

    internal static bool HasFlag_Context(this ApiInfoFlags flags) => (flags & ContextMask) != ApiInfoFlags.None;
    internal static bool HasFlag_ResolverExcept(this ApiInfoFlags flags, ApiInfoFlags other) => (flags & ~other & ResolverMask) != ApiInfoFlags.None;
}
