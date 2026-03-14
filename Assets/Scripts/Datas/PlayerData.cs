using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace Datas
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        #region Serialization
        
        // J't'ai retiré les fields: et les get/set, la sécu est contraingnante ici
        
        [Header("Movement")] 
        [Tooltip("Player base speed")]
        [Range(0f, 20f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] 
        public float PlayerSpeed  = 10f;
        
        [Tooltip("Max reachable speed")]
        [Min(0f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float MaxSpeed  = 20f;
        [Tooltip("Accel speed towards base speed")]
        [Min(0f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float PlayerAcceleration  = 25f;
        [Space(10f)]
        
        [Header("Jump")]
        [Tooltip("Jump force applied (height reached)")]
        [ Min(0f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float JumpStrength  = 8f;
        [Tooltip("Max falling speed (has to be negative)")]
        [SerializeField] public float MaxFallSpeed  = -8f;
        [Tooltip("JumpCut parameter allowing jumps depending on player input")]
        [ Range(0f, 1f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float JumpCutMultiplier  = 0.5f;
        [Space(10f)]

        [Header("Other")]
        [Tooltip("Time to jump after leaving platform")]
        [Min(0f), Range(0f, 0.5f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float CoyoteTime  = 0.2f;
        [Tooltip("Time before landing where you can press jump to jump on landing")]
        [Min(0f), Range(0f, 1f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float JumpBufferTime  = 0.2f;
        [Tooltip("Amount of air control")]
        [Min(0f), Obsolete("PAS TOUCHER, PASSE PAR LE WRAPPER")] public float AirControl  = 13f;
        
        #endregion

        public PlayerDataWrapper GetRuntimeData()
        {
            return new PlayerDataWrapper(this);
        }
        
    }

    public class PlayerDataWrapper
    {
        // readonly -> modifiable SEULEMENT quand tu le déclares ou
        // dans le constructor, pour que tes copains aient une erreur si ils essaient de le modif
        // ( pas qu'on fasse pas confiance aux autres devs, mais on a tendance à faire des dingueries quand on dev à 3h)
        public readonly float PlayerSpeed;
        public readonly float MaxSpeed;
        public readonly float PlayerAcceleration;
        
        public readonly float JumpStrength;
        public readonly float MaxFallSpeed;
        public readonly float JumpCutMultiplier;

        public readonly float CoyoteTime;
        public readonly float JumpBufferTime;
        public readonly float AirControl;

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
