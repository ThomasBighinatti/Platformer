using System;
using Arrows;
using Managers;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("tué");
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
        }
        else
        {
            Arrow _arrow =  other.gameObject.GetComponent<Arrow>();
            _arrow.DestroyArrow();
            Debug.Log("destroy fleche");
        }

    }
}
