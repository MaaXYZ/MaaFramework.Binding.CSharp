using System.Runtime.InteropServices;
using System.Text;

namespace MaaCommon.Interop
{
    using MaaAdbControllerType = Int32;
    using MaaBool = Byte;
    using MaaCallbackTransparentArg = IntPtr;
    using MaaControllerHandle = IntPtr;
    using MaaId = Int64;
    using MaaInstanceHandle = IntPtr;
    using MaaOption = Int32;
    using MaaOptionValue = IntPtr;
    using MaaOptionValueSize = UInt64;
    using MaaResourceHandle = IntPtr;
    using MaaSize = UInt64;
    using MaaStatus = Int32;
    using MaaString = IntPtr;

    public class MaaProxy
    {
        private delegate void MaaCallbackDelegate(MaaString msg, MaaString details_json, MaaCallbackTransparentArg callback_arg);

        /* Resource */

        [DllImport("MaaFramework")]
        private static unsafe extern MaaResourceHandle MaaResourceCreate(MaaString user_path, MaaCallbackDelegate callback, MaaCallbackTransparentArg callbac_arg);
        [DllImport("MaaFramework")]
        private static unsafe extern void MaaResourceDestroy(MaaResourceHandle* res_handle);

        [DllImport("MaaFramework")]
        private static unsafe extern MaaId MaaResourcePostResource(MaaResourceHandle res_handle, MaaString path);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaStatus MaaResourceStatus(MaaResourceHandle res_handle, MaaId id);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaStatus MaaResourceWait(MaaResourceHandle res_handle, MaaId id);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaResourceLoaded(MaaResourceHandle res_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaResourceSetOption(MaaResourceHandle res_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaSize MaaResourceGetHash(MaaResourceHandle res_handle, byte* buff, MaaSize buff_size);

        /* Controller */

        [DllImport("MaaFramework")]
        private static unsafe extern MaaControllerHandle MaaAdbControllerCreate(MaaString adb_path, MaaString address, MaaAdbControllerType type,
               MaaString config, MaaCallbackDelegate callback,
               MaaCallbackTransparentArg callback_arg);
        [DllImport("MaaFramework")]
        private static unsafe extern void MaaControllerDestroy(MaaControllerHandle* ctrl_handle);

        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaControllerSetOption(MaaControllerHandle ctrl_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaId MaaControllerPostConnection(MaaControllerHandle ctrl_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaId MaaControllerPostClick(MaaControllerHandle ctrl_handle, Int32 x, Int32 y);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaId MaaControllerPostSwipe(MaaControllerHandle ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, MaaSize buff_size);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaId MaaControllerPostScreencap(MaaControllerHandle ctrl);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaStatus MaaControllerStatus(MaaControllerHandle ctrl_handle, MaaId id);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaStatus MaaControllerWait(MaaControllerHandle ctrl_handle, MaaId id);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaControllerConnected(MaaControllerHandle ctrl_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaSize MaaControllerGetImage(MaaControllerHandle ctrl_handle, byte* buff, MaaSize buff_size);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaSize MaaControllerGetUUID(MaaControllerHandle ctrl_handle, byte* buff, MaaSize buff_size);

        /* Instance */

        [DllImport("MaaFramework")]
        private static unsafe extern MaaInstanceHandle MaaInstanceCreate(MaaCallbackDelegate callback, MaaCallbackTransparentArg callback_arg);
        [DllImport("MaaFramework")]
        private static unsafe extern void MaaInstanceDestroy(MaaInstanceHandle* inst_handle);

        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaInstanceSetOption(MaaInstanceHandle inst_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaBindResource(MaaInstanceHandle inst_handle, MaaResourceHandle resource_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaBindController(MaaInstanceHandle inst_handle, MaaControllerHandle controller_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaInited(MaaInstanceHandle inst_handle);

        [DllImport("MaaFramework")]
        private static unsafe extern void MaaRegisterCustomTask(MaaInstanceHandle inst_handle, MaaString name, IntPtr task);
        [DllImport("MaaFramework")]
        private static unsafe extern void MaaUnregisterCustomTask(MaaInstanceHandle inst_handle, MaaString name);
        [DllImport("MaaFramework")]
        private static unsafe extern void MaaClearCustomTask(MaaInstanceHandle inst_handle);

        [DllImport("MaaFramework")]
        private static unsafe extern MaaId MaaInstancePostTask(MaaInstanceHandle inst_handle, MaaString name, MaaString args);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaSetTaskParam(MaaInstanceHandle inst_handle, MaaId id, MaaString args);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaStatus MaaTaskStatus(MaaInstanceHandle inst_handle, MaaId id);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaStatus MaaTaskWait(MaaInstanceHandle inst_handle, MaaId id);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaTaskAllFinished(MaaInstanceHandle inst_handle);

        [DllImport("MaaFramework")]
        private static unsafe extern void MaaStop(MaaInstanceHandle inst_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaResourceHandle MaaGetResource(MaaInstanceHandle inst_handle);
        [DllImport("MaaFramework")]
        private static unsafe extern MaaControllerHandle MaaGetController(MaaInstanceHandle inst_handle);

        /* Utils */

        [DllImport("MaaFramework")]
        private static unsafe extern MaaBool MaaSetGlobalOption(MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);

        [DllImport("MaaFramework")]
        private static unsafe extern MaaString MaaVersion();
        class NativeString
        {
            public IntPtr str;
            public int len;

            public NativeString(string s)
            {
                len = Encoding.UTF8.GetByteCount(s);
                byte[] buffer = new byte[len + 1];
                Encoding.UTF8.GetBytes(s, 0, s.Length, buffer, 0);
                str = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, str, buffer.Length);
            }

            ~NativeString()
            {
                Marshal.FreeHGlobal(str);
            }
        }

        public delegate void CallbackDelegate(string msg, string details_json, MaaCallbackTransparentArg callback_arg);
		public static MaaControllerHandle AdbControllerCreate(string adb_path, string address, MaaAdbControllerType type, string config, CallbackDelegate callback, MaaCallbackTransparentArg callback_arg)
        {
            var adb_path_native = new NativeString(adb_path);
            var address_native = new NativeString(address);
            var config_native = new NativeString(config);
            unsafe
            {
                return MaaAdbControllerCreate(adb_path_native.str, address_native.str, type, config_native.str, (msg, detail, arg) =>
                {
                    callback(Marshal.PtrToStringUTF8(msg) ?? "", Marshal.PtrToStringUTF8(detail) ?? "{}", arg);
                }, callback_arg);
            }
        }

        public static void ControllerDestroy(MaaControllerHandle handle)
        {
            unsafe
            {
                /*
                int len = sizeof(MaaControllerHandle);
                IntPtr buf = Marshal.AllocHGlobal(len);
                MaaControllerHandle[] cache = { handle };
                Marshal.Copy(cache, 0, buf, 1);
                */
                // 下面这玩意真的能用吗
                MaaInstanceDestroy(&handle);
            }
        }

        public static MaaId ControllerPostConnection(MaaControllerHandle ctrl_handle)
        {
            return MaaControllerPostConnection(ctrl_handle);
        }

        public static MaaId ControllerPostClick(MaaControllerHandle ctrl_handle, Int32 x, Int32 y)
        {
            return MaaControllerPostClick(ctrl_handle, x, y);
        }

        public static MaaStatus ControllerWait(MaaControllerHandle ctrl_handle, MaaId id)
        {
            return MaaControllerWait(ctrl_handle, id);
        }

        public static bool SetLogging(string path)
        {
            var path_native = new NativeString(path);
            unsafe
            {
                // TODO: Use enum
                return 0 != MaaSetGlobalOption(1, path_native.str, (ulong)path_native.len);
            }
        }

        public static string Version()
        {
            unsafe
            {
                return Marshal.PtrToStringUTF8(MaaVersion()) ?? "";
            }
        }

    }
}
