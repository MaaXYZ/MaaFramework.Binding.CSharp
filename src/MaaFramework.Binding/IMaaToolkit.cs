﻿namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaToolkit.
/// </summary>
public interface IMaaToolkit
{
    /// <summary>
    ///     Initializes Maa ToolKit.
    /// </summary>
    /// <returns>
    ///     true if the Maa ToolKit was initialized successfully; otherwise, false.
    /// </returns>
    bool Init();

    /// <summary>
    ///     Uninitializes Maa ToolKit.
    /// </summary>
    /// <returns>
    ///     true if the Maa ToolKit was uninitialized successfully; otherwise, false.
    /// </returns>
    bool Uninit();

    /// <summary>
    ///     Finds informations of devices.
    /// </summary>
    /// <param name="adbPath">The adb path that devices connected to.</param>
    /// <returns>
    ///     The arrays of device information.
    /// </returns>
    DeviceInfo[] Find(string adbPath = "");
}
