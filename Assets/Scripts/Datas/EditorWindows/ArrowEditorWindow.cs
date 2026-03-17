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
            GetWindow<ArrowEditorWindow>("Arrow Modifier");
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
                
                GUIContent strengthLabel = new GUIContent("Strength", "Initial arrow strength (speed)");
                dataCible.Strength = EditorGUILayout.FloatField(strengthLabel, dataCible.Strength);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-25")) dataCible.Strength -= 25f;
                if (GUILayout.Button("-5")) dataCible.Strength -= 5f;
                if (GUILayout.Button("+5")) dataCible.Strength += 5f;
                if (GUILayout.Button("+25")) dataCible.Strength += 25f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);

                GUIContent useGravityLabel = new GUIContent("Use Gravity","Activates the use of gravity after designated amount of time");
                dataCible.UseGravity = EditorGUILayout.Toggle(useGravityLabel, dataCible.UseGravity);
                if (dataCible.UseGravity) 
                {
                    EditorGUI.indentLevel++;
                    
                    GUIContent gravityForceLabel = new GUIContent("Gravity Force", "Strength of gravity applied to arrow (1 = normal gravity)");
                    dataCible.GravityForce = EditorGUILayout.FloatField(gravityForceLabel, dataCible.GravityForce);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-0.2")) dataCible.GravityForce -= 0.2f;
                    if (GUILayout.Button("-0.05")) dataCible.GravityForce -= 0.05f;
                    if (GUILayout.Button("+0.05")) dataCible.GravityForce += 0.05f;
                    if (GUILayout.Button("+0.2")) dataCible.GravityForce += 0.2f;
                    EditorGUILayout.EndHorizontal();
                    
                    GUIContent gravityLerpForceLabel = new GUIContent("Gravity Lerp Force", "The bigger the faster to get to gravity force");
                    dataCible.GravityLerpForce = EditorGUILayout.FloatField(gravityLerpForceLabel, dataCible.GravityLerpForce);
                    EditorGUILayout.BeginHorizontal();
                    dataCible.GravityLerpForce = GUILayout.HorizontalSlider(dataCible.GravityLerpForce, 0f, 1f);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(15f);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-0.2")) dataCible.GravityLerpForce -= 0.2f;
                    if (GUILayout.Button("-0.05")) dataCible.GravityLerpForce -= 0.05f;
                    if (GUILayout.Button("+0.05")) dataCible.GravityLerpForce += 0.05f;
                    if (GUILayout.Button("+0.2")) dataCible.GravityLerpForce += 0.2f;
                    EditorGUILayout.EndHorizontal();
                    
                    GUIContent gravityActivationTimeLabel = new GUIContent("Gravity Activation Time", "Time for activation of gravity");
                    dataCible.GravityActivationTime = EditorGUILayout.FloatField(gravityActivationTimeLabel, dataCible.GravityActivationTime);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-0.2")) dataCible.GravityActivationTime -= 0.2f;
                    if (GUILayout.Button("-0.05")) dataCible.GravityActivationTime -= 0.05f;
                    if (GUILayout.Button("+0.05")) dataCible.GravityActivationTime += 0.05f;
                    if (GUILayout.Button("+0.2")) dataCible.GravityActivationTime += 0.2f;
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.Space(25f);
                
                GUIContent useDestroyLabel = new GUIContent("Use Destroy (Not really GD important)","Activates destruction of arrow after designated amount of time");
                dataCible.UseDestroy = EditorGUILayout.Toggle(useDestroyLabel, dataCible.UseDestroy);
                if (dataCible.UseDestroy) 
                {
                    EditorGUI.indentLevel++;
                    
                    GUIContent destroyTimeLabel = new GUIContent("Destroy Time", "Time to destroy");
                    dataCible.DestroyTime = EditorGUILayout.FloatField(destroyTimeLabel, dataCible.DestroyTime);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-5")) dataCible.DestroyTime -= 5f;
                    if (GUILayout.Button("-1")) dataCible.DestroyTime -= 1f;
                    if (GUILayout.Button("+1")) dataCible.DestroyTime += 1f;
                    if (GUILayout.Button("+5")) dataCible.DestroyTime += 5f;
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUI.indentLevel--;
                }
                
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
