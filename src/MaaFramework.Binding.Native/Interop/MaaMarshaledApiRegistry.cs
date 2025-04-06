using System.Collections.Concurrent;

namespace MaaFramework.Binding.Interop.Native;

/// <summary>
///     A class providing implementation for managing marshaled parameters in <see cref="Binding"/>.
/// </summary>
/// <typeparam name="T">The marshaled api.</typeparam>
internal sealed class MaaMarshaledApiRegistry<T>
{
    private readonly ConcurrentDictionary<string, T> _apis = [];

    public IEnumerable<string> Names => _apis.Keys;

    public bool Register(string key, T tuple)
    {
        _apis[key] = tuple;
        return true;
    }

    public bool Remove(string key)
    {
        _ = _apis.TryRemove(key, out _);
        return true;
    }

    public bool Clear()
    {
        _apis.Clear();
        return true;
    }
}
