﻿using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class LevelCreator : EditorWindow
    {
        private string _mLevelName;

        private readonly GUIContent _mNameContent = new GUIContent ("New Level Name");
    
        [MenuItem("Maze Runner Team/New Level...", priority = 100)]
        private static void Init ()
        {
            LevelCreator window = GetWindow<LevelCreator> ();
            window.Show();
            window._mLevelName = "Level_X";
        }

        private void OnGUI ()
        {
            _mLevelName = EditorGUILayout.TextField (_mNameContent, _mLevelName);
        
            if(GUILayout.Button ("Create"))
                CheckAndCreateScene ();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void CheckAndCreateScene()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning ("Cannot create scenes while in play mode. Exit play mode first.");
                return;
            }

            if (string.IsNullOrEmpty (_mLevelName))
            {
                Debug.LogWarning ("Please enter a scene name before creating a scene.");
                return;
            }

            var currentActiveScene = SceneManager.GetActiveScene ();

            if (currentActiveScene.isDirty)
            {
                string hasBeenModified = currentActiveScene.name + " Has Been Modified";
                string message = "Do you want to save the changes you made to " + currentActiveScene.path + "?\nChanges " +
                                 "will be lost if you don't save them.";
                int option = EditorUtility.DisplayDialogComplex (hasBeenModified, message, "Save", "Don't " +
                    "Save", "Cancel");

                switch (option)
                {
                    case 0:
                        EditorSceneManager.SaveScene (currentActiveScene);
                        break;
                    case 2:
                        return;
                }
            }
        
            CreateScene ();
        }

        private void CreateScene ()
        {
            string[] result = AssetDatabase.FindAssets("_TemplateLevel");

            if (result.Length > 0)
            {
                string newScenePath = "Assets/Levels/" + _mLevelName + ".unity";
                AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(result[0]), newScenePath);
                AssetDatabase.Refresh();
                var newScene = EditorSceneManager.OpenScene(newScenePath, OpenSceneMode.Single);
                AddSceneToBuildSettings(newScene);
                Close();
            }
            else
            {
                //Debug.LogError("The template scene <b>_TemplateScene</b> couldn't be found ");
                EditorUtility.DisplayDialog("Error",
                    "The scene _TemplateLevel was not found in the Scenes folder. This scene is required by the " +
                    "Level Creator.",
                    "OK");
            }
        }

        private static void AddSceneToBuildSettings (Scene scene)
        {
            EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        
            EditorBuildSettingsScene[] newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
            for (int i = 0; i < buildScenes.Length; i++)
            {
                newBuildScenes[i] = buildScenes[i];
            }
            newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scene.path, true);
            EditorBuildSettings.scenes = newBuildScenes;
        }

        protected GameObject InstantiatePrefab (string folderPath, string prefabName)
        {
            GameObject instance = null;
            string[] prefabFolderPath = { folderPath };
            string[] guids = AssetDatabase.FindAssets (prefabName, prefabFolderPath);
        
            switch (guids.Length)
            {
                case 0:
                    Debug.LogError ("The " + prefabName + " prefab could not be found in " + folderPath + " and " +
                                    "could therefore not be instantiated.  Please create one manually.");
                    break;
                case > 1:
                    Debug.LogError ("Multiple " + prefabName + " prefabs were found in " + folderPath + " and one " +
                                    "could therefore not be instantiated.  Please create one manually.");
                    break;
                default:
                {
                    string path = AssetDatabase.GUIDToAssetPath (guids[0]);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject> (path);
                    instance = Instantiate (prefab);
                    break;
                }
            }

            return instance;
        }
    }
}