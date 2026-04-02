using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private enum Direction
    {
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }
    
    [Serializable]
    private struct MovingPlatformSettings
    {
        public Direction direction;
        public float speed;
        public float distance;
    }

    [SerializeField] private MovingPlatformSettings settings;
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
        Debug.DrawRay(transform.position,GetDirection() * settings.distance,Color.red,float.MaxValue);
        _initialPos = transform.position;
        _targetPos = _initialPos + GetDirection() * settings.distance;
    }

    private enum MovingState
    {
        Static = 1,
        MoveTo = 2,
        MoveBack = 4,
    }

    private MovingState _movingState = MovingState.Static;

    private void FixedUpdate()
    {
        switch (_movingState)
        {
            case MovingState.Static:
                return;
            case MovingState.MoveTo:
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, settings.speed * Time.fixedDeltaTime);
                if (Vector2.Distance(transform.position, _targetPos) <= 0.001)
                {
                    transform.position = _targetPos;
                    _movingState = MovingState.Static;
                }
                break;
            case MovingState.MoveBack:
                transform.position = Vector3.MoveTowards(transform.position, _initialPos, settings.speed * Time.fixedDeltaTime);
                if (Vector2.Distance(transform.position, _initialPos) <= 0.001)
                {
                    transform.position = _initialPos;
                    _movingState = MovingState.Static;
                }
                break;
        }
    }
    
    //TODO count number of interactions

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("ouui");
            other.transform.SetParent(transform);
            _movingState = MovingState.MoveTo;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("nonn");
            _movingState = MovingState.MoveBack;
        }
    }

}
