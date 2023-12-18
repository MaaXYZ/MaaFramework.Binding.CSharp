using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Grpc.Interop;

#pragma warning disable CA1707 // 标识符不应包含下划线
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable S1104 // Fields should not have public accessibility

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomRecognizerApi" />.
/// </summary>
public static class MaaRecognizerApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">The MaaSyncContextHandle.</param>
    /// <param name="image_handle">The MaaImageBufferHandle.</param>
    /// <param name="task">The MaaStringView.</param>
    /// <param name="param">The MaaStringView.</param>
    /// <param name="arg">The MaaTransparentArg.</param>
    /// <param name="box">The MaaRectHandle.</param>
    /// <param name="detail">The MaaStringBufferHandle.</param>
    /// <returns></returns>
    public delegate bool Analyze(
        string context,
        string image_handle,
        string task,
        string param,
        nint arg,
        IMaaRectBuffer box,
        IMaaStringBuffer detail);
}

/// <summary>
///     MaaCustomRecognizerApi
/// </summary>
public class MaaCustomRecognizerApi : IMaaDef
{
    public required MaaRecognizerApi.Analyze Analyze { get; init; }
}
