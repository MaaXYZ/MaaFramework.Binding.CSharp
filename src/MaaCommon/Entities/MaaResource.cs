namespace MaaCommon.Entities;

/// <summary>
///     Represents a Maa Resource instance
/// </summary>
public class MaaResource
{
    /// <summary>
    ///     The resource handle
    /// </summary>
    public IntPtr Handle { get; private set; }
    
    /// <summary>
    ///     Create a new Maa Resource instance
    /// </summary>
    /// <param name="handle">The resource handle returned from MaaFramework</param>
    public MaaResource(IntPtr handle)
    {
        Handle = handle;
    }
}
