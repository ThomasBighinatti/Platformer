using System.Collections;
using Managers;
using UnityEngine;

namespace Objects
{
    public class HeartScript : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                animator.Play("Noyau Explosion",  0, 0f);
                StartCoroutine(WaitBroken());
            }
            else
            {
                return;
            }
        }

        private IEnumerator WaitBroken()
        {
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.CrystalImpact);
            yield return new WaitForSecondsRealtime(0.6f);
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.CrystalBroken);
        }
    }
}
