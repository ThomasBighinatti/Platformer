using Controllers;
using Managers;
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
            Debug.Log("collision");
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.OnSticky(onStickyMult, false);
            }
            if (other.gameObject.CompareTag("Arrow"))
            {
                Debug.Log("sound");
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.StickyPlanted);
            }
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            Debug.Log("endcollision");
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.OnSticky(onStickyMult, true);
            }
            if (other.gameObject.CompareTag("Arrow"))
            {
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.StickyReturn);
            }
        }
        
    }
}
