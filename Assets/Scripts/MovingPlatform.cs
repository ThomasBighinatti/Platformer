using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    
    [Serializable]
    private struct MovingPlatformSettings
    {
        public Direction direction;
        public float speed;
        public float distance;
    }

    [SerializeField] private MovingPlatformSettings settings;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 _initialPos;
    private Vector2 _targetPos;

    private Vector2 GetDirection()
    {
        return settings.direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Right => Vector2.right,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void Start()
    {
        Debug.DrawRay(transform.position,GetDirection(),Color.red,float.MaxValue);
        _initialPos = transform.position;
        _targetPos = _initialPos + GetDirection() * settings.distance;
    }

    private bool _canMove;

    private void FixedUpdate()
    {
        if (!_canMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, settings.speed);
        //rb.MovePosition(settings.speed * Time.fixedDeltaTime * GetDirection());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("ouui");
            _canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("ouui");
            _canMove = true;
        }
    }

}
