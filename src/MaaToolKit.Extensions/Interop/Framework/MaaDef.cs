global using MaaStringBufferHandle = nint;
global using MaaImageBufferHandle = nint;
global using MaaResourceHandle = nint;
global using MaaControllerHandle = nint;
global using MaaInstanceHandle = nint;

global using MaaBool = System.Byte;
global using MaaSize = System.UInt64;
// const MaaNullSize

global using MaaStringView = nint;

global using MaaStatus = System.Int32;
// enum MaaJobStatus

global using MaaId = System.Int64;
global using MaaCtrlId = System.Int64;
global using MaaResId = System.Int64;
global using MaaTaskId = System.Int64;
// const MaaInvalidId

global using MaaOption = System.Int32;
global using MaaOptionValue = System.Byte; // ref MaaOptionValue
global using MaaOptionValueSize = System.UInt64; // MaaOptionValue.Length

global using MaaGlobalOption = System.Int32;
// enum GlobalOption
global using MaaResOption = System.Int32;
// enum ResourceOption
global using MaaCtrlOption = System.Int32;
// enum ResourceOption
global using MaaInstOption = System.Int32;
// enum InstanceOption
// const MaaTaskParam_Empty
global using MaaAdbControllerType = System.Int32;
// enum AdbControllerType

global using MaaTransparentArg = nint;
global using MaaCallbackTransparentArg = nint;

global using MaaResourceCallback = MaaToolKit.Extensions.Interop.Framework.MaaApiCallback;
global using MaaControllerCallback = MaaToolKit.Extensions.Interop.Framework.MaaApiCallback;
global using MaaInstanceCallback = MaaToolKit.Extensions.Interop.Framework.MaaApiCallback;

global using MaaCustomControllerHandle = nint;
global using MaaCustomRecognizerHandle = nint;
global using MaaCustomActionHandle = nint;
global using MaaSyncContextHandle = nint;

global using int32_t = System.Int32;

// Consider using SafeHandle
using System.Runtime.InteropServices;

namespace MaaToolKit.Extensions.Interop.Framework;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class MaaDef
{
    #region include/MaaFramework/MaaAPI.h, version: v1.1.0.

    #endregion

    #region include/MaaFramework/MaaDef.h, version: v1.1.0.

    #endregion

    internal const MaaSize MaaNullSize = MaaSize.MaxValue;
    internal const MaaId MaaInvalidId = 0;
    internal const string EmptyMaaTaskParam = "{}";
}

public delegate void MaaApiCallback(MaaStringView msg, MaaStringView details_json, MaaCallbackTransparentArg callback_arg);

[StructLayout(LayoutKind.Sequential)]
public struct MaaRect : IMaaDefStruct
{
    public int32_t X;
    public int32_t Y;
    public int32_t Width;
    public int32_t Height;
}

public interface IMaaDefStruct
{

}
