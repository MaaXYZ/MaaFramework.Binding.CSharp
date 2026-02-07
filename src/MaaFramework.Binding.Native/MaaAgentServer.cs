using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaAgentServer;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaAgentServer"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaAgentServer : IMaaAgentServer
{
    /// <summary>
    ///     Gets the unique identifier used to communicate with the agent client.
    /// </summary>
    public static string CurrentId { get; protected set; }

    /// <summary>
    ///    Gets the current <see cref="MaaAgentServer"/> instance.
    /// </summary>
    public static MaaAgentServer Current { get; protected set; }

    /// <summary>
    ///     Creates a <see cref="MaaAgentServer"/> instance.
    /// </summary>
    protected MaaAgentServer()
    {
        // DO NOT add any MaaAgentServerAPI.
        // Due to WithNativeLibrary().

        MaaEventCallbacks = new Dictionary<MaaHandleType, MaaEventCallback>(4)
        {
            [MaaHandleType.Tasker] = (handle, message, detailsJson, transArg) => InvokeCallback(new MaaTasker(handle), new MaaCallbackEventArgs<MaaTaskerHandle>(handle, message, detailsJson, (MaaHandleType)transArg)),
            [MaaHandleType.Resource] = (handle, message, detailsJson, transArg) => InvokeCallback(new MaaResource(handle), new MaaCallbackEventArgs<MaaResourceHandle>(handle, message, detailsJson, (MaaHandleType)transArg)),
            [MaaHandleType.Controller] = (handle, message, detailsJson, transArg) => InvokeCallback(new MaaController(handle), new MaaCallbackEventArgs<MaaControllerHandle>(handle, message, detailsJson, (MaaHandleType)transArg)),
            [MaaHandleType.Context] = (handle, message, detailsJson, transArg) => InvokeCallback(new MaaContext(handle), new MaaCallbackEventArgs<MaaContextHandle>(handle, message, detailsJson, (MaaHandleType)transArg)),
        };
    }

    static MaaAgentServer()
    {
        NativeBindingContext.SwitchToAgentServerContext();
        CurrentId = string.Empty;
        Current = new();
    }

    /// <inheritdoc/>
    public event EventHandler<MaaCallbackEventArgs>? Callback
    {
        add
        {
            EnsureCallbacksRegistered();
            PrivateCallback += value;
        }
        remove => PrivateCallback -= value;
    }

    private void EnsureCallbacksRegistered()
    {
        if (_isRegistered)
            return;

        try
        {
            _isRegistered = true;
            _ = MaaAgentServerAddTaskerSink(MaaEventCallbacks[MaaHandleType.Tasker], (nint)MaaHandleType.Tasker).ThrowIfEquals(MaaDef.MaaInvalidId);
            _ = MaaAgentServerAddResourceSink(MaaEventCallbacks[MaaHandleType.Resource], (nint)MaaHandleType.Resource).ThrowIfEquals(MaaDef.MaaInvalidId);
            _ = MaaAgentServerAddControllerSink(MaaEventCallbacks[MaaHandleType.Controller], (nint)MaaHandleType.Controller).ThrowIfEquals(MaaDef.MaaInvalidId);
            _ = MaaAgentServerAddContextSink(MaaEventCallbacks[MaaHandleType.Context], (nint)MaaHandleType.Context).ThrowIfEquals(MaaDef.MaaInvalidId);
        }
        catch
        {
            _isRegistered = false;
            throw;
        }
    }

    private bool _isStarted;
    private bool _isRegistered;
    private event EventHandler<MaaCallbackEventArgs>? PrivateCallback;
    private readonly MaaMarshaledApiRegistry<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApiRegistry<MaaCustomRecognitionCallback> _recognitions = new();

    /// <summary>
    ///     Gets the delegates to avoid garbage collection before MaaFramework calls.
    /// </summary>
    protected IReadOnlyDictionary<MaaHandleType, MaaEventCallback> MaaEventCallbacks
    {
        get;
        set
        {
            if (_isRegistered)
                throw new InvalidOperationException("Callbacks are already registered.");
            field = value;
        }
    }

    /// <summary>
    ///     Raises the Callback event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An object that contains the event data.</param>
    protected void InvokeCallback(object? sender, MaaCallbackEventArgs e)
        => PrivateCallback?.Invoke(sender, e);

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{GetType().Name} {{ {nameof(CurrentId)} = {CurrentId}, CustomActions = [{string.Join(", ", _actions.Names)}] , CustomRecognitions = [{string.Join(" & ", _recognitions.Names)}] }}";

    IMaaAgentServer IMaaAgentServer.WithIdentifier(string identifier) => WithIdentifier(identifier);
    IMaaAgentServer IMaaAgentServer.Register<T>(string name, T custom) => Register(name, custom);
    IMaaAgentServer IMaaAgentServer.Register<T>(string? name) => Register<T>(name);
    IMaaAgentServer IMaaAgentServer.Register<T>(T custom) => Register(custom);
    IMaaAgentServer IMaaAgentServer.StartUp() => StartUp();
    IMaaAgentServer IMaaAgentServer.ShutDown() => ShutDown();
    IMaaAgentServer IMaaAgentServer.Join() => Join();
    IMaaAgentServer IMaaAgentServer.Detach() => Detach();

    /// <inheritdoc cref="IMaaAgentServer.WithIdentifier"/>
    public MaaAgentServer WithIdentifier(string identifier)
    {
        if (_isStarted)
            throw new InvalidOperationException($"""MaaAgentServer has started up and cannot modify identifier to "${identifier}".""");
        CurrentId = identifier;
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.Register{T}(string, T)"/>
    public MaaAgentServer Register<T>(string name, T custom) where T : IMaaCustom
    {
        custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc/>
    public MaaAgentServer Register<T>(string? name = null) where T : IMaaCustom, new()
    {
        var custom = new T();
        if (name != null)
            custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc cref="IMaaAgentServer.Register{T}(T)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerRegisterCustomAction"/> and <see cref="MaaAgentServerRegisterCustomRecognition"/>.
    /// </remarks>
    public MaaAgentServer Register<T>(T custom) where T : IMaaCustom
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
        _isStarted = MaaAgentServerStartUp(CurrentId).ThrowIfFalse();
        return this;
    }

    /// <inheritdoc cref="IMaaAgentServer.ShutDown"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerShutDown"/>.
    /// </remarks>
    public MaaAgentServer ShutDown()
    {
        MaaAgentServerShutDown();
        _isStarted = false;
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

#pragma warning disable CA1707 // 标识符不应包含下划线

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
    /// <remarks>
    ///     Uses this method before other methods inclue events; otherwise, throws an <see cref="InvalidOperationException"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">NativeLibrary is already loaded.</exception>
    public static MaaAgentServer WithNativeLibrary(this MaaAgentServer server, params string[] paths)
    {
        NativeBindingContext.AppendNativeLibrarySearchPaths(paths);
        return server;
    }
}
