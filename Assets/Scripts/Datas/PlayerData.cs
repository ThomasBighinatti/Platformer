using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        #region Serialization
        
        [Header("Movement")] 
        [Tooltip("Player base speed")]
        [SerializeField, Range(0f, 20f)] private float playerSpeed = 10f;
        [Tooltip("Max reachable speed")]
        [SerializeField, Min(0f)] private float maxSpeed = 20f;
        [Tooltip("Accel speed towards base speed")]
        [SerializeField, Min(0f)] private float playerAcceleration = 25f;
        [Space(10f)]
        
        [Header("Jump")]
        [Tooltip("Jump force applied (height reached)")]
        [SerializeField, Min(0f)] private float jumpStrength = 8f;
        [Tooltip("Max falling speed (has to be negative)")]
        [SerializeField] private float maxFallSpeed = -8f;
        [Tooltip("JumpCut parameter allowing jumps depending on player input")]
        [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;
        [Space(10f)]

        [Header("Other")]
        [Tooltip("Time to jump after leaving platform")]
        [SerializeField, Min(0f), Range(0f, 0.5f)] private float coyoteTime = 0.2f;
        [Tooltip("Time before landing where you can press jump to jump on landing")]
        [SerializeField, Min(0f), Range(0f, 1f)] private float jumpBufferTime = 0.2f;
        [Tooltip("Amount of air control")]
        [SerializeField, Min(0f)] private float airControl = 13f;
        
        #endregion
        
        #region Public Versions

        public float PlayerSpeed => playerSpeed;
        public float MaxSpeed => maxSpeed;
        public float PlayerAcceleration => playerAcceleration;
        
        public float JumpStrength => jumpStrength;
        public float MaxFallSpeed => maxFallSpeed;
        public float JumpCutMultiplier => jumpCutMultiplier;

        public float CoyoteTime => coyoteTime;
        public float JumpBufferTime => jumpBufferTime;
        public float AirControl => airControl;

        #endregion
        
    }
}
