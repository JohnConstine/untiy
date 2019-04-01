using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SG1.Editor
{
    public class ScenesViewWindow : EditorWindow
    {
        private List<EditorBuildSettingsScene> m_BuildScenes = new List<EditorBuildSettingsScene>();

        private int m_CurrentSeletSceneIndex = -1;

        [MenuItem(Constant.AssemblyInfo.Namespace + "/Scenes View", false, 11)]
        static void Init()
        {
            EditorWindow.GetWindow<ScenesViewWindow>("Scenes View", true);
        }

        private void OnFocus()
        {
            Refresh();
        }

        private void Refresh()
        {
            m_BuildScenes.Clear();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                m_BuildScenes.Add(scene);
            }
        }

        private void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.button.richText = true;
            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            {
                for (int i = 0; i < m_BuildScenes.Count; i++)
                {
                    DrawGUI(i);
                }
            }
            GUILayout.EndVertical();
            GUI.skin.button.richText = false;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.label.richText = false;
        }

        private void DrawGUI(int index)
        {
            GUILayout.BeginVertical("box");
            {
                var editorBuildSettingsScene = m_BuildScenes[index];

                if (!File.Exists(editorBuildSettingsScene.path))
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Format("<b>{0}</b> Deleted",
                            Path.GetFileNameWithoutExtension(editorBuildSettingsScene.path)));
                        if (GUILayout.Button("Remove"))
                        {
                            var temp = new List<EditorBuildSettingsScene>();
                            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                            {
                                if (i == index)
                                {
                                    continue;
                                }

                                temp.Add(EditorBuildSettings.scenes[i]);
                            }

                            EditorBuildSettings.scenes = temp.ToArray();
                        }
                    }
                    GUILayout.EndHorizontal();
                    Refresh();
                }
                else
                {
                    if (GUILayout.Button(string.Format("<b>{0}</b>",
                        Path.GetFileNameWithoutExtension(editorBuildSettingsScene.path))))
                    {
                        if (m_CurrentSeletSceneIndex != index)
                        {
                            m_CurrentSeletSceneIndex = index;
                        }
                        else
                        {
                            m_CurrentSeletSceneIndex = -1;
                        }
                    }

                    if (m_CurrentSeletSceneIndex == index)
                    {
                        SceneAsset sceneAsset =
                            (SceneAsset) AssetDatabase.LoadAssetAtPath(editorBuildSettingsScene.path,
                                typeof(SceneAsset));

                        var scene = EditorSceneManager.GetSceneByPath(editorBuildSettingsScene.path);

                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(string.Format("<b>{0}</b>", "Selet")))
                            {
                                if (scene.isLoaded)
                                {
                                    EditorSceneManager.SetActiveScene(scene);
                                }

                                EditorGUIUtility.PingObject(sceneAsset);
                                Selection.SetActiveObjectWithContext(sceneAsset, null);
                            }

                            if (!scene.isLoaded)
                            {
                                GUILayout.Space(5);
                                if (GUILayout.Button(string.Format("<b>{0}</b>", "Open")))
                                {
                                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                    {
                                        EditorSceneManager.OpenScene(editorBuildSettingsScene.path,
                                            OpenSceneMode.Single);
                                    }
                                }
                            }
                            else if (scene.isLoaded && EditorSceneManager.loadedSceneCount > 1)
                            {
                                GUILayout.Space(5);
                                if (GUILayout.Button(string.Format("<b>{0}</b>", "Close Ohter")))
                                {
                                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                    {
                                        EditorSceneManager.OpenScene(editorBuildSettingsScene.path,
                                            OpenSceneMode.Single);
                                    }
                                }
                            }

                            if (!scene.isLoaded)
                            {
                                GUILayout.Space(5);
                                if (GUILayout.Button(string.Format("<b>{0}</b>", "Load")))
                                {
                                    EditorSceneManager.OpenScene(editorBuildSettingsScene.path, OpenSceneMode.Additive);
                                }
                            }

                            if (scene.isLoaded && EditorSceneManager.loadedSceneCount > 1)
                            {
                                GUILayout.Space(5);
                                if (GUILayout.Button(string.Format("<b>{0}</b>", "Close")))
                                {
                                    if (scene.isDirty)
                                    {
                                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                        {
                                            EditorSceneManager.CloseScene(
                                                EditorSceneManager.GetSceneByPath(editorBuildSettingsScene.path),
                                                true);
                                        }
                                    }
                                    else
                                    {
                                        EditorSceneManager.CloseScene(
                                            EditorSceneManager.GetSceneByPath(editorBuildSettingsScene.path),
                                            true);
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
    }
}