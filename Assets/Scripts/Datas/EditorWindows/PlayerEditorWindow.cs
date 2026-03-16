using UnityEditor;
using UnityEngine;

namespace Datas.EditorWindows
{
    public class PlayerEditorWindow : EditorWindow
    {
        
        [SerializeField] private PlayerData dataCible;
        private Vector2 _scrollPosition;

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
                EditorGUIUtility.labelWidth = 150;
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                
                EditorGUILayout.BeginVertical("box");
            
                GUIContent playerSpeedLabel = new GUIContent("Player Speed", "Player base speed");
                dataCible.PlayerSpeed = EditorGUILayout.FloatField(playerSpeedLabel, dataCible.PlayerSpeed);
                EditorGUILayout.BeginHorizontal();
                dataCible.PlayerSpeed = GUILayout.HorizontalSlider(dataCible.PlayerSpeed, 0f, 30f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10f);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-2")) dataCible.PlayerSpeed -= 2f;
                if (GUILayout.Button("-0.5")) dataCible.PlayerSpeed -= 0.5f;
                if (GUILayout.Button("+0.5")) dataCible.PlayerSpeed += 0.5f;
                if (GUILayout.Button("+2")) dataCible.PlayerSpeed += 2f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent maxSpeedLabel = new GUIContent("Max Speed", "Max reachable speed");
                dataCible.MaxSpeed = EditorGUILayout.FloatField(maxSpeedLabel, dataCible.MaxSpeed);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-5")) dataCible.MaxSpeed -= 5f;
                if (GUILayout.Button("-1")) dataCible.MaxSpeed -= 1f;
                if (GUILayout.Button("+1")) dataCible.MaxSpeed += 1f;
                if (GUILayout.Button("+5")) dataCible.MaxSpeed += 5f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent playerAccelerationLabel = new GUIContent("Player Acceleration", "Acceleration speed towards base speed");
                dataCible.PlayerAcceleration = EditorGUILayout.FloatField(playerAccelerationLabel, dataCible.PlayerAcceleration);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-5")) dataCible.PlayerAcceleration -= 5f;
                if (GUILayout.Button("-1")) dataCible.PlayerAcceleration -= 1f;
                if (GUILayout.Button("+1")) dataCible.PlayerAcceleration += 1f;
                if (GUILayout.Button("+5")) dataCible.PlayerAcceleration += 5f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent jumpStrengthLabel = new GUIContent("Jump Strength", "Jump force applied (height reached)");
                dataCible.JumpStrength = EditorGUILayout.FloatField(jumpStrengthLabel, dataCible.JumpStrength);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-2")) dataCible.JumpStrength -= 2f;
                if (GUILayout.Button("-0.5")) dataCible.JumpStrength -= 0.5f;
                if (GUILayout.Button("+0.5")) dataCible.JumpStrength += 0.5f;
                if (GUILayout.Button("+2")) dataCible.JumpStrength += 2f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent maxFallSpeedLabel = new GUIContent("Max Fall Speed", "Max falling speed");
                dataCible.MaxFallSpeed = EditorGUILayout.FloatField(maxFallSpeedLabel, dataCible.MaxFallSpeed);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-2")) dataCible.MaxFallSpeed -= 2f;
                if (GUILayout.Button("-0.5")) dataCible.MaxFallSpeed -= 0.5f;
                if (GUILayout.Button("+0.5")) dataCible.MaxFallSpeed += 0.5f;
                if (GUILayout.Button("+2")) dataCible.MaxFallSpeed += 2f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent jumpCutMultiplierLabel = new GUIContent("Jump Cut Multiplier", "JumpCut parameter allowing jumps depending on player input");
                dataCible.JumpCutMultiplier = EditorGUILayout.FloatField(jumpCutMultiplierLabel, dataCible.JumpCutMultiplier);
                EditorGUILayout.BeginHorizontal();
                dataCible.JumpCutMultiplier = GUILayout.HorizontalSlider(dataCible.JumpCutMultiplier, 0f, 1f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10f);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-0.2")) dataCible.JumpCutMultiplier -= 0.2f;
                if (GUILayout.Button("-0.05")) dataCible.JumpCutMultiplier -= 0.05f;
                if (GUILayout.Button("+0.05")) dataCible.JumpCutMultiplier += 0.05f;
                if (GUILayout.Button("+0.2")) dataCible.JumpCutMultiplier += 0.2f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent coyoteTimeLabel = new GUIContent("Coyote Time", "Time to jump after leaving platform");
                dataCible.CoyoteTime = EditorGUILayout.FloatField(coyoteTimeLabel, dataCible.CoyoteTime);
                EditorGUILayout.BeginHorizontal();
                dataCible.CoyoteTime = GUILayout.HorizontalSlider(dataCible.CoyoteTime, 0f, 0.5f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10f);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-0.1")) dataCible.CoyoteTime -= 0.1f;
                if (GUILayout.Button("-0.02")) dataCible.CoyoteTime -= 0.02f;
                if (GUILayout.Button("+0.02")) dataCible.CoyoteTime += 0.02f;
                if (GUILayout.Button("+0.1")) dataCible.CoyoteTime += 0.1f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent jumpBufferTimeLabel = new GUIContent("Jump Buffer Time", "Time before landing where you can press jump to jump on landing");
                dataCible.JumpBufferTime = EditorGUILayout.FloatField(jumpBufferTimeLabel, dataCible.JumpBufferTime);
                EditorGUILayout.BeginHorizontal();
                dataCible.JumpBufferTime = GUILayout.HorizontalSlider(dataCible.JumpBufferTime, 0f, 1f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10f);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-0.2")) dataCible.JumpBufferTime -= 0.2f;
                if (GUILayout.Button("-0.05")) dataCible.JumpBufferTime -= 0.05f;
                if (GUILayout.Button("+0.05")) dataCible.JumpBufferTime += 0.05f;
                if (GUILayout.Button("+0.2")) dataCible.JumpBufferTime += 0.2f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(25f);
                
                GUIContent airControlLabel = new GUIContent("Air Control", "Amount of air control");
                dataCible.AirControl = EditorGUILayout.FloatField(airControlLabel, dataCible.AirControl);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-2")) dataCible.AirControl -= 2f;
                if (GUILayout.Button("-0.5")) dataCible.AirControl -= 0.5f;
                if (GUILayout.Button("+0.5")) dataCible.AirControl += 0.5f;
                if (GUILayout.Button("+2")) dataCible.AirControl += 2f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndScrollView();

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
