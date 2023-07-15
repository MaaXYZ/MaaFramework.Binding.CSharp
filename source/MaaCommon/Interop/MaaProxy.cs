using System.Runtime.InteropServices;

namespace MaaCommon.Interop
{
    using MaaResourceHandle = IntPtr;
    using MaaControllerHandle = IntPtr;
    using MaaInstanceHandle = IntPtr;

    using MaaBool = byte;
    using MaaSize = UInt64;

    using MaaString = IntPtr;
    using MaaStatus = Int32;

    using MaaId = Int64;
    using MaaOption = Int32;
    using MaaOptionValue = IntPtr;
    using MaaOptionValueSize = UInt64;

    using MaaAdbControllerType = Int32;

    using MaaCallbackTransparentArg = IntPtr;

    public partial class MaaProxy
    {
        private delegate void MaaCallbackDelegate(MaaString msg, MaaString details_json, MaaCallbackTransparentArg callback_arg);

        /* Resource */

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaResourceHandle MaaResourceCreate(MaaString user_path, MaaCallbackDelegate callback, MaaCallbackTransparentArg callbac_arg);
        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaResourceDestroy(MaaResourceHandle* res_handle);

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaId MaaResourcePostResource(MaaResourceHandle res_handle, MaaString path);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaStatus MaaResourceStatus(MaaResourceHandle res_handle, MaaId id);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaStatus MaaResourceWait(MaaResourceHandle res_handle, MaaId id);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaResourceLoaded(MaaResourceHandle res_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaResourceSetOption(MaaResourceHandle res_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaSize MaaResourceGetHash(MaaResourceHandle res_handle, byte* buff, MaaSize buff_size);

        /* Controller */

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaControllerHandle MaaAdbControllerCreate(MaaString adb_path, MaaString address, MaaAdbControllerType type,
                                                   MaaString config, MaaCallbackDelegate callback,
                                                   MaaCallbackTransparentArg callback_arg);
        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaControllerDestroy(MaaControllerHandle* ctrl_handle);

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaControllerSetOption(MaaControllerHandle ctrl_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaId MaaControllerPostConnection(MaaControllerHandle ctrl_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaId MaaControllerPostClick(MaaControllerHandle ctrl_handle, Int32 x, Int32 y);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaId MaaControllerPostSwipe(MaaControllerHandle ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, MaaSize buff_size);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaId MaaControllerPostScreencap(MaaControllerHandle ctrl);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaStatus MaaControllerStatus(MaaControllerHandle ctrl_handle, MaaId id);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaStatus MaaControllerWait(MaaControllerHandle ctrl_handle, MaaId id);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaControllerConnected(MaaControllerHandle ctrl_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaSize MaaControllerGetImage(MaaControllerHandle ctrl_handle, byte* buff, MaaSize buff_size);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaSize MaaControllerGetUUID(MaaControllerHandle ctrl_handle, byte* buff, MaaSize buff_size);

        /* Instance */

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaInstanceHandle MaaInstanceCreate(MaaCallbackDelegate callback, MaaCallbackTransparentArg callback_arg);
        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaInstanceDestroy(MaaInstanceHandle* inst_handle);

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaInstanceSetOption(MaaInstanceHandle inst_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaBindResource(MaaInstanceHandle inst_handle, MaaResourceHandle resource_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaBindController(MaaInstanceHandle inst_handle, MaaControllerHandle controller_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaInited(MaaInstanceHandle inst_handle);

        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaRegisterCustomTask(MaaInstanceHandle inst_handle, MaaString name, IntPtr task);
        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaUnregisterCustomTask(MaaInstanceHandle inst_handle, MaaString name);
        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaClearCustomTask(MaaInstanceHandle inst_handle);

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaId MaaInstancePostTask(MaaInstanceHandle inst_handle, MaaString name, MaaString args);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaSetTaskParam(MaaInstanceHandle inst_handle, MaaId id, MaaString args);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaStatus MaaTaskStatus(MaaInstanceHandle inst_handle, MaaId id);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaStatus MaaTaskWait(MaaInstanceHandle inst_handle, MaaId id);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaTaskAllFinished(MaaInstanceHandle inst_handle);

        [LibraryImport("MaaFramework")]
        private static unsafe partial void MaaStop(MaaInstanceHandle inst_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaResourceHandle MaaGetResource(MaaInstanceHandle inst_handle);
        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaControllerHandle MaaGetController(MaaInstanceHandle inst_handle);

        /* Utils */

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaBool MaaSetGlobalOption(MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);

        [LibraryImport("MaaFramework")]
        private static unsafe partial MaaString MaaVersion();
    }
}