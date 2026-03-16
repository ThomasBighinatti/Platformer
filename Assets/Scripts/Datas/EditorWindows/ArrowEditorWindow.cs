using UnityEditor;
using UnityEngine;

namespace Datas.EditorWindows
{
    public class ArrowEditorWindow : EditorWindow
    {
        [SerializeField] private ArrowData dataCible;
        private Vector2 _scrollPosition;

        [MenuItem("Tools/Arrow Modifier")]
        public static void ShowWindow()
        {
            GetWindow<PlayerEditorWindow>("Arrow Modifier");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Player modifier", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            dataCible = (ArrowData)EditorGUILayout.ObjectField("Entity to modify", dataCible, typeof(ArrowData), false);

            if (dataCible is not null)
            {
                EditorGUIUtility.labelWidth = 150;
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                
                EditorGUILayout.BeginVertical("box");
                
                
                
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndScrollView();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(dataCible);
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Glisse un ScriptableObject 'Ton nom arrow data' chef", MessageType.Info);
            }
        }
    
        private void OnEnable()
        {
            Prefill();
        }
    
        private void Prefill()
        {
            string[] guids = AssetDatabase.FindAssets("t:ArrowData");

            if (guids.Length <= 0) 
                return;
            
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            dataCible = AssetDatabase.LoadAssetAtPath<ArrowData>(path);
        }
    }
}
