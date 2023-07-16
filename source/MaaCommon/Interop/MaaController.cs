using static MaaCommon.Interop.MaaProxy;

namespace MaaCommon.Interop
{
    public class MaaController : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;

        public MaaController(IntPtr ctrl_handle)
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
    }
}
