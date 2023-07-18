using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace MaaCommon.Common;

/// <summary>
///     Represents a native string, in <see cref="IntPtr"/> form
/// </summary>
internal struct NativeString : IDisposable, IEquatable<NativeString>
{
    /// <summary>
    ///     The native string value
    /// </summary>
    public IntPtr Value { get; private set; }
    
    /// <summary>
    ///     The length of the string
    /// </summary>
    public int Length { get; private set; }
    
    /// <summary>
    ///     The original string
    /// </summary>
    public string OriginalString { get; private set; }
    
    /// <summary>
    ///     Create a new Native String
    /// </summary>
    /// <param name="value"></param>
    public NativeString([NotNull] string value)
    {
        OriginalString = value;
        
        Length = Encoding.UTF8.GetByteCount(value);
        var buffer = new byte[Length + 1];
        Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
        Value = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, Value, buffer.Length);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Marshal.FreeHGlobal(Value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is NativeString s)
        {
            return s.Equals(this);
        }
        
        return false;
    }

    /// <inheritdoc />
    public bool Equals(NativeString other)
    {
        return this.GetHashCode() == other.GetHashCode();
    }
    
    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Length, OriginalString);
    }
    
    /// <summary>
    ///     Compare two native strings
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(NativeString left, NativeString right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Compare two native strings
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(NativeString left, NativeString right)
    {
        return !(left == right);
    }
}
