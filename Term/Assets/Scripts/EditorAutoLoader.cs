#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class EditorAutoLoader
{
    static EditorAutoLoader()
    {
        EditorApplication.playModeStateChanged += LoadLobbyScene;
    }

    static void LoadLobbyScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            if (SceneManager.GetActiveScene().name != "Start")
            {
                SceneManager.LoadScene("Start");
            }
        }
    }
}
#endif
