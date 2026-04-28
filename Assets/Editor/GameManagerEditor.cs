using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        
        if (GUILayout.Button("Reload Scene", GUILayout.Height(30)))
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Debug.LogWarning("Il faut être en jeu pour reload la scene BG");
            }
        }
    }
}