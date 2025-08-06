// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MaaFramework.Binding;

#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes

/// <summary>
///     InferenceCoreMLFlags are bool options we want to set for CoreML EP.
/// </summary>
[Flags]
public enum InferenceCoreMLFlags
{
    /// <remarks>
    ///     Using in Binding.
    /// </remarks>
    Auto = -1,

    /// <remarks>
    ///     Do not use CoreML Execution Provider.
    /// </remarks>
    None = 0x000,

    /// <remarks>
    ///     Using CPU only in CoreML EP, this may decrease the perf but will provide
    ///     reference output value without precision loss, which is useful for validation.
    /// </remarks>
    UseCpuOnly = 0x001,

    /// <remarks>
    ///     Enable CoreML EP on subgraph.
    /// </remarks>
    EnableOnSubgraph = 0x002,

    /// <remarks>
    ///     By default, CoreML Execution Provider will be enabled for all compatible Apple devices.<para/>
    ///     Enable this option will only enable CoreML EP for Apple devices with ANE (Apple Neural Engine).<para/>
    ///     Please note, enable this option does not guarantee the entire model to be executed using ANE only.
    /// </remarks>
    OnlyEnableDeviceWithAne = 0x004,

    /// <remarks>
    ///     Only allow CoreML EP to take nodes with inputs with static shapes.<para/>
    ///     By default it will also allow inputs with dynamic shapes.<para/>
    ///     However, the performance may be negatively impacted if inputs have dynamic shapes.
    /// </remarks>
    OnlyAllowStaticInputShapes = 0x008,

    /// <remarks>
    ///     Create an MLProgram.<para/>
    ///     By default it will create a NeuralNetwork model. Requires Core ML 5 or later.
    /// </remarks>
    CreateMLProgram = 0x010,

    /// <remarks>
    ///     See: <a href="https://developer.apple.com/documentation/coreml/mlcomputeunits?language=objc">MLComputeUnits</a>.<para/>
    ///     there are four compute units:<para/>
    ///     MLComputeUnitsCPUAndNeuralEngine | MLComputeUnitsCPUAndGPU | MLComputeUnitsCPUOnly | MLComputeUnitsAll<para/>
    ///     different CU will have different performance and power consumption.
    /// </remarks>
    UseCpuAndGpu = 0x020,

    /// <remarks>
    ///     Keep Last at the end of the enum definition.<para/>
    ///     And assign the last CoreMLFlag to it.
    /// </remarks>
    Last = UseCpuAndGpu,
}
