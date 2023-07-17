using System.Runtime.InteropServices;
using System.Text;

namespace MaaCommon.Interop
{
    public class MaaProxy
    {
        private class MaaAPI
        {
            public delegate void MaaCallbackDelegate(IntPtr msg, IntPtr details_json, IntPtr callback_arg);

            /* Resource */

            [DllImport("MaaFramework")]
            public static unsafe extern IntPtr MaaResourceCreate(IntPtr user_path, MaaCallbackDelegate callback, IntPtr callback_arg);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaResourceDestroy(IntPtr res_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern Int64 MaaResourcePostResource(IntPtr res_handle, IntPtr path);
            [DllImport("MaaFramework")]
            public static unsafe extern Int32 MaaResourceStatus(IntPtr res_handle, Int64 id);
            [DllImport("MaaFramework")]
            public static unsafe extern Int32 MaaResourceWait(IntPtr res_handle, Int64 id);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaResourceLoaded(IntPtr res_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaResourceSetOption(IntPtr res_handle, Int32 option, IntPtr value, UInt64 value_size);
            [DllImport("MaaFramework")]
            public static unsafe extern UInt64 MaaResourceGetHash(IntPtr res_handle, IntPtr buff, UInt64 buff_size);

            /* Controller */

            [DllImport("MaaFramework")]
            public static unsafe extern IntPtr MaaAdbControllerCreate(IntPtr adb_path, IntPtr address, Int32 type,
                   IntPtr config, MaaCallbackDelegate callback,
                   IntPtr callback_arg);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaControllerDestroy(IntPtr ctrl_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaControllerSetOption(IntPtr ctrl_handle, Int32 option, IntPtr value, UInt64 value_size);
            [DllImport("MaaFramework")]
            public static unsafe extern Int64 MaaControllerPostConnection(IntPtr ctrl_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern Int64 MaaControllerPostClick(IntPtr ctrl_handle, Int32 x, Int32 y);
            [DllImport("MaaFramework")]
            public static unsafe extern Int64 MaaControllerPostSwipe(IntPtr ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, UInt64 buff_size);
            [DllImport("MaaFramework")]
            public static unsafe extern Int64 MaaControllerPostScreencap(IntPtr ctrl_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern Int32 MaaControllerStatus(IntPtr ctrl_handle, Int64 id);
            [DllImport("MaaFramework")]
            public static unsafe extern Int32 MaaControllerWait(IntPtr ctrl_handle, Int64 id);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaControllerConnected(IntPtr ctrl_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern UInt64 MaaControllerGetImage(IntPtr ctrl_handle, IntPtr buff, UInt64 buff_size);
            [DllImport("MaaFramework")]
            public static unsafe extern UInt64 MaaControllerGetUUID(IntPtr ctrl_handle, IntPtr buff, UInt64 buff_size);

            /* Instance */

            [DllImport("MaaFramework")]
            public static unsafe extern IntPtr MaaInstanceCreate(MaaCallbackDelegate callback, IntPtr callback_arg);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaInstanceDestroy(IntPtr inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaInstanceSetOption(IntPtr inst_handle, Int32 option, IntPtr value, UInt64 value_size);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaBindResource(IntPtr inst_handle, IntPtr resource_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaBindController(IntPtr inst_handle, IntPtr controller_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaInited(IntPtr inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern void MaaRegisterCustomTask(IntPtr inst_handle, IntPtr name, IntPtr task);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaUnregisterCustomTask(IntPtr inst_handle, IntPtr name);
            [DllImport("MaaFramework")]
            public static unsafe extern void MaaClearCustomTask(IntPtr inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern Int64 MaaInstancePostTask(IntPtr inst_handle, IntPtr name, IntPtr args);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaSetTaskParam(IntPtr inst_handle, Int64 id, IntPtr args);
            [DllImport("MaaFramework")]
            public static unsafe extern Int32 MaaTaskStatus(IntPtr inst_handle, Int64 id);
            [DllImport("MaaFramework")]
            public static unsafe extern Int32 MaaTaskWait(IntPtr inst_handle, Int64 id);
            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaTaskAllFinished(IntPtr inst_handle);

            [DllImport("MaaFramework")]
            public static unsafe extern void MaaStop(IntPtr inst_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern IntPtr MaaGetResource(IntPtr inst_handle);
            [DllImport("MaaFramework")]
            public static unsafe extern IntPtr MaaGetController(IntPtr inst_handle);

            /* Utils */

            [DllImport("MaaFramework")]
            public static unsafe extern Byte MaaSetGlobalOption(Int32 option, IntPtr value, UInt64 value_size);

            [DllImport("MaaFramework")]
            public static unsafe extern IntPtr MaaVersion();
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

        public delegate void MaaCallbackDelegate(string msg, string details_json, IntPtr callback_arg);

        private static MaaAPI.MaaCallbackDelegate WrapCallback(MaaCallbackDelegate callback)
        {
            return (msg, detail, arg) => { callback(Marshal.PtrToStringUTF8(msg) ?? "", Marshal.PtrToStringUTF8(detail) ?? "{}", arg); };
        }

        public static IntPtr MaaResourceCreate(string user_path, MaaCallbackDelegate callback, IntPtr callback_arg)
        {
            var user_path_native = new NativeString(user_path);
            return MaaAPI.MaaResourceCreate(user_path_native.str, WrapCallback(callback), callback_arg);
        }

        public static void MaaResourceDestroy(IntPtr res_handle)
        {
            MaaResourceDestroy(res_handle);
        }

        public static Int64 MaaResourcePostResource(IntPtr res_handle, string path)
        {
            var path_native = new NativeString(path);
            return MaaAPI.MaaResourcePostResource(res_handle, path_native.str);
        }

        public static Int32 MaaResourceStatus(IntPtr res_handle, Int64 id)
        {
            return MaaAPI.MaaResourceStatus(res_handle, id);
        }

        public static Int32 MaaResourceWait(IntPtr res_handle, Int64 id)
        {
            return MaaAPI.MaaResourceWait(res_handle, id);
        }

        public static bool MaaResourceLoaded(IntPtr res_handle)
        {
            return 0 != MaaAPI.MaaResourceLoaded(res_handle);
        }

        // 没有可选的Option，乐
        // public static bool MaaResourceSetOption(IntPtr res_handle, Int32 option, IntPtr value, UInt64 value_size)

        // 没人实现
        // public static string MaaResourceGetHash(IntPtr res_handle, UInt64 buffer_size);

        public static IntPtr MaaAdbControllerCreate(string adb_path, string address, AdbControllerType type, string config, MaaCallbackDelegate callback, IntPtr callback_arg)
        {
            var adb_path_native = new NativeString(adb_path);
            var address_native = new NativeString(address);
            var config_native = new NativeString(config);
            return MaaAPI.MaaAdbControllerCreate(adb_path_native.str, address_native.str, (Int32)type, config_native.str, WrapCallback(callback), callback_arg);
        }

        public static void MaaControllerDestroy(IntPtr handle)
        {
            MaaAPI.MaaControllerDestroy(handle);
        }

        public static bool MaaControllerSetScreenshotTargetWidth(IntPtr handle, int width)
        {
            return 0 != MaaAPI.MaaControllerSetOption(handle, (Int32)CtrlOption.ScreenshotTargetWidth, width, sizeof(int));
        }

        public static bool MaaControllerSetScreenshotTargetHeight(IntPtr handle, int height)
        {
            return 0 != MaaAPI.MaaControllerSetOption(handle, (Int32)CtrlOption.ScreenshotTargetHeight, height, sizeof(int));
        }

        public static bool MaaControllerSetDefaultAppPackageEntry(IntPtr handle, string entry)
        {
            var entry_native = new NativeString(entry);
            return 0 != MaaAPI.MaaControllerSetOption(handle, (Int32)CtrlOption.DefaultAppPackageEntry, entry_native.str, (UInt64)entry_native.len);
        }

        public static bool MaaControllerSetDefaultAppPackage(IntPtr handle, string package)
        {
            var package_native = new NativeString(package);
            return 0 != MaaAPI.MaaControllerSetOption(handle, (Int32)CtrlOption.DefaultAppPackage, package_native.str, (UInt64)package_native.len);
        }

        public static Int64 MaaControllerPostConnection(IntPtr ctrl_handle)
        {
            return MaaAPI.MaaControllerPostConnection(ctrl_handle);
        }

        public static Int64 MaaControllerPostClick(IntPtr ctrl_handle, Int32 x, Int32 y)
        {
            return MaaAPI.MaaControllerPostClick(ctrl_handle, x, y);
        }

        // public static  Int64 MaaControllerPostSwipe(IntPtr ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, UInt64 buff_size);

        public static Int64 MaaControllerPostScreencap(IntPtr ctrl_handle)
        {
            return MaaAPI.MaaControllerPostScreencap(ctrl_handle);
        }

        public static Int32 MaaControllerStatus(IntPtr ctrl_handle, Int64 id)
        {
            return MaaAPI.MaaControllerStatus(ctrl_handle, id);
        }

        public static Int32 MaaControllerWait(IntPtr ctrl_handle, Int64 id)
        {
            return MaaAPI.MaaControllerWait(ctrl_handle, id);
        }

        public static bool MaaControllerConnected(IntPtr ctrl_handle)
        {
            return 0 != MaaAPI.MaaControllerConnected(ctrl_handle);
        }

        public static byte[]? MaaControllerGetImage(IntPtr ctrl_handle, int buffer_size = 3 << 20)
        {
            IntPtr buf = Marshal.AllocHGlobal(buffer_size);
            UInt64 size = MaaAPI.MaaControllerGetImage(ctrl_handle, buf, (UInt64)buffer_size);
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

        public static string? MaaControllerGetUUID(IntPtr ctrl_handle, int buffer_size = 64) // 16
        {
            IntPtr buf = Marshal.AllocHGlobal(buffer_size);
            UInt64 size = MaaAPI.MaaControllerGetUUID(ctrl_handle, buf, (UInt64)buffer_size);
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

        public static IntPtr MaaInstanceCreate(MaaCallbackDelegate callback, IntPtr callback_arg)
        {
            return MaaAPI.MaaInstanceCreate(WrapCallback(callback), callback_arg);
        }

        public static void MaaInstanceDestroy(IntPtr inst_handle)
        {
            MaaAPI.MaaInstanceDestroy(inst_handle);
        }

        // public static bool MaaInstanceSetOption(IntPtr inst_handle, Int32 option, IntPtr value, UInt64 value_size);

        public static bool MaaBindResource(IntPtr inst_handle, IntPtr resource_handle)
        {
            return 0 != MaaAPI.MaaBindResource(inst_handle, resource_handle);
        }

        public static bool MaaBindController(IntPtr inst_handle, IntPtr controller_handle)
        {
            return 0 != MaaAPI.MaaBindController(inst_handle, controller_handle);
        }

        public static bool MaaInited(IntPtr inst_handle)
        {
            return 0 != MaaAPI.MaaInited(inst_handle);
        }

        // public static void MaaRegisterCustomTask(IntPtr inst_handle, IntPtr name, IntPtr task)
        // public static void MaaUnregisterCustomTask(IntPtr inst_handle, IntPtr name);
        // public static void MaaClearCustomTask(IntPtr inst_handle);

        public static Int64 MaaInstancePostTask(IntPtr inst_handle, string name, string args)
        {
            var name_native = new NativeString(name);
            var args_native = new NativeString(args);
            return MaaAPI.MaaInstancePostTask(inst_handle, name_native.str, args_native.str);
        }

        public static bool MaaSetTaskParam(IntPtr inst_handle, Int64 id, string args)
        {
            var args_native = new NativeString(args);
            return 0 != MaaAPI.MaaSetTaskParam(inst_handle, id, args_native.str);
        }

        public static Int32 MaaTaskStatus(IntPtr inst_handle, Int64 id)
        {
            return MaaAPI.MaaTaskStatus(inst_handle, id);
        }

        public static Int32 MaaTaskWait(IntPtr inst_handle, Int64 id)
        {
            return MaaAPI.MaaTaskWait(inst_handle, id);
        }

        public static bool MaaTaskAllFinished(IntPtr inst_handle)
        {
            return 0 != MaaAPI.MaaTaskAllFinished(inst_handle);
        }

        public static void MaaStop(IntPtr inst_handle)
        {
            MaaAPI.MaaStop(inst_handle);
        }

        public static IntPtr MaaGetResource(IntPtr inst_handle)
        {
            return MaaAPI.MaaGetResource(inst_handle);
        }

        public static IntPtr MaaGetController(IntPtr inst_handle)
        {
            return MaaAPI.MaaGetController(inst_handle);
        }

        public static bool MaaSetLogging(string path)
        {
            var path_native = new NativeString(path);
            return 0 != MaaAPI.MaaSetGlobalOption((Int32)GlobalOption.Logging, path_native.str, (UInt64)path_native.len);
        }

        public static string MaaVersion()
        {
            return Marshal.PtrToStringUTF8(MaaAPI.MaaVersion()) ?? "";
        }

    }
}
