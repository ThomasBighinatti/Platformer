using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "ArrowData", menuName = "Scriptable Objects/ArrowData")]
    public class ArrowData : ScriptableObject
    {
        
        #region Serialization
        
        [Header("Force")]
        [Tooltip("Initial arrow strength (speed)")]
        [SerializeField, Min(0f)] private float strength = 10f;
        [Space(10f)]
        
        [Header("Gravity")]
        [Tooltip("Activates the use of gravity after designated amount of time")]
        [SerializeField] private bool useGravity = true;
        [Tooltip("Strength of gravity applied to arrow (1 = normal gravity")]
        [SerializeField] private float gravityForce = 0.3f;
        [Tooltip("The bigger the faster to get to gravity force")]
        [SerializeField] private float gravityLerpForce = 0.25f;
        [Tooltip("Time to activation of gravity")]
        [SerializeField] private float gravityActivationTime = 0.1f;
        [Space(10f)]
        
        [Header("Destroy (Not really gd important)")]
        [Tooltip("Activates destruction of arrow after designated amount of time")]
        [SerializeField] private bool useDestroy = true;
        [Tooltip("Time to destroy")]
        [SerializeField, Min(0f)] private float destroyTime = 10f;

        #endregion
        
        #region Public Versions
        
        public float Strength => strength;

        public bool UseGravity => useGravity;
        public float GravityForce => gravityForce;
        public float GravityLerpForce => gravityLerpForce;
        public float GravityActivationTime => gravityActivationTime;
        
        public bool UseDestroy => useDestroy;
        public float DestroyTime => destroyTime;
        
        #endregion
        
    }
}
