using UnityEditor;
using UnityEngine;

namespace Datas.EditorWindows
{
    public class PlayerEditorWindow : EditorWindow
    {
        
        [SerializeField] private PlayerData dataCible;

        [MenuItem("Tools/PlayerModifier")]
        public static void ShowWindow()
        {
            GetWindow<PlayerEditorWindow>("PlayerModifier");
        }

        private void OnGUI()
        {
            GUILayout.Label("Player modifier", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            dataCible = (PlayerData)EditorGUILayout.ObjectField("Entity to modify", dataCible, typeof(PlayerData), false);

            if (dataCible is not null)
            {
                EditorGUILayout.BeginVertical("box");
            
                dataCible.PlayerSpeed = EditorGUILayout.FloatField("Speed", dataCible.PlayerSpeed);

                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("-10")) dataCible.PlayerSpeed -= 1;
                if (GUILayout.Button("+10")) dataCible.PlayerSpeed += 1;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(dataCible);
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Glisse un ScriptableObject 'Ton nom data' chef", MessageType.Info);
            }
        }
    
        private void OnEnable()
        {
            Prefill();
        }
    
        private void Prefill()
        {
            string[] guids = AssetDatabase.FindAssets("t:EntityData");

            if (guids.Length <= 0) 
                return;
            
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            dataCible = AssetDatabase.LoadAssetAtPath<PlayerData>(path);
        }
    

    }
}
