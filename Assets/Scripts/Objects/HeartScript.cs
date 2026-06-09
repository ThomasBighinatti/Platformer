using System;
using UnityEngine;
using UnityEngine.Events;

public class HeartScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("gros caca qui pue");
        if (other.gameObject.CompareTag("Arrow"))
        {
            animator.Play("Noyau Explosion",  0, 0f);
        }
        else
        {
            return;
        }
    }
}
