using Managers;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelBoundaries : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Arrow":
                Destroy(other.gameObject);
                break;
            case "Player":
                GameManager.Instance.RespawnPlayer();
                break;
        } 
    }
    
}
