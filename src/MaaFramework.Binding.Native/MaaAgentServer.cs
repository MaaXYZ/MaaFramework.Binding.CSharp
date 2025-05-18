﻿using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaAgentServer;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaAgentServer"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class MaaAgentServer : IMaaAgentServer
{
    /// <summary>
    ///     Gets the unique identifier used to communicate with the agent client.
    /// </summary>
    public static string CurrentId { get; private set; }

    /// <summary>
    ///    Gets the current <see cref="MaaAgentServer"/> instance.
    /// </summary>
    public static MaaAgentServer Current { get; }

    /// <summary>
    ///     Creates a <see cref="MaaAgentServer"/> instance.
    /// </summary>
    private MaaAgentServer() { }
    static MaaAgentServer()
    {
        NativeBindingContext.SwitchToAgentServerContext();
        CurrentId = string.Empty;
        Current = new();
    }

    private readonly MaaMarshaledApiRegistry<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApiRegistry<MaaCustomRecognitionCallback> _recognitions = new();

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{GetType().Name} {{ {nameof(CurrentId)} = {CurrentId}, CustomActions = [{string.Join(", ", _actions.Names)}] , CustomRecognitions = [{string.Join(" & ", _recognitions.Names)}] }}";

    IMaaAgentServer IMaaAgentServer.WithIdentifier(string identifier) => WithIdentifier(identifier);
    IMaaAgentServer IMaaAgentServer.Register<T>(string name, T custom) => Register(name, custom);
    IMaaAgentServer IMaaAgentServer.Register<T>(T custom) => Register(custom);
    IMaaAgentServer IMaaAgentServer.StartUp() => StartUp();
    IMaaAgentServer IMaaAgentServer.ShutDown() => ShutDown();
    IMaaAgentServer IMaaAgentServer.Join() => Join();
    IMaaAgentServer IMaaAgentServer.Detach() => Detach();

    /// <inheritdoc cref="IMaaAgentServer.WithIdentifier"/>
    public MaaAgentServer WithIdentifier(string identifier)
    {
        CurrentId = identifier;
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.Register{T}(string, T)"/>
    public MaaAgentServer Register<T>(string name, T custom) where T : IMaaCustomResource
    {
        custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc cref="IMaaAgentServer.Register{T}(T)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerRegisterCustomAction"/> and <see cref="MaaAgentServerRegisterCustomRecognition"/>.
    /// </remarks>
    public MaaAgentServer Register<T>(T custom) where T : IMaaCustomResource
    {
        var ret = custom switch
        {
            IMaaCustomAction res
                => MaaAgentServerRegisterCustomAction(res.Name, res.Convert(out var callback), nint.Zero)
                && _actions.Register(res.Name, callback),
            IMaaCustomRecognition res
                => MaaAgentServerRegisterCustomRecognition(res.Name, res.Convert(out var callback), nint.Zero)
                && _recognitions.Register(res.Name, callback),
            _
                => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
        };
        _ = ret.ThrowIfFalse();
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.StartUp"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerStartUp"/>.
    /// </remarks>
    public MaaAgentServer StartUp()
    {
        if (string.IsNullOrEmpty(CurrentId))
            throw new InvalidOperationException("Identifier is not set. Use 'WithIdentifier' method to set it.");
        _ = MaaAgentServerStartUp(CurrentId).ThrowIfFalse();
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.ShutDown"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerShutDown"/>.
    /// </remarks>
    public MaaAgentServer ShutDown()
    {
        MaaAgentServerShutDown();
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.Join"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerJoin"/>.
    /// </remarks>
    public MaaAgentServer Join()
    {
        MaaAgentServerJoin();
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.Detach"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerDetach"/>.
    /// </remarks>
    public MaaAgentServer Detach()
    {
        MaaAgentServerDetach();
        return this;
    }
}

/// <summary>
///     A static class providing extension methods for <see cref="MaaAgentServer"/>.
/// </summary>
public static class MaaAgentServerExtensions
{
    /// <returns></returns>
    /// <inheritdoc cref="IMaaToolkitConfig.InitOption"/>
    public static MaaAgentServer WithToolkitConfig_InitOption(this MaaAgentServer server, string userPath = nameof(Environment.CurrentDirectory), [StringSyntax("Json")] string defaultJson = "{}")
    {
        _ = MaaToolkit.Shared.Config.InitOption(userPath, defaultJson).ThrowIfFalse();
        return server;
    }

    /// <summary>
    ///     Configures the MaaAgentServer to use the specified native libraries.
    /// </summary>
    /// <param name="server">The server.</param>
    /// <param name="paths">The directory paths to search for native libraries.</param>
    public static MaaAgentServer WithNativeLibrary(this MaaAgentServer server, params string[] paths)
    {
        NativeBindingContext.AppendNativeLibrarySearchPaths(paths);
        return server;
    }
}
