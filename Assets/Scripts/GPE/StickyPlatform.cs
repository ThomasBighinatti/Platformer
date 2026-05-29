using Arrows;
using Controllers;
using UnityEngine;

namespace GPE
{
    
    [RequireComponent(typeof(Collider2D))]
    public class StickyPlatform : MonoBehaviour
    {
        
        [Header("Platform Settings")]
        [SerializeField] private float onStickyMult = 0.5f;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.ChangeStickyMult(onStickyMult, false);
            }
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.ChangeStickyMult(onStickyMult, true);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Arrow")) 
                return;
            
            Momentum momentumArrow = other.gameObject.GetComponent<Momentum>();
            if (momentumArrow == null)
            {
                Debug.Log("StickyPlatform : No Momentum Component");
                return;
            }
            
            momentumArrow.IsOnStickyBlock();
        }
        
    }
}
