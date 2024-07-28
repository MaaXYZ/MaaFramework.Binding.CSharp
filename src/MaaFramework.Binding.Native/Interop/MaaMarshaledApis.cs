using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

/// <summary>
///     A class providing implementation for managing marshaled parameters in <see cref="Binding"/>.
/// </summary>
/// <typeparam name="T">The marshaled api.</typeparam>
internal sealed class MaaMarshaledApis<T>
{
    private readonly Dictionary<string, T> _apis = [];
    private readonly Dictionary<string, GCHandle> _handles = [];

    public bool Set(string key, T tuple, GCHandle handle)
    {
        _handles[key] = handle;
        _apis[key] = tuple;
        return true;
    }

    public bool Remove(string key)
    {
        _handles[key].Free();
        _handles.Remove(key);
        return _apis.Remove(key);
    }

    public bool Clear()
    {
        foreach (var (_, handle) in _handles)
            handle.Free();
        _handles.Clear();
        _apis.Clear();
        return true;
    }
}
