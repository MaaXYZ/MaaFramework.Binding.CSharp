namespace MaaCommon.Exceptions;

/// <summary>
///     Exception for MAA adb operations
/// </summary>
public class MaaAdbException : Exception
{
    /// <summary>
    ///     Exception for MAA adb operations
    /// </summary>
    /// <param name="message"></param>
    public MaaAdbException(string message) : base(message) { }
}
