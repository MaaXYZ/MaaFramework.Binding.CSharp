using System.Reflection;

namespace MaaFramework.Binding.UnitTests;

#pragma warning disable CA1019 // 定义属性参数的访问器

/// <inheritdoc/>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MaaDataAttribute : DataRowAttribute, ITestDataSource
{
    private readonly string _maaDataSourceName;
    private readonly MaaTypes _maaInteropTypes;
    private readonly TypeInfo _commonTypeInfo = typeof(Common).GetTypeInfo();

    /// <inheritdoc/>
    public override string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
        => string.IsNullOrWhiteSpace(DisplayName)
        ? base.GetDisplayName(methodInfo, data)
        : $"{data?.First(static d => d is MaaTypes)}: {DisplayName}";

    /// <inheritdoc/>
    IEnumerable<object?[]> ITestDataSource.GetData(MethodInfo methodInfo)
    {
        foreach (var maaObject in GetMaaObjects(methodInfo))
        {
            if (!_maaInteropTypes.HasFlag(maaObject.Key))
                continue;

            foreach (var data in GetData(methodInfo))
            {
                var array = new object?[data.Length + 2];
                array[0] = maaObject.Key;
                array[1] = maaObject.Value;
                for (var i = 0; i < data.Length; i++)
                {
                    array[i + 2] = CheckArgument(methodInfo, data[i]);
                }
                yield return array;
            }
        }
    }

    /// <remarks>
    ///     NewData
    /// </remarks>
    private IEnumerable<KeyValuePair<MaaTypes, object>> GetMaaObjects(MethodInfo methodInfo)
    {
        var declaredProperty = (methodInfo.DeclaringType?.GetTypeInfo().GetDeclaredProperty(_maaDataSourceName))
            ?? throw new ArgumentNullException($"{DynamicDataSourceType.Property} {_maaDataSourceName} not exists.");

        var obj = declaredProperty!.GetValue(null, null)
            ?? throw new ArgumentNullException($"{_maaDataSourceName}.GetValue returns null.");

        var enumerable = obj as IEnumerable<KeyValuePair<MaaTypes, object>>
            ?? throw new ArgumentNullException($"{_maaDataSourceName} is not a IEnumerable<KeyValuePair<MaaTypes, object>> type.");

        return enumerable.Any()
            ? enumerable
            : throw new ArgumentException($"{_maaDataSourceName} is empty.");
    }

    private object? CheckArgument(MethodInfo methodInfo, object? arg)
    {
        if (arg is not string str || string.IsNullOrWhiteSpace(str))
            return arg;

        var val = methodInfo.DeclaringType?.GetTypeInfo().GetDeclaredProperty(str)?.GetValue(null, null)
            ?? _commonTypeInfo.GetDeclaredProperty(str)?.GetValue(null, null);

        return val ?? arg;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MaaDataAttribute"/> class.
    /// </summary>
    /// <param name="maaInteropTypes">The MaaInteropTypes.</param>
    /// <param name="maaDataSourceName">The name of maa data source.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
        : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?,object?,object?,object?,object?,object?,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
        : base(arg1, arg2, arg3, arg4, arg5, arg6)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?,object?,object?,object?,object?,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
        : base(arg1, arg2, arg3, arg4, arg5)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?,object?,object?,object?,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1, object? arg2, object? arg3, object? arg4)
        : base(arg1, arg2, arg3, arg4)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?,object?,object?,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1, object? arg2, object? arg3)
        : base(arg1, arg2, arg3)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?,object?,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1, object? arg2)
        : base(arg1, arg2)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName, object? arg1)
        : base(arg1)
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }

    /// <inheritdoc cref="MaaDataAttribute(MaaTypes,string,object?)"/>
    public MaaDataAttribute(MaaTypes maaInteropTypes, string maaDataSourceName)
        : base()
    {
        _maaDataSourceName = maaDataSourceName;
        _maaInteropTypes = maaInteropTypes;
    }
}
