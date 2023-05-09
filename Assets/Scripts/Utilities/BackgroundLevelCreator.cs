using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class BackgroundLevelCreator : EditorWindow
    {
        private readonly GUIContent _mNameContent = new("New BG Level name");
        private string _bLevelName;

        /// <summary>
        ///     The OnGUI function is called every time the editor window is updated.
        ///     This function will be used to create a text field for the user to enter their level name, and a button
        ///     that will check if the scene exists and then create it.
        /// </summary>
        /// <returns> A boolean value.</returns>
        private void OnGUI()
        {
            _bLevelName = EditorGUILayout.TextField(_mNameContent, _bLevelName);

            if (GUILayout.Button("Create"))
                checkAndCreateScene();
        }

        /// <summary>
        ///     Creates the LevelCreator window.
        ///     It also sets the name of the level to Level_X; by default.
        /// </summary>
        /// <returns> A window of type LevelCreator.</returns>
        [MenuItem("TMZ/New Background Level...", priority = 90)]
        private static void init()
        {
            var window = GetWindow<BackgroundLevelCreator>();
            window.Show();
            window._bLevelName = "bLevel_X";
            LevelData thisLevelData = ScriptableObject.CreateInstance<LevelData>();
            thisLevelData.name = window._bLevelName + "Data";
        } 
        
        // ReSharper disable Unity.PerformanceAnalysis

        /// <summary>
        ///     The CheckAndCreateScene function checks to see if the scene is dirty, and if it is, asks the user
        ///     whether they want to save their changes. If they do not want to save their changes, then nothing happens.
        ///     If they do want to save their changes or there are no unsaved changes in the current scene, then a new
        ///     scene will be created.
        /// </summary>
        /// <returns> A boolean value of true or false.</returns>
        private void checkAndCreateScene()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("Cannot create scenes while in play mode. Exit play mode first.");
                return;
            }

            if (string.IsNullOrEmpty(_bLevelName))
            {
                Debug.LogWarning("Please enter a scene name before creating a scene.");
                return;
            }

            var currentActiveScene = SceneManager.GetActiveScene();

            if (currentActiveScene.isDirty)
            {
                var hasBeenModified = currentActiveScene.name + " Has Been Modified";
                var message = "Do you want to save the changes you made to " + currentActiveScene.path + "?\nChanges " +
                              "will be lost if you don't save them.";
                var option = EditorUtility.DisplayDialogComplex(hasBeenModified, message, "Save", "Don't " +
                    "Save", "Cancel");

                switch (option)
                {
                    case 0:
                        EditorSceneManager.SaveScene(currentActiveScene);
                        break;
                    case 2:
                        return;
                }
            }

            createScene();
        }

        /// <summary>
        ///     The CreateScene function creates a new scene based on the template scene.
        ///     It first searches for the template scene in the project, and if it finds it, copies
        ///     that to a new location with the name specified by _bLevelName. Then it adds this
        ///     newly created level to build settings.
        /// </summary>
        /// <returns> A string array.</returns>
        private void createScene()
        {
            var result = AssetDatabase.FindAssets("_TemplateBackgroundLevel");

            if (result.Length > 0)
            {
                var newScenePath = "Assets/Levels/" + _bLevelName + ".unity";
                AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(result[0]), newScenePath);
                AssetDatabase.Refresh();
                var newScene = EditorSceneManager.OpenScene(newScenePath, OpenSceneMode.Single);
                addSceneToBuildSettings(newScene);
                Close();
            }
            else
            {
                //Debug.LogError("The template scene <b>_TemplateScene</b> couldn't be found ");
                EditorUtility.DisplayDialog("Error",
                    "The scene _TemplateBackgroundLevel was not found in the Scenes folder. This scene is " +
                    "required by the Level Creator.",
                    "OK");
            }
        }


        /// <summary> The AddSceneToBuildSettings function adds a scene to the build settings.</summary>
        /// <param name="scene">
        ///     /// the scene to add to the build settings.
        /// </param>
        /// <returns>
        ///     A boolean value. if the scene is successfully added to the build settings, it returns true;
        ///     otherwise, it returns false.
        /// </returns>
        private static void addSceneToBuildSettings(Scene scene)
        {
            var buildScenes = EditorBuildSettings.scenes;

            var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
            for (var i = 0; i < buildScenes.Length; i++) newBuildScenes[i] = buildScenes[i];
            newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scene.path, true);
            EditorBuildSettings.scenes = newBuildScenes;
        }
        
    }
}