using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        #region Serialization

        [Header("Movement")] 
        [Tooltip("Player base speed")] 
        [SerializeField, Range(0f, 30f)]
        private float playerSpeed = 10f;
        public float PlayerSpeed 
        { 
            get => playerSpeed; 
            internal set => playerSpeed = Mathf.Round(Mathf.Clamp(value,0f,30f) * 10f) / 10f; 
        }
        
        [Tooltip("Max reachable speed")] 
        [SerializeField, Min(0f)] 
        private float maxSpeed = 20f;
        public float MaxSpeed
        {
            get => maxSpeed; 
            internal set => maxSpeed = Mathf.Max(value,0f);
        }
        
        [Tooltip("Acceleration speed towards base speed")] 
        [SerializeField, Min(0f)] 
        private float playerAcceleration = 25f;
        public float PlayerAcceleration
        {
            get => playerAcceleration; 
            internal set => playerAcceleration = Mathf.Max(value,0f);
        }
        
        [Space(10f)]
        
        [Header("Jump")]
        [Tooltip("Height reached by jump")]
        [SerializeField, Min(0f)] 
        private float jumpHeight = 6f;
        public float JumpHeight
        {
            get => jumpHeight;
            internal set => jumpHeight = Mathf.Max(value,0f);
        }
        
        [Tooltip("Time in seconds to reach jump apex")]
        [SerializeField, Min(0f)] 
        private float timeToJumpApex = 0.3f;
        public float TimeToJumpApex
        {
            get => timeToJumpApex;
            internal set => timeToJumpApex = Mathf.Max(value, 0f);
        }

        [Tooltip("Time in seconds to fall back to ground")]
        [SerializeField, Min(0f)] 
        private float timeToFall = 0.2f;
        public float TimeToFall
        {
            get => timeToFall;
            internal set => timeToFall = Mathf.Max(value, 0f);
        }

        [Tooltip("Vertical speed at which apex is reached")]
        [SerializeField, Min(0f)] 
        private float apexHangThreshold = 1f;
        public float ApexHangThreshold
        {
            get => apexHangThreshold;
            internal set => apexHangThreshold = Mathf.Max(value, 0f);
        }

        [Tooltip("Gravity multiplier at apex (1 = floaty, 0 = no gravity)")]
        [SerializeField, Range(0f, 1f)] 
        private float apexHangGravityMult = 0.5f;
        public float ApexHangGravityMult
        {
            get => apexHangGravityMult;
            internal set => apexHangGravityMult = Mathf.Round(Mathf.Clamp(value, 0f, 1f) * 1000f) / 1000f;
        }

        [Tooltip("Max falling speed")] 
        [SerializeField, Min(0f)] 
        private float maxFallSpeed = 8f;
        public float MaxFallSpeed
        {
            get => maxFallSpeed; 
            internal set => maxFallSpeed = Mathf.Max(value,0f);
        }
        
        [Tooltip("JumpCut parameter allowing jumps depending on player input")] 
        [SerializeField, Range(0f, 1f)]
        private float jumpCutMultiplier = 0.5f;
        public float JumpCutMultiplier 
        { 
            get => jumpCutMultiplier; 
            internal set => jumpCutMultiplier = Mathf.Round(Mathf.Clamp(value,0f,1f) * 1000f) / 1000f; 
        } 
        
        [Space(10f)]

        [Header("Other")]
        [Tooltip("Time to jump after leaving platform")]
        [SerializeField, Range(0f, 0.5f)] 
        private float coyoteTime = 0.2f;
        public float CoyoteTime
        {
            get => coyoteTime; 
            internal set => coyoteTime = Mathf.Round(Mathf.Clamp(value, 0f, 0.5f) * 1000f) / 1000f;
        }
        
        [Tooltip("Time before landing where you can press jump to jump on landing")]
        [SerializeField, Range(0f, 1f)] 
        private float jumpBufferTime = 0.2f;
        public float JumpBufferTime
        {
            get => jumpBufferTime; 
            internal set => jumpBufferTime = Mathf.Round(Mathf.Clamp(value,0f,1f) * 1000f) / 1000f;
        }
        
        [Tooltip("Amount of air control")]
        [SerializeField, Min(0f)] 
        private float airControl = 13f;
        public float AirControl
        {
            get => airControl; 
            internal set => airControl = Mathf.Max(value,0f);
        }
        
        #endregion
        
    }

}
