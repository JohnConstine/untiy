using System;
using GameFramework;
using GameFramework.Debugger;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public sealed class XLuaWindow : IDebuggerWindow
    {
        private const string DefaultLuaString =
            @"local Log = CS.UnityGameFramework.Runtime.Log
Log.Info('Hello Would!')";

        private XLuaComponent m_XLuaComponent = null;

        private SettingComponent m_SettingComponent = null;

        private Vector2 m_ScrollPosition = Vector2.zero;

        private string m_LuaString = String.Empty;

        private bool m_LockScroll = false;

        private GUIStyle m_guiStyle;

        public void Initialize(params object[] args)
        {
            m_XLuaComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<XLuaComponent>();
            if (m_XLuaComponent == null)
            {
                Log.Fatal("XLua component is invalid.");
                return;
            }

            m_SettingComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<SettingComponent>();
            if (m_SettingComponent == null)
            {
                Log.Fatal("Setting component is invalid.");
                return;
            }

            m_LuaString = m_SettingComponent.GetString("Debugger.XLua.LuaString", DefaultLuaString);
        }

        public void Shutdown()
        {
        }

        public void OnEnter()
        {
        }

        public void OnLeave()
        {
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal("label");
            {
                GUILayout.Label("<b>XLua Window</b>");
                m_LockScroll = GUILayout.Toggle(m_LockScroll, "<b>Lock Scroll</b>");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box");
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, GUILayout.Height(350f));
                {
                    if (m_LockScroll)
                    {
                        m_ScrollPosition.y = float.MaxValue;
                    }

                    if (m_guiStyle == null)
                    {
                        m_guiStyle = new GUIStyle(GUI.skin.label);
                        m_guiStyle.fontSize = 18;
                    }

                    m_LuaString =
                        GUILayout.TextArea(m_LuaString, m_guiStyle, GUILayout.ExpandHeight(true),
                            GUILayout.ExpandWidth(true));
                }
                GUILayout.EndScrollView();

                GUILayout.BeginVertical();
                {
                    int fontSize = m_guiStyle.fontSize;
                    GUILayout.Label(Utility.Text.Format("Font Size:{0}", fontSize.ToString()));
                    m_guiStyle.fontSize = (int) GUILayout.HorizontalSlider(fontSize, 12, 40);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Run", GUILayout.Height(30f)))
                {
                    Run();
                }

                if (GUILayout.Button("Save", GUILayout.Height(30f)))
                {
                    Save();
                }

                if (GUILayout.Button("Clear", GUILayout.Height(30f)))
                {
                    Clear();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void Run()
        {
            m_XLuaComponent.DoString(m_LuaString, "XLuaWindow", m_XLuaComponent.ScriptTable);
        }

        public void Save()
        {
            m_SettingComponent.SetString("Debugger.XLua.LuaString", m_LuaString);
        }

        public void Clear()
        {
            m_LuaString = string.Empty;
        }
    }
}