using System.Text.Json;
using System.Text.Json.Nodes;
using static MaaCommon.Interop.MaaProxy;

namespace MaaCommon.Interop
{

    internal class Resolution
    {
        public int width { set; get; } = 0;
        public int height { set; get; } = 0;
    }
    internal class UUIDGot
    {
        public string uuid { set; get; } = "";
    }

    internal class ResolutionGot
    {
        public Resolution resolution { set; get; } = new Resolution();
    }


    internal class ConnectSuccess
    {
        public string uuid { get; set; } = "";
        public Resolution resolution { get; set; } = new Resolution();
    }

    internal class ConnectFailed
    {
        public string why { get; set; } = "";
    }

    public interface MaaControllerResponser
    {
        IntPtr GetArg()
        {
            return 0;
        }

        void ConnectSuccess(string uuid, int width, int height) { }
        void ConnectFailed(string why) { }

        void UUIDGot(string uuid) { }
        void UUIDGetFailed() { }

        void ResolutionGot(int width, int height) { }
        void ResolutionGetFailed() { }

        void ScreencapInited() { }
        void ScreencapInitFailed() { }

        void TouchinputInited() { }
        void TouchinputInitFailed() { }
    }

    public class MaaController : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;

        private MaaController(IntPtr ctrl_handle)
        {
            handle = ctrl_handle;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                MaaControllerDestroy(handle);
                handle = IntPtr.Zero;
                disposed = true;
            }
        }

        ~MaaController()
        {
            Dispose(false);
        }

        public IntPtr GetHandle()
        {
            return handle;
        }

        // TODO: 也许封装一些高层接口？比如异步化之类的
        public static MaaController CreateAdb(string adb_path, string address, AdbControllerType type, string config, MaaControllerResponser resp)
        {
            IntPtr ctrl = MaaAdbControllerCreate(adb_path, address, type, config, (msg, details, arg) =>
            {
                switch (msg)
                {
                    case "Controller.ConnectFailed":
                        {
                            ConnectFailed? d = JsonSerializer.Deserialize<ConnectFailed>(details);
                            if (d != null)
                            {
                                resp.ConnectFailed(d.why);
                            }
                            break;
                        }
                    case "Controller.ConnectSuccess":
                        {
                            ConnectSuccess? d = JsonSerializer.Deserialize<ConnectSuccess>(details);
                            if (d != null)
                            {
                                resp.ConnectSuccess(d.uuid, d.resolution.width, d.resolution.height);
                            }
                            break;
                        }
                    case "Controller.UUIDGot":
                        {
                            UUIDGot? d = JsonSerializer.Deserialize<UUIDGot>(details);
                            if (d != null)
                            {
                                resp.UUIDGot(d.uuid);
                            }
                            break;
                        }
                    case "Controller.UUIDGetFailed":
                        resp.UUIDGetFailed(); break;
                    case "Controller.ResolutionGot":
                        {
                            ResolutionGot? d = JsonSerializer.Deserialize<ResolutionGot>(details);
                            if (d != null)
                            {
                                resp.ResolutionGot(d.resolution.width, d.resolution.height);
                            }
                            break;
                        }
                    case "Controller.ResolutionGetFailed":
                        resp.ResolutionGetFailed(); break;
                    case "Controller.ScreencapInited":
                        resp.ScreencapInited(); break;
                    case "Controller.ScreencapInitFailed":
                        resp.ScreencapInitFailed(); break;
                    case "Controller.TouchinputInited":
                        resp.TouchinputInited(); break;
                    case "Controller.TouchinputInitFailed":
                        resp.TouchinputInitFailed(); break;
                }
            }, resp.GetArg());
            if (ctrl == IntPtr.Zero)
            {
                throw new Exception("MaaAdbControllerCreate Failed");
            }
            return new MaaController(ctrl);
        }

        public bool SetScreenshotTargetWidth(int width)
        {
            return MaaControllerSetScreenshotTargetWidth(handle, width);
        }

        public bool SetScreenshotTargetHeight(int height)
        {
            return MaaControllerSetScreenshotTargetHeight(handle, height);
        }

        public bool SetDefaultAppPackageEntry(string entry)
        {
            return MaaControllerSetDefaultAppPackageEntry(handle, entry);
        }

        public bool SetDefaultAppPackage(string package)
        {
            return MaaControllerSetDefaultAppPackage(handle, package);
        }
    }
}
