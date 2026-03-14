using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "ArrowData", menuName = "Scriptable Objects/ArrowData")]
    public class ArrowData : ScriptableObject
    {
        
        #region Serialization
        
        [field:Header("Force")]
        [field:Tooltip("Initial arrow strength (speed)")]
        [field:SerializeField, Min(0f)] public float Strength { get; private set; } = 10f;
        [field:Space(10f)]

        [field:Header("Gravity")]
        [field:Tooltip("Activates the use of gravity after designated amount of time")]
        [field:SerializeField] public bool UseGravity { get; private set; } = true;
        [field:Tooltip("Strength of gravity applied to arrow (1 = normal gravity")]
        [field:SerializeField] public float GravityForce { get; private set; } = 0.3f;
        [field:Tooltip("The bigger the faster to get to gravity force")]
        [field:SerializeField] public float GravityLerpForce { get; private set; } = 0.25f;
        [field:Tooltip("Time to activation of gravity")]
        [field:SerializeField] public float GravityActivationTime { get; private set; } = 0.1f;
        [field:Space(10f)]
        
        [field:Header("Destroy (Not really GD important)")]
        [field:Tooltip("Activates destruction of arrow after designated amount of time")]
        [field:SerializeField] public bool UseDestroy { get; private set; } = true;
        [field:Tooltip("Time to destroy")]
        [field:SerializeField, Min(0f)] public float DestroyTime { get; private set; } = 10f;

        #endregion
        
        public ArrowDataWrapper GetRuntimeData()
        {
            return new ArrowDataWrapper(this);
        }
        
    }

    public class ArrowDataWrapper
    {
        public float Strength;

        public bool UseGravity;
        public float GravityForce;
        public float GravityLerpForce;
        public float GravityActivationTime;
        
        public bool UseDestroy;
        public float DestroyTime;

        public ArrowDataWrapper(ArrowData data)
        {
            Strength = data.Strength;

            UseGravity = data.UseGravity;
            GravityForce = data.GravityForce;
            GravityLerpForce = data.GravityLerpForce;
            GravityActivationTime = data.GravityActivationTime;

            UseDestroy = data.UseDestroy;
            DestroyTime = data.DestroyTime;
        }
    }
}
