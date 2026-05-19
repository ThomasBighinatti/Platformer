using System;
using Arrows;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Momentum momentumArrow = other.gameObject.GetComponent<Momentum>();
            
            momentumArrow.IsOnStickyBlock();
        }
    }
}
