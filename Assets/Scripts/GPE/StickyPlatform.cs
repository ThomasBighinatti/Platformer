using Arrows;
using UnityEngine;

namespace GPE
{
    public class StickyPlatform : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.parent.SetParent(transform);
            }
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.parent.SetParent(null);
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
