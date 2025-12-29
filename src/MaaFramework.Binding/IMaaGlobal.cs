using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaGlobal.
/// </summary>
public interface IMaaGlobal : IMaaOption<GlobalOption>
{
    /// <summary>
    ///     Gets version of MaaFramework.
    /// </summary>
    string Version { get; }

    /// <summary>
    ///     Loads a plugin with full path or name only, name only will search in system directory and current directory.
    ///     <para>or loads plugins with recursive search in the directory.</para>
    /// </summary>
    /// <param name="libraryPath">The name / file path / directory path.</param>
    /// <returns><see langword="true"/> if the plugins are loaded successfully; otherwise, <see langword="false"/>.</returns>
    bool LoadPlugin(string libraryPath);
}
