﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable CS1573 // 参数在 XML 注释中没有匹配的 param 标记
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

﻿namespace MaaFramework.Binding.Notification;

//MaaApiDocument Version: (main) v2.3.1
/// <summary>
///  A callback consists of a message and a payload.
///  The message is a string that indicates the type of the message.
///  The payload is a JSON object that contains the details of the message.
/// </summary>
public static class MaaMsg
{
    public static class Resource
    {
        public static class Loading
        {
            /// <summary>
            ///     The message for the resource loading.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { res_id: number, hash: string, path: string, }</para>
            /// </remarks>
            public const string Starting = "Resource.Loading.Starting";

            /// <summary>
            ///     The message for the resource loading.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { res_id: number, hash: string, path: string, }</para>
            /// </remarks>
            public const string Prefix = "Resource.Loading";

            /// <summary>
            ///     The message for the resource loading.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { res_id: number, hash: string, path: string, }</para>
            /// </remarks>
            public const string Succeeded = "Resource.Loading.Succeeded";

            /// <summary>
            ///     The message for the resource loading.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { res_id: number, hash: string, path: string, }</para>
            /// </remarks>
            public const string Failed = "Resource.Loading.Failed";

        }
    }
    public static class Controller
    {
        public static class Action
        {
            /// <summary>
            ///     Message for the controller actions.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { ctrl_id: number, uuid: string, action: string, }</para>
            /// </remarks>
            public const string Starting = "Controller.Action.Starting";

            /// <summary>
            ///     Message for the controller actions.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { ctrl_id: number, uuid: string, action: string, }</para>
            /// </remarks>
            public const string Prefix = "Controller.Action";

            /// <summary>
            ///     Message for the controller actions.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { ctrl_id: number, uuid: string, action: string, }</para>
            /// </remarks>
            public const string Succeeded = "Controller.Action.Succeeded";

            /// <summary>
            ///     Message for the controller actions.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { ctrl_id: number, uuid: string, action: string, }</para>
            /// </remarks>
            public const string Failed = "Controller.Action.Failed";

        }
    }
    public static class Tasker
    {
        public static class Task
        {
            /// <summary>
            ///     Message for the tasks.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Starting = "Tasker.Task.Starting";

            /// <summary>
            ///     Message for the tasks.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Prefix = "Tasker.Task";

            /// <summary>
            ///     Message for the tasks.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Succeeded = "Tasker.Task.Succeeded";

            /// <summary>
            ///     Message for the tasks.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Failed = "Tasker.Task.Failed";

        }
    }
    public static class Task
    {
        public static class NextList
        {
            /// <summary>
            ///     Message for the Recognition List.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], }</para>
            /// </remarks>
            public const string Starting = "Task.NextList.Starting";

            /// <summary>
            ///     Message for the Recognition List.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], }</para>
            /// </remarks>
            public const string Prefix = "Task.NextList";

            /// <summary>
            ///     Message for the Recognition List.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], }</para>
            /// </remarks>
            public const string Succeeded = "Task.NextList.Succeeded";

            /// <summary>
            ///     Message for the Recognition List.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], }</para>
            /// </remarks>
            public const string Failed = "Task.NextList.Failed";

        }
        public static class Recognition
        {
            /// <summary>
            ///     Message for the recognition list.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, }</para>
            /// </remarks>
            public const string Starting = "Task.Recognition.Starting";

            /// <summary>
            ///     Message for the recognition list.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, }</para>
            /// </remarks>
            public const string Prefix = "Task.Recognition";

            /// <summary>
            ///     Message for the recognition list.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, }</para>
            /// </remarks>
            public const string Succeeded = "Task.Recognition.Succeeded";

            /// <summary>
            ///     Message for the recognition list.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, }</para>
            /// </remarks>
            public const string Failed = "Task.Recognition.Failed";

        }
        public static class Action
        {
            /// <summary>
            ///     Message for the task action.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, }</para>
            /// </remarks>
            public const string Starting = "Task.Action.Starting";

            /// <summary>
            ///     Message for the task action.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, }</para>
            /// </remarks>
            public const string Prefix = "Task.Action";

            /// <summary>
            ///     Message for the task action.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, }</para>
            /// </remarks>
            public const string Succeeded = "Task.Action.Succeeded";

            /// <summary>
            ///     Message for the task action.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, }</para>
            /// </remarks>
            public const string Failed = "Task.Action.Failed";

        }
    }
}