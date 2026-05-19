using System;
using UnityEngine;

namespace GPE
{
    public class MovingPlatform : MonoBehaviour, IGpeInterface
    {
        [Header("Platform Settings")]
        [SerializeField] private MovingPlatformSettings settings;
        [Serializable]
        private struct MovingPlatformSettings
        {
            public Direction direction;
            public float speed;
            public float distance;
        }
    
        private Vector2 _initialPos;
        private Vector2 _targetPos;

        #region Direction GD
    
        private enum Direction
        {
            Up = 1,
            Right = 2,
            Down = 4,
            Left = 8
        }
    
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
    
        #endregion

        private void Start()
        {
            Debug.DrawRay(transform.position,GetDirection() * settings.distance,Color.red,float.MaxValue);
            _initialPos = transform.position;
            _targetPos = _initialPos + GetDirection() * settings.distance;
        }
    
        private void FixedUpdate()
        {
            MovingStateLimits();
        }
    
        private enum MovingState
        {
            Static = 1,
            MoveTo = 2,
            MoveBack = 4,
        }
        private MovingState _movingState = MovingState.Static;

        private void MovingStateLimits()
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int _numberOfInteractions;
        private int NumberOfInteractions
        {
            get => _numberOfInteractions;
            set
            {
                if (value < 0) value = 0;
                _numberOfInteractions = value;
                _movingState = _numberOfInteractions > 0 ? MovingState.MoveTo : MovingState.MoveBack;
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                other.transform.SetParent(transform);
                NumberOfInteractions++;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                NumberOfInteractions--;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.SetParent(null);
            }
        }
        
        public void ResetToInitialState()
        {
            transform.position = _initialPos;
            _movingState = MovingState.Static;
            NumberOfInteractions = 0;
        }

    }
}
