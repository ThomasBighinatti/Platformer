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
        [field:SerializeField, Range(0f, 20f)] public float PlayerSpeed { get; private set; } = 10f;
        [field:Tooltip("Max reachable speed")]
        [field:SerializeField, Min(0f)] public float MaxSpeed { get; private set; } = 20f;
        [field:Tooltip("Accel speed towards base speed")]
        [field:SerializeField, Min(0f)] public float PlayerAcceleration { get; private set; } = 25f;
        [field:Space(10f)]
        
        [field:Header("Jump")]
        [field:Tooltip("Jump force applied (height reached)")]
        [field:SerializeField, Min(0f)] public float JumpStrength { get; private set; } = 8f;
        [field:Tooltip("Max falling speed (has to be negative)")]
        [field:SerializeField] public float MaxFallSpeed { get; private set; } = -8f;
        [field:Tooltip("JumpCut parameter allowing jumps depending on player input")]
        [field:SerializeField, Range(0f, 1f)] public float JumpCutMultiplier { get; private set; } = 0.5f;
        [field:Space(10f)]

        [field:Header("Other")]
        [field:Tooltip("Time to jump after leaving platform")]
        [field:SerializeField, Min(0f), Range(0f, 0.5f)] public float CoyoteTime { get; private set; } = 0.2f;
        [field:Tooltip("Time before landing where you can press jump to jump on landing")]
        [field:SerializeField, Min(0f), Range(0f, 1f)] public float JumpBufferTime { get; private set; } = 0.2f;
        [field:Tooltip("Amount of air control")]
        [field:SerializeField, Min(0f)] public float AirControl { get; private set; } = 13f;
        
        #endregion

        public PlayerDataWrapper GetRuntimeData()
        {
            return new PlayerDataWrapper(this);
        }
        
    }

    public class PlayerDataWrapper
    {
        public float PlayerSpeed;
        public float MaxSpeed;
        public float PlayerAcceleration;
        
        public float JumpStrength;
        public float MaxFallSpeed;
        public float JumpCutMultiplier;

        public float CoyoteTime;
        public float JumpBufferTime;
        public float AirControl;

        public PlayerDataWrapper(PlayerData data)
        {
            PlayerSpeed = data.PlayerSpeed;
            MaxSpeed = data.MaxSpeed;
            PlayerAcceleration = data.PlayerAcceleration;

            JumpStrength = data.JumpStrength;
            MaxFallSpeed = data.MaxFallSpeed;
            JumpCutMultiplier = data.JumpCutMultiplier;

            CoyoteTime = data.CoyoteTime;
            JumpBufferTime = data.JumpBufferTime;
            AirControl = data.AirControl;
        }
    }
}
