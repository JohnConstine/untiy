using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SG1.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class InspectorButtonEditor : UnityEditor.Editor
    {
        private readonly Dictionary<Type, string> _typeDisplayName = new Dictionary<Type, string>
        {
            {typeof(float), "float"},
            {typeof(int), "int"},
            {typeof(string), "string"},
            {typeof(bool), "bool"},
            {typeof(Color), "Color"},
            {typeof(Vector3), "Vector3"},
            {typeof(Vector2), "Vector2"},
            {typeof(Quaternion), "Quaternion"}
        };

        private readonly Dictionary<Type, ParameterDrawer> _typeDrawer = new Dictionary<Type, ParameterDrawer>
        {
            {typeof(float), DrawFloatParameter},
            {typeof(int), DrawIntParameter},
            {typeof(string), DrawStringParameter},
            {typeof(bool), DrawBoolParameter},
            {typeof(Color), DrawColorParameter},
            {typeof(Vector3), DrawVector3Parameter},
            {typeof(Vector2), DrawVector2Parameter},
            {typeof(Quaternion), DrawQuaternionParameter}
        };

        private EditorButtonState[] _editorButtonStates;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var mono = target as MonoBehaviour;
            if (mono == null) return;


            var methods = mono.GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(o => Attribute.IsDefined(o, typeof(InspectorButtonAttribute)));

            var methodIndex = 0;

            if (_editorButtonStates == null)
                CreateEditorButtonStates(methods.Select(member => (MethodInfo) member).ToArray());

            foreach (var memberInfo in methods)
            {
                var method = memberInfo as MethodInfo;
                DrawButtonforMethod(mono, method, GetEditorButtonState(method, methodIndex));
                methodIndex++;
            }
        }

        private void CreateEditorButtonStates(MethodInfo[] methods)
        {
            _editorButtonStates = new EditorButtonState[methods.Length];
            var methodIndex = 0;
            foreach (var methodInfo in methods)
            {
                _editorButtonStates[methodIndex] = new EditorButtonState(methodInfo.GetParameters().Length);
                methodIndex++;
            }
        }

        private EditorButtonState GetEditorButtonState(MethodInfo method, int methodIndex)
        {
            return _editorButtonStates[methodIndex];
        }

        private void DrawButtonforMethod(MonoBehaviour target, MethodInfo methodInfo, EditorButtonState state)
        {
            EditorGUILayout.BeginHorizontal();
            var ba = (InspectorButtonAttribute) Attribute.GetCustomAttribute(methodInfo,
                typeof(InspectorButtonAttribute));
            GUI.enabled = !EditorApplication.isCompiling &&
                          (ba.Mode == InspectorDiplayMode.AlwaysEnabled || (EditorApplication.isPlaying
                               ? ba.Mode == InspectorDiplayMode.EnabledInPlayMode
                               : ba.Mode == InspectorDiplayMode.DisabledInPlayMode));

            var paramsNum = methodInfo.GetParameters().Length;
            if (paramsNum > 0)
            {
                var foldoutRect = EditorGUILayout.GetControlRect(GUILayout.Width(10.0f));
                state.Opened = EditorGUI.Foldout(foldoutRect, state.Opened, "");
            }
            else
            {
                state.Opened = false;
            }

            var showName = string.IsNullOrEmpty(ba.MethodName) ? MethodDisplayName(methodInfo) : ba.MethodName;
            var clicked = GUILayout.Button(showName, GUILayout.ExpandWidth(true));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            if (state.Opened)
            {
                EditorGUI.indentLevel++;
                var paramIndex = 0;
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    var currentVal = state.Parameters[paramIndex];
                    state.Parameters[paramIndex] = DrawParameterInfo(parameterInfo, currentVal);
                    paramIndex++;
                }

                EditorGUI.indentLevel--;
            }

            if (clicked)
            {
                var returnVal = methodInfo.Invoke(target, state.Parameters);

                if (returnVal is IEnumerator enumerator)
                    target.StartCoroutine(enumerator);
                else if (returnVal != null)
                    Debug.Log("Method call result -> " + returnVal);
            }
        }

        private object GetDefaultValue(ParameterInfo parameter)
        {
            var hasDefaultValue = !DBNull.Value.Equals(parameter.DefaultValue);

            if (hasDefaultValue) return parameter.DefaultValue;

            var parameterType = parameter.ParameterType;
            if (parameterType.IsValueType) return Activator.CreateInstance(parameterType);

            return null;
        }

        private object DrawParameterInfo(ParameterInfo parameterInfo, object currentValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(parameterInfo.Name);

            var drawer = GetParameterDrawer(parameterInfo);
            if (currentValue == null) currentValue = GetDefaultValue(parameterInfo);
            var paramValue = drawer.Invoke(parameterInfo, currentValue);

            EditorGUILayout.EndHorizontal();

            return paramValue;
        }

        private ParameterDrawer GetParameterDrawer(ParameterInfo parameter)
        {
            var parameterType = parameter.ParameterType;

            if (typeof(Object).IsAssignableFrom(parameterType)) return DrawUnityEngineObjectParameter;

            return _typeDrawer.TryGetValue(parameterType, out var drawer) ? drawer : null;
        }

        private static object DrawFloatParameter(ParameterInfo parameterInfo, object val)
        {
            //Since it is legal to define a float param with an integer default value (e.g void method(float p = 5);)
            //we must use Convert.ToSingle to prevent forbidden casts
            //because you can't cast an "int" object to float 
            //See for http://stackoverflow.com/questions/17516882/double-casting-required-to-convert-from-int-as-object-to-float more info

            return EditorGUILayout.FloatField(Convert.ToSingle(val));
        }

        private static object DrawIntParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.IntField((int) val);
        }

        private static object DrawBoolParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Toggle((bool) val);
        }

        private static object DrawStringParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.TextField((string) val);
        }

        private static object DrawColorParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.ColorField((Color) val);
        }

        private static object DrawUnityEngineObjectParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.ObjectField((Object) val, parameterInfo.ParameterType, true);
        }

        private static object DrawVector2Parameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Vector2Field("", (Vector2) val);
        }

        private static object DrawVector3Parameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Vector3Field("", (Vector3) val);
        }

        private static object DrawQuaternionParameter(ParameterInfo parameterInfo, object val)
        {
            return Quaternion.Euler(EditorGUILayout.Vector3Field("", ((Quaternion) val).eulerAngles));
        }

        private string MethodDisplayName(MethodInfo method)
        {
            var sb = new StringBuilder();
            sb.Append(method.Name + "(");
            var methodParams = method.GetParameters();
            foreach (var parameter in methodParams)
            {
                sb.Append(MethodParameterDisplayName(parameter));
                sb.Append(",");
            }

            if (methodParams.Length > 0) sb.Remove(sb.Length - 1, 1);

            sb.Append(")");
            return sb.ToString();
        }

        private string MethodParameterDisplayName(ParameterInfo parameterInfo)
        {
            if (!_typeDisplayName.TryGetValue(parameterInfo.ParameterType, out var parameterTypeDisplayName))
                parameterTypeDisplayName = parameterInfo.ParameterType.ToString();

            return parameterTypeDisplayName + " " + parameterInfo.Name;
        }

        private string MethodUID(MethodInfo method)
        {
            var sb = new StringBuilder();
            sb.Append(method.Name + "_");
            foreach (var parameter in method.GetParameters())
            {
                sb.Append(parameter.ParameterType);
                sb.Append("_");
                sb.Append(parameter.Name);
            }

            sb.Append(")");
            return sb.ToString();
        }

        private class EditorButtonState
        {
            public readonly object[] Parameters;
            public bool Opened;

            public EditorButtonState(int numberOfParameters)
            {
                Parameters = new object[numberOfParameters];
            }
        }

        private delegate object ParameterDrawer(ParameterInfo parameter, object val);
    }
}