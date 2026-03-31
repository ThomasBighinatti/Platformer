using UnityEngine;
namespace Datas
{
    [CreateAssetMenu(fileName = "MomentumArrowData", menuName = "Scriptable Objects/MomentumArrowData")]
    public class MomentumArrowData : ArrowData
    {
        [Header("Recall")]
        [Tooltip("Initial recall arrow speed")] 
        [SerializeField, Min(0f)] 
        private float recallInitialSpeed = 25f;
        public float RecallInitialSpeed
        {
            get => recallInitialSpeed; 
            internal set => recallInitialSpeed = Mathf.Max(value,0f);
        }

        [Tooltip("Recall speed acceleration amount")] 
        [SerializeField, Min(0f)] 
        private float recallAcceleration = 100f;
        public float RecallAcceleration
        {
            get => recallAcceleration; 
            internal set => recallAcceleration = Mathf.Max(value,0f);
        }
   
        [Tooltip("Force multiplicator for knockback")] 
        [SerializeField, Range(0f, 3f)] 
        private float knockbackForce = 0.25f;
        public float KnockbackForce
        {
            get => knockbackForce; 
            internal set => knockbackForce = Mathf.Round(Mathf.Clamp(value,0f,3f) * 100f) / 100f;
        }
    }
}