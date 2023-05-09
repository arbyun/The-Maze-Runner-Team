using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class LevelCreator : EditorWindow
    {
        private readonly GUIContent _mNameContent = new("New Level name");
        private string _mLevelName;

        /// <summary>
        ///     The OnGUI function is called every time the editor window is updated.
        ///     This function will be used to create a text field for the user to enter their level name, and a button
        ///     that will check if the scene exists and then create it.
        /// </summary>
        /// <returns> A boolean value.</returns>
        private void OnGUI()
        {
            _mLevelName = EditorGUILayout.TextField(_mNameContent, _mLevelName);

            if (GUILayout.Button("Create"))
                CheckAndCreateScene();
        }

        /// <summary>
        ///     Creates the LevelCreator window.
        ///     It also sets the name of the level to Level_X; by default.
        /// </summary>
        /// <returns> A window of type LevelCreator.</returns>
        [MenuItem("TMZ/New Level...", priority = 100)]
        private static void Init()
        {
            var window = GetWindow<LevelCreator>();
            window.Show();
            window._mLevelName = "Level_X";
            LevelData thisLevelData = ScriptableObject.CreateInstance<LevelData>();
            thisLevelData.name = window._mLevelName + "Data";
        } // ReSharper disable Unity.PerformanceAnalysis
        
        /// <summary>
        ///     The CheckAndCreateScene function checks to see if the scene is dirty, and if it is, asks the user
        ///     whether they want to save their changes. If they do not want to save their changes, then nothing happens.
        ///     If they do want to save their changes or there are no unsaved changes in the current scene, then a new
        ///     scene will be created.
        /// </summary>
        /// <returns> A boolean value of true or false.</returns>
        private void CheckAndCreateScene()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("Cannot create scenes while in play mode. Exit play mode first.");
                return;
            }

            if (string.IsNullOrEmpty(_mLevelName))
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

            CreateScene();
        }

        /// <summary>
        ///     The CreateScene function creates a new scene based on the template scene.
        ///     It first searches for the template scene in the project, and if it finds it, copies
        ///     that to a new location with the name specified by _mLevelName. Then it adds this
        ///     newly created level to build settings.
        /// </summary>
        /// <returns> A string array.</returns>
        private void CreateScene()
        {
            var result = AssetDatabase.FindAssets("_TemplateLevel");

            if (result.Length > 0)
            {
                var newScenePath = "Assets/Levels/" + _mLevelName + ".unity";
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


        /// <summary> The AddSceneToBuildSettings function adds a scene to the build settings.</summary>
        /// <param name="scene">
        ///     /// the scene to add to the build settings.
        /// </param>
        /// <returns>
        ///     A boolean value. if the scene is successfully added to the build settings, it returns true;
        ///     otherwise, it returns false.
        /// </returns>
        private static void AddSceneToBuildSettings(Scene scene)
        {
            var buildScenes = EditorBuildSettings.scenes;

            var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
            for (var i = 0; i < buildScenes.Length; i++) newBuildScenes[i] = buildScenes[i];
            newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scene.path, true);
            EditorBuildSettings.scenes = newBuildScenes;
        }


        /// <summary>
        ///     The InstantiatePrefab function instantiates a prefab from the specified folder path and with the
        ///     specified name.  If no prefab is found, an error message is logged to the console.
        /// </summary>
        /// <param name="folderPath"> The path to the folder containing the prefab.</param>
        /// <param name="prefabName"> The name of the prefab to be instantiated.</param>
        /// <returns> A GameObject instance.</returns>
        protected GameObject InstantiatePrefab(string folderPath, string prefabName)
        {
            GameObject instance = null;
            string[] prefabFolderPath = { folderPath };
            var guids = AssetDatabase.FindAssets(prefabName, prefabFolderPath);

            switch (guids.Length)
            {
                case 0:
                    Debug.LogError("The " + prefabName + " prefab could not be found in " + folderPath + " and " +
                                   "could therefore not be instantiated.  Please create one manually.");
                    break;
                case > 1:
                    Debug.LogError("Multiple " + prefabName + " prefabs were found in " + folderPath + " and one " +
                                   "could therefore not be instantiated.  Please create one manually.");
                    break;
                default:
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    instance = Instantiate(prefab);
                    break;
                }
            }

            return instance;
        }
    }
}