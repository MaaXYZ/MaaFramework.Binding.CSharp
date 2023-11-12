namespace MaaFramework.Binding;

/// <summary>
///     A static class adding extensional methods to FlagsEnum.
/// </summary>
public static class FlagsEnumExtension
{
    /// <summary>
    ///      Whether a flags enum contains another flags enum.
    /// </summary>
    /// <param name="flagsEnum">The flags enum.</param>
    /// <param name="containedEnum">The another flags enum.</param>
    /// <returns>true if the flags enum contains the another flags enum; otherwise, false.</returns>
    public static bool Contains(this DisposeOptions flagsEnum, DisposeOptions containedEnum)
        => (flagsEnum & containedEnum) == containedEnum;

    /// <inheritdoc cref="Contains(DisposeOptions, DisposeOptions)"/>
    public static bool Contains(this AdbControllerTypes flagsEnum, AdbControllerTypes containedEnum)
        => (flagsEnum & containedEnum) == containedEnum;

    /// <inheritdoc cref="Contains(DisposeOptions, DisposeOptions)"/>
    public static bool Contains(this DebuggingControllerTypes flagsEnum, DebuggingControllerTypes containedEnum)
        => (flagsEnum & containedEnum) == containedEnum;

    /// <summary>
    ///     Whether a flags enum contains all necessary enumeration value. 
    /// </summary>
    /// <param name="flagsEnum">The flags enum.</param>
    /// <exception cref="MaaException">Throw a MaaException if not all inclusive.</exception>
    public static void Check(this AdbControllerTypes flagsEnum)
    {
        if ((flagsEnum & AdbControllerTypes.TouchMask) == AdbControllerTypes.Invalid)
            throw new ArgumentException($"This types ({flagsEnum}) missing Touch type will cause {nameof(IMaaController.LinkStart)} failure.{Environment.NewLine}Adds Touch type using the bitwise OR operation.");
        if ((flagsEnum & AdbControllerTypes.KeyMask) == AdbControllerTypes.Invalid)
            throw new MaaException($"This types ({flagsEnum}) missing Key type will cause {nameof(IMaaController.LinkStart)} failure.{Environment.NewLine}Adds Key type using the bitwise OR operation.");
        if ((flagsEnum & AdbControllerTypes.ScreenCapMask) == AdbControllerTypes.Invalid)
            throw new MaaException($"This types ({flagsEnum}) missing ScreenCap type will cause {nameof(IMaaController.LinkStart)} failure.{Environment.NewLine}Adds ScreenCap type using the bitwise OR operation.");
    }

    /// <inheritdoc cref="Check(AdbControllerTypes)"/>
    public static void Check(this DebuggingControllerTypes flagsEnum)
    {
        if ((flagsEnum & DebuggingControllerTypes.TouchMask) == DebuggingControllerTypes.Invalid)
            throw new ArgumentException($"This types ({flagsEnum}) missing Touch type will cause {nameof(IMaaController.LinkStart)} failure.{Environment.NewLine}Adds Touch type using the bitwise OR operation.");
        if ((flagsEnum & DebuggingControllerTypes.KeyMask) == DebuggingControllerTypes.Invalid)
            throw new MaaException($"This types ({flagsEnum}) missing Key type will cause {nameof(IMaaController.LinkStart)} failure.{Environment.NewLine}Adds Key type using the bitwise OR operation.");
        if ((flagsEnum & DebuggingControllerTypes.ScreenCapMask) == DebuggingControllerTypes.Invalid)
            throw new MaaException($"This types ({flagsEnum}) missing ScreenCap type will cause {nameof(IMaaController.LinkStart)} failure.{Environment.NewLine}Adds ScreenCap type using the bitwise OR operation.");
    }
}
