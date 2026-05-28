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
                    Debug.Log("Killed");
                    GameManager.Instance.RespawnPlayer();
                    break;
                case "Arrow":
                    Arrow arrow =  other.gameObject.GetComponent<Arrow>();
                    if (arrow != null)
                    {
                        arrow.DestroyArrow();
                        Debug.Log("Destroy Arrow");
                    }
                    else
                    {
                        Debug.Log("No Arrow Component");
                    }
                    break;
            }
        }

        
    }
}
