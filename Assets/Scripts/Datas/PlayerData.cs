using UnityEngine;
using UnityEngine.Serialization;

namespace Datas
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        #region Serialization
        
        [field:Header("Movement")] 
        [field:Tooltip("Player base speed")]
        [field:SerializeField, Range(0f, 20f)] public float PlayerSpeed { get; internal set; } = 10f;
        [field:Tooltip("Max reachable speed")]
        [field:SerializeField, Min(0f)] public float MaxSpeed { get; internal set; } = 20f;
        [field:Tooltip("Accel speed towards base speed")]
        [field:SerializeField, Min(0f)] public float PlayerAcceleration { get; internal set; } = 25f;
        [field:Space(10f)]
        
        [field:Header("Jump")]
        [field:Tooltip("Jump force applied (height reached)")]
        [field:SerializeField, Min(0f)] public float JumpStrength { get; internal set; } = 8f;
        [field:Tooltip("Max falling speed (has to be negative)")]
        [field:SerializeField] public float MaxFallSpeed { get; internal set; } = -8f;
        [field:Tooltip("JumpCut parameter allowing jumps depending on player input")]
        [field:SerializeField, Range(0f, 1f)] public float JumpCutMultiplier { get; internal set; } = 0.5f;
        [field:Space(10f)]

        [field:Header("Other")]
        [field:Tooltip("Time to jump after leaving platform")]
        [field:SerializeField, Min(0f), Range(0f, 0.5f)] public float CoyoteTime { get; internal set; } = 0.2f;
        [field:Tooltip("Time before landing where you can press jump to jump on landing")]
        [field:SerializeField, Min(0f), Range(0f, 1f)] public float JumpBufferTime { get; internal set; } = 0.2f;
        [field:Tooltip("Amount of air control")]
        [field:SerializeField, Min(0f)] public float AirControl { get; internal set; } = 13f;
        
        #endregion
        
    }

}
