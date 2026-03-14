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

        // -> potentiellement devenu obsolete avec l'assembly et l'internal
        public PlayerDataWrapper GetRuntimeData()
        {
            return new PlayerDataWrapper(this);
        }
        
    }

    public class PlayerDataWrapper
    {
        // readonly -> modifiable SEULEMENT quand tu le déclares ou
        // dans le constructor, pour que tes copains aient une erreur si ils essaient de le modif
        // ( pas qu'on fasse pas confiance aux autres devs, mais on a tendance à faire des dingueries quand on dev à 3h,
        // alors un petit coup d'pouce pour dire "tu fais quoi chef? vas dormir et recommence demain là ca va pas", c'est jamais de refus)
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
