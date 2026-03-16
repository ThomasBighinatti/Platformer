using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "ArrowData", menuName = "Scriptable Objects/ArrowData")]
    public class ArrowData : ScriptableObject
    {
        #region Serialization
        
        [Header("Force")]
        [Tooltip("Initial arrow strength (speed)")] 
        [SerializeField, Min(0f)] 
        private float strength = 200f;
        public float Strength
        {
            get => strength; 
            internal set => strength = Mathf.Max(value,0f);
        }
        
        [Space(10f)]
        
        [Header("Gravity")]
        [Tooltip("Activates the use of gravity after designated amount of time")] 
        [SerializeField] 
        private bool useGravity = true;
        public bool UseGravity
        {
            get => useGravity; 
            internal set => useGravity = value;
        } 
        
        [Tooltip("Strength of gravity applied to arrow (1 = normal gravity")] 
        [SerializeField, Min(0f)] 
        private float gravityForce = 1f;
        public float GravityForce
        {
            get => gravityForce; 
            internal set => gravityForce = Mathf.Max(value,0f);
        }
        
        [Tooltip("The bigger the faster to get to gravity force")] 
        [SerializeField, Range(0f, 1f)] 
        private float gravityLerpForce = 0.2f;
        public float GravityLerpForce
        {
            get => gravityLerpForce; 
            internal set => gravityLerpForce = Mathf.Round(Mathf.Clamp(value,0f,1f) * 1000f) / 1000f;
        }
        
        [Tooltip("Time to activation of gravity")]
        [SerializeField, Min(0f)] 
        private float gravityActivationTime = 0.1f;
        public float GravityActivationTime
        {
            get => gravityActivationTime; 
            internal set => gravityActivationTime = Mathf.Max(value,0f);
        }
        
        [Space(10f)]
        
        [Header("Destroy (Not really GD important)")]
        [Tooltip("Activates destruction of arrow after designated amount of time")]
        [SerializeField] 
        private bool useDestroy = true;
        public bool UseDestroy
        {
            get => useDestroy;
            internal set => useDestroy = value;
        }
        
        [Tooltip("Time to destroy")]
        [SerializeField, Min(0f)]
        private float destroyTime = 10f;
        public float DestroyTime
        {
            get => destroyTime; 
            internal set => destroyTime = Mathf.Max(value,0f);
        }

        #endregion
        
    }
}
