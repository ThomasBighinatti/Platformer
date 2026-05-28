using Arrows;
using Controllers;
using Managers;
using UnityEngine;

namespace GPE
{
    public class StickyPlatform : MonoBehaviour
    {
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
            if (other.gameObject.CompareTag("Arrow"))
            {
                Momentum momentumArrow = other.gameObject.GetComponent<Momentum>();
            
                momentumArrow.IsOnStickyBlock();
            }
        }
    }
}
