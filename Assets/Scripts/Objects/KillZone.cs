using Arrows;
using Managers;
using UnityEngine;

namespace Objects
{
    
    [RequireComponent(typeof(Collider2D))]
    public class KillZone : MonoBehaviour
    {
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            CheckOtherTag(other.gameObject);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            CheckOtherTag(other.gameObject);
        }

        private static void CheckOtherTag(GameObject other)
        {
            switch (other.gameObject.tag)
            {
                case "Player":
                    if (GameManager.Instance != null )
                    {
                        SoundManager.Instance.SoundPlay(SoundManager.MainSfx.Death);
                        GameManager.Instance.RespawnPlayer();
                    }
                    else
                    {
                        Debug.LogWarning("KillZone : No GameManager");
                    }
                    break;
                
                case "Arrow":
                    Arrow arrow =  other.gameObject.GetComponent<Arrow>();
                    if (arrow != null)
                    {
                        arrow.DestroyArrow();
                    }
                    else
                    {
                        Debug.Log("KillZone : No Arrow Component");
                    }
                    break;
            }
        }
        
    }
}
