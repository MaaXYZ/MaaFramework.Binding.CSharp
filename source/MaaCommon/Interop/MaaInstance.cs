using static MaaCommon.Interop.MaaProxy;

namespace MaaCommon.Interop
{
    public class MaaInstance : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;

        private MaaResource? resource = null;
        private MaaController? controller = null;

        public MaaInstance(IntPtr inst_handle)
        {
            handle = inst_handle;
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
                MaaInstanceDestroy(handle);
                handle = IntPtr.Zero;
                disposed = true;
            }
        }

        ~MaaInstance()
        {
            Dispose(false);
        }

        public IntPtr GetHandle()
        {
            return handle;
        }

        // TODO: 也许封装一些高层接口？比如异步化之类的

        public bool BindController(MaaController ctrl)
        {
            if (controller != null)
            {
                throw new InvalidOperationException("Controller already bounden");
            }
            if (MaaBindController(handle, ctrl.GetHandle()))
            {
                controller = ctrl;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BindResource(MaaResource res)
        {
            if (controller != null)
            {
                throw new InvalidOperationException("Resource already bounden");
            }
            if (MaaBindResource(handle, res.GetHandle()))
            {
                resource = res;
                return true;
            }
            else
            {
                return false;
            }
        }

        public MaaController? GetController()
        {
            if (controller == null)
            {
                return null;
            }
            else
            {
                IntPtr ctrl = MaaGetController(handle);
                if (ctrl != controller.GetHandle())
                {
                    throw new InvalidOperationException("Controller bounden mismatch");
                }
                else
                {
                    return controller;
                }
            }
        }

        public MaaResource? GetResource()
        {
            if (resource == null)
            {
                return null;
            }
            else
            {
                IntPtr ctrl = MaaGetResource(handle);
                if (ctrl != resource.GetHandle())
                {
                    throw new InvalidOperationException("Resource bounden mismatch");
                }
                else
                {
                    return resource;
                }
            }
        }
    }
}
