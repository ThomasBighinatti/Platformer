using System;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.StickyPlanted);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.StickyReturn);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.OnSticky(onStickyMult, false);
            }
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.OnSticky(onStickyMult, true);
            }
        }
        
    }
}
