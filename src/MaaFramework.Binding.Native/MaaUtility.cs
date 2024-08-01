using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaUtility;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaUtility"/>.
/// </summary>
public class MaaUtility : IMaaUtility
{
    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaVersion"/>.
    /// </remarks>
    public string Version => MaaVersion().ToStringUtf8();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    public bool SetOption<T>(GlobalOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var optValue = (value, opt) switch
        {
            (int vvvv, GlobalOption.StdoutLevel) => vvvv.ToMaaOptionValue(),
            (string v, GlobalOption.LogDir) => v.ToMaaOptionValue(),
            (bool vvv, GlobalOption.SaveDraw
                    or GlobalOption.Recording
                    or GlobalOption.ShowHitDraw) => vvv.ToMaaOptionValue(),

            (LoggingLevel v, GlobalOption.StdoutLevel) => ((int)v).ToMaaOptionValue(),

            _ => throw new InvalidOperationException(),
        };

        return MaaSetGlobalOption((MaaGlobalOption)opt, optValue, (MaaOptionValueSize)optValue.Length).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaQueryTaskDetail"/>.
    /// </remarks>
    public bool QueryTaskDetail(MaaTaskId taskId, out string entry, out MaaNodeId[] nodeIdList)
    {
        entry = string.Empty;
        nodeIdList = [];
        MaaSize nodeIdListSize = 0;
        using var buffer = new MaaStringBuffer();
        if (!MaaQueryTaskDetail(taskId, buffer.Handle, null, ref nodeIdListSize).ToBoolean())
            return false;

        entry = buffer.ToString();
        nodeIdList = new MaaNodeId[nodeIdListSize];
        return MaaQueryTaskDetail(taskId, buffer.Handle, nodeIdList, ref nodeIdListSize).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaQueryNodeDetail"/>.
    /// </remarks>
    public bool QueryNodeDetail(MaaNodeId nodeId, out string name, out MaaRecoId recognitionId, out bool runCompleted)
    {
        using var buffer = new MaaStringBuffer();
        var ret = MaaQueryNodeDetail(nodeId, buffer.Handle, out recognitionId, out var runCompletedByte).ToBoolean();
        name = buffer.ToString();
        runCompleted = runCompletedByte.ToBoolean();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaQueryRecognitionDetail"/>.
    /// </remarks>
    public bool QueryRecognitionDetail<T>(MaaRecoId recognitionId, out string name, out bool hit, IMaaRectBuffer? hitBox, out string detailJson, T? raw, IMaaList<T>? draws)
        where T : IMaaImageBuffer, new()
    {
        var hitBoxHandle = (hitBox as IMaaRectBuffer<nint>)?.Handle ?? MaaRectHandle.Zero;
        var rawHandle = (raw as IMaaImageBuffer<nint>)?.Handle ?? MaaImageBufferHandle.Zero;
        var drawsHandle = (draws as IMaaList<nint, T>)?.Handle ?? MaaImageListBufferHandle.Zero;

        using var nameBuffer = new MaaStringBuffer();
        using var detailJsonBuffer = new MaaStringBuffer();

        var ret = MaaQueryRecognitionDetail(recognitionId, nameBuffer.Handle, out var hitByte, hitBoxHandle, detailJsonBuffer.Handle, rawHandle, drawsHandle).ToBoolean();
        name = nameBuffer.ToString();
        hit = hitByte.ToBoolean();
        detailJson = detailJsonBuffer.ToString();
        return ret;
    }
}
