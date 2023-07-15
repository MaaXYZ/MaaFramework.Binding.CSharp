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
        private class MaaAPI
        {
            public delegate void MaaCallbackDelegate(MaaString msg, MaaString details_json, MaaCallbackTransparentArg callback_arg);

            /* Resource */

            [DllImport("MaaFramework")]
            public static unsafe extern MaaResourceHandle MaaResourceCreate(MaaString user_path, MaaCallbackDelegate callback, MaaCallbackTransparentArg callbac_arg);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaResourceDestroy(MaaResourceHandle* res_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern MaaId MaaResourcePostResource(MaaResourceHandle res_handle, MaaString path);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaStatus MaaResourceStatus(MaaResourceHandle res_handle, MaaId id);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaStatus MaaResourceWait(MaaResourceHandle res_handle, MaaId id);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaResourceLoaded(MaaResourceHandle res_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaResourceSetOption(MaaResourceHandle res_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaSize MaaResourceGetHash(MaaResourceHandle res_handle, byte* buff, MaaSize buff_size);

            /* Controller */

            [DllImport("MaaFramework")]
            public static unsafe extern MaaControllerHandle MaaAdbControllerCreate(MaaString adb_path, MaaString address, MaaAdbControllerType type,
                   MaaString config, MaaCallbackDelegate callback,
                   MaaCallbackTransparentArg callback_arg);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaControllerDestroy(MaaControllerHandle* ctrl_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaControllerSetOption(MaaControllerHandle ctrl_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaId MaaControllerPostConnection(MaaControllerHandle ctrl_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaId MaaControllerPostClick(MaaControllerHandle ctrl_handle, Int32 x, Int32 y);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaId MaaControllerPostSwipe(MaaControllerHandle ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, MaaSize buff_size);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaId MaaControllerPostScreencap(MaaControllerHandle ctrl_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaStatus MaaControllerStatus(MaaControllerHandle ctrl_handle, MaaId id);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaStatus MaaControllerWait(MaaControllerHandle ctrl_handle, MaaId id);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaControllerConnected(MaaControllerHandle ctrl_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaSize MaaControllerGetImage(MaaControllerHandle ctrl_handle, IntPtr buff, MaaSize buff_size);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaSize MaaControllerGetUUID(MaaControllerHandle ctrl_handle, IntPtr buff, MaaSize buff_size);

            /* Instance */

            [DllImport("MaaFramework")]
            public static unsafe extern MaaInstanceHandle MaaInstanceCreate(MaaCallbackDelegate callback, MaaCallbackTransparentArg callback_arg);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaInstanceDestroy(MaaInstanceHandle* inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaInstanceSetOption(MaaInstanceHandle inst_handle, MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaBindResource(MaaInstanceHandle inst_handle, MaaResourceHandle resource_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaBindController(MaaInstanceHandle inst_handle, MaaControllerHandle controller_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaInited(MaaInstanceHandle inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern void MaaRegisterCustomTask(MaaInstanceHandle inst_handle, MaaString name, IntPtr task);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaUnregisterCustomTask(MaaInstanceHandle inst_handle, MaaString name);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaClearCustomTask(MaaInstanceHandle inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern MaaId MaaInstancePostTask(MaaInstanceHandle inst_handle, MaaString name, MaaString args);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaSetTaskParam(MaaInstanceHandle inst_handle, MaaId id, MaaString args);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaStatus MaaTaskStatus(MaaInstanceHandle inst_handle, MaaId id);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaStatus MaaTaskWait(MaaInstanceHandle inst_handle, MaaId id);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaTaskAllFinished(MaaInstanceHandle inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern void MaaStop(MaaInstanceHandle inst_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaResourceHandle MaaGetResource(MaaInstanceHandle inst_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern MaaControllerHandle MaaGetController(MaaInstanceHandle inst_handle);

            /* Utils */

            [DllImport("MaaFramework")]
            public static unsafe extern MaaBool MaaSetGlobalOption(MaaOption option, MaaOptionValue value, MaaOptionValueSize value_size);

            [DllImport("MaaFramework")]
            public static unsafe extern MaaString MaaVersion();
        }

        private class NativeString
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

        public enum GlobalOption
        {
            Logging = 1
        }

        public enum CtrlOption
        {
            ScreenshotTargetWidth = 1,
            ScreenshotTargetHeight = 2,
            DefaultAppPackageEntry = 3,
            DefaultAppPackage = 4,
        }

        [Flags]
        public enum AdbControllerType
        {
            Touch_Adb = 1,
            Touch_MiniTouch = 2,
            Touch_MaaTouch = 3,
            Touch_Mask = 0xFF,

            Key_Adb = 1 << 8,
            Key_MaaTouch = 2 << 8,
            Key_Mask = 0xFF00,

            Screencap_FastestWay = 1 << 16,
            Screencap_RawByNetcat = 2 << 16,
            Screencap_RawWithGzip = 3 << 16,
            Screencap_Encode = 4 << 16,
            Screencap_EncodeToFile = 5 << 16,
            Screencap_MinicapDirect = 6 << 16,
            Screencap_MinicapStream = 7 << 16,
            Screencap_Mask = 0xFF0000,

            Input_Preset_Adb = Touch_Adb | Key_Adb,
            Input_Preset_Minitouch = Touch_MiniTouch | Key_Adb,
            Input_Preset_Maatouch =
                Touch_MaaTouch | Key_MaaTouch,
        };

        public delegate void CallbackDelegate(string msg, string details_json, MaaCallbackTransparentArg callback_arg);
        public static MaaControllerHandle MaaAdbControllerCreate(string adb_path, string address, AdbControllerType type, string config, CallbackDelegate callback, MaaCallbackTransparentArg callback_arg)
        {
            var adb_path_native = new NativeString(adb_path);
            var address_native = new NativeString(address);
            var config_native = new NativeString(config);
            return MaaAPI.MaaAdbControllerCreate(adb_path_native.str, address_native.str, (MaaAdbControllerType)type, config_native.str, (msg, detail, arg) => { callback(Marshal.PtrToStringUTF8(msg) ?? "", Marshal.PtrToStringUTF8(detail) ?? "{}", arg); }, callback_arg);
        }

        public static void MaaControllerDestroy(MaaControllerHandle handle)
        {
            unsafe
            {
                // 下面这玩意真的能用吗 好吧，确实能用
                MaaAPI.MaaControllerDestroy(&handle);
            }
        }

        public static bool MaaControllerSetScreenshotTargetWidth(MaaControllerHandle handle, int width)
        {
            return 0 != MaaAPI.MaaControllerSetOption(handle, (MaaOption)CtrlOption.ScreenshotTargetWidth, width, sizeof(int));
        }

        public static bool MaaControllerSetScreenshotTargetHeight(MaaControllerHandle handle, int height)
        {
            return 0 != MaaAPI.MaaControllerSetOption(handle, (MaaOption)CtrlOption.ScreenshotTargetHeight, height, sizeof(int));
        }

        public static bool MaaControllerSetDefaultAppPackageEntry(MaaControllerHandle handle, string entry)
        {
            var entry_native = new NativeString(entry);
            return 0 != MaaAPI.MaaControllerSetOption(handle, (MaaOption)CtrlOption.DefaultAppPackageEntry, entry_native.str, (MaaOptionValueSize)entry_native.len);
        }

        public static bool MaaControllerSetDefaultAppPackage(MaaControllerHandle handle, string package)
        {
            var package_native = new NativeString(package);
            return 0 != MaaAPI.MaaControllerSetOption(handle, (MaaOption)CtrlOption.DefaultAppPackage, package_native.str, (MaaOptionValueSize)package_native.len);
        }

        public static MaaId MaaControllerPostConnection(MaaControllerHandle ctrl_handle)
        {
            return MaaAPI.MaaControllerPostConnection(ctrl_handle);
        }

        public static  MaaId MaaControllerPostClick(MaaControllerHandle ctrl_handle, Int32 x, Int32 y)
        {
            return MaaAPI.MaaControllerPostClick(ctrl_handle, x, y);
        }

        // public static  MaaId MaaControllerPostSwipe(MaaControllerHandle ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, MaaSize buff_size);

        public static MaaId MaaControllerPostScreencap(MaaControllerHandle ctrl_handle)
        {
            return MaaAPI.MaaControllerPostScreencap(ctrl_handle);
        }

        public static MaaStatus MaaControllerStatus(MaaControllerHandle ctrl_handle, MaaId id)
        {
            return MaaAPI.MaaControllerStatus(ctrl_handle, id);
        }

        public static MaaStatus MaaControllerWait(MaaControllerHandle ctrl_handle, MaaId id)
        {
            return MaaAPI.MaaControllerWait(ctrl_handle, id);
        }

        public static bool MaaControllerConnected(MaaControllerHandle ctrl_handle)
        {
            return 0 != MaaAPI.MaaControllerConnected(ctrl_handle);
        }

        public static byte[]? MaaControllerGetImage(MaaControllerHandle ctrl_handle, int buffer_size = 3 << 20)
        {
            IntPtr buf = Marshal.AllocHGlobal(buffer_size);
            MaaSize size = MaaAPI.MaaControllerGetImage(ctrl_handle, buf, (MaaSize)buffer_size);
            if (size == 0xFFFFFFFFFFFFFFFF)
            {
                Marshal.FreeHGlobal(buf);
                return null;
            }
            else
            {
                byte[] ret = new byte[size];
                Marshal.Copy(buf, ret, 0, (int)size);
                Marshal.FreeHGlobal(buf);
                return ret;
            }
        }

        public static string? MaaControllerGetUUID(MaaControllerHandle ctrl_handle, int buffer_size = 64) // 16
        {
            IntPtr buf = Marshal.AllocHGlobal(buffer_size);
            MaaSize size = MaaAPI.MaaControllerGetUUID(ctrl_handle, buf, (MaaSize)buffer_size);
            if (size == 0xFFFFFFFFFFFFFFFF)
            {
                Marshal.FreeHGlobal(buf);
                return null;
            }
            else
            {
                string? ret = Marshal.PtrToStringUTF8(buf);
                Marshal.FreeHGlobal(buf);
                return ret;
            }
        }

        public static bool SetLogging(string path)
        {
            var path_native = new NativeString(path);
            return 0 != MaaAPI.MaaSetGlobalOption((MaaOption)GlobalOption.Logging, path_native.str, (MaaOptionValueSize)path_native.len);
        }

        public static string Version()
        {
            return Marshal.PtrToStringUTF8(MaaAPI.MaaVersion()) ?? "";
        }

    }
}
