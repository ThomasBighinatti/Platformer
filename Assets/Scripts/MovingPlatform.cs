using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        StartMoving(Vector2.one, 10f);
    }
    
    private void StartMoving(Vector2 direction, float speed){}
}
