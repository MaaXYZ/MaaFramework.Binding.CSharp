using static MaaCommon.Interop.MaaProxy;

namespace MaaCommon.Interop
{
    public class MaaResource : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;

        public MaaResource(IntPtr res_handle)
        {
            handle = res_handle;
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
                MaaResourceDestroy(handle);
                handle = IntPtr.Zero;
                disposed = true;
            }
        }

        ~MaaResource()
        {
            Dispose(false);
        }

        public IntPtr GetHandle()
        {
            return handle;
        }

        // TODO: 也许封装一些高层接口？比如异步化之类的
        // Resource好像主要是能不能共享？
    }
}
