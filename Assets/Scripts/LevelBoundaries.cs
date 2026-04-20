using System;
using TreeEditor;
using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Arrow"))
        {
            Destroy(other);
        }
        else if (other.CompareTag("Player"))
        {
            //die
        }
        
        return;
    }
}
