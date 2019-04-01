using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SG1
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InspectorButtonAttribute : Attribute
    {
        public readonly string MethodName;
        public readonly InspectorDiplayMode Mode;

        public InspectorButtonAttribute(InspectorDiplayMode mode = InspectorDiplayMode.AlwaysEnabled,
            string methodName = "")
        {
            Mode = mode;
            MethodName = methodName;
        }

        public InspectorButtonAttribute(string methodName)
        {
            Mode = InspectorDiplayMode.AlwaysEnabled;
            MethodName = methodName;
        }
    }
}