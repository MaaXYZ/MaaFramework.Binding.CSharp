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

//MaaApiDocument Version: (main) v4.2.3
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
            ///     Message for the task.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Starting = "Tasker.Task.Starting";

            /// <summary>
            ///     Message for the task.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Prefix = "Tasker.Task";

            /// <summary>
            ///     Message for the task.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Succeeded = "Tasker.Task.Succeeded";

            /// <summary>
            ///     Message for the task.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, entry: string, uuid: string, hash: string }</para>
            /// </remarks>
            public const string Failed = "Tasker.Task.Failed";

        }
    }
    public static class Node
    {
        public static class NextList
        {
            /// <summary>
            ///     Message for the next list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], focus: any, }</para>
            /// </remarks>
            public const string Starting = "Node.NextList.Starting";

            /// <summary>
            ///     Message for the next list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], focus: any, }</para>
            /// </remarks>
            public const string Prefix = "Node.NextList";

            /// <summary>
            ///     Message for the next list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], focus: any, }</para>
            /// </remarks>
            public const string Succeeded = "Node.NextList.Succeeded";

            /// <summary>
            ///     Message for the next list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, name: string, list: string[], focus: any, }</para>
            /// </remarks>
            public const string Failed = "Node.NextList.Failed";

        }
        public static class Recognition
        {
            /// <summary>
            ///     Message for the recognition list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Starting = "Node.Recognition.Starting";

            /// <summary>
            ///     Message for the recognition list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Prefix = "Node.Recognition";

            /// <summary>
            ///     Message for the recognition list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Succeeded = "Node.Recognition.Succeeded";

            /// <summary>
            ///     Message for the recognition list of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, reco_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Failed = "Node.Recognition.Failed";

        }
        public static class Action
        {
            /// <summary>
            ///     Message for the action of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Starting = "Node.Action.Starting";

            /// <summary>
            ///     Message for the action of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Prefix = "Node.Action";

            /// <summary>
            ///     Message for the action of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Succeeded = "Node.Action.Succeeded";

            /// <summary>
            ///     Message for the action of node.
            /// </summary>
            /// <remarks>
            ///     <para>payload: { task_id: number, node_id: number, name: string, focus: any, }</para>
            /// </remarks>
            public const string Failed = "Node.Action.Failed";

        }
    }
}