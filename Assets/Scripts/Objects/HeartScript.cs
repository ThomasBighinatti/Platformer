using System.Collections;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Objects
{
    public class HeartScript : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                animator.Play("Noyau Explosion",  0, 0f);
                Debug.Log("gros caca qui pue");
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.CrystalImpact);
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.CrystalStartBreak);
                StartCoroutine(WaitBroken());
            }
            else
            {
                return;
            }
        }

        private IEnumerator WaitBroken()
        {
            yield return new WaitForSecondsRealtime(1f);
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.CrystalBroken);
        }
    }
}
