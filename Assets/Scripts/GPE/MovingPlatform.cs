using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPE
{
    
    [RequireComponent(typeof(Collider2D))]
    public class MovingPlatform : MonoBehaviour, IResettable
    {
        
        [Header("Platform Settings")]
        [SerializeField] private MovingPlatformSettings settings;

        [Space(10f)] [Header("Visual Settings")] 
        [SerializeField] private float lengthScale;
        [SerializeField] private float heightScale;
        // two by two
        [SerializeField] private List<Sprite> armVertical;
        [SerializeField] private List<Sprite> armHorizontal;
        [SerializeField] private List<Sprite> hands;
        [SerializeField] private List<Sprite> ends;
        
        [Serializable] 
        private struct MovingPlatformSettings
        {
            public Direction direction;
            public float speed;
            public float distance;
        }
    
        private Vector2 _initialPos;
        private Vector2 _targetPos;

        private GameObject _parent;

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
            _initialPos = transform.position;
            _targetPos = _initialPos + GetDirection() * settings.distance;
            
            _parent = new GameObject(gameObject.name + "Parent");
            gameObject.transform.SetParent(_parent.transform);
            Debug.DrawRay(transform.position,GetDirection() * settings.distance,Color.red,float.MaxValue);
            
            GameManager.Instance.Subscribe(this);

            switch (settings.direction)
            {
                case Direction.Up:
                    SpawnVisuals(heightScale, armVertical, hands[0], hands[1], ends[0], ends[1], GetDirection());
                    break;
                case Direction.Right:
                    SpawnVisuals(lengthScale, armHorizontal, hands[2], hands[3], ends[2], ends[3], GetDirection());
                    break;
                case Direction.Down:
                    SpawnVisuals(heightScale, armVertical, hands[4], hands[5], ends[4], ends[5], GetDirection());
                    break;
                case Direction.Left:
                    SpawnVisuals(lengthScale, armHorizontal, hands[6], hands[7], ends[6], ends[7], GetDirection());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SpawnVisuals(float scale, List<Sprite> armSprites, Sprite handA, Sprite handB, Sprite endA, Sprite endB, Vector2 direction)
        {
            float halfScale = scale / 2f - 1f;
            
            GameObject hand = new GameObject("Hand");
            hand.transform.SetParent(transform);
            hand.transform.localPosition = direction * halfScale;
            Debug.Log(direction);
            Debug.Log(halfScale);
            Debug.Log(direction * halfScale);
            hand.AddComponent<SpriteRenderer>().sprite = Random.Range(0, 2) == 0 ? handA : handB;
            
            for (int i = 0; i < (int)settings.distance; i++)
            {
                GameObject arm = new GameObject("Arm " + i);
                arm.transform.SetParent(transform);
                arm.transform.position = _initialPos + direction * (halfScale + i + 0.5f);
                arm.transform.SetParent(_parent.transform);
                arm.AddComponent<SpriteRenderer>().sprite = armSprites[i % armSprites.Count];
            }
            
            GameObject end = new GameObject("End");
            end.transform.SetParent(transform);
            end.transform.position = _initialPos + direction * (halfScale + settings.distance + 0.5f);
            end.AddComponent<SpriteRenderer>().sprite = Random.Range(0, 2) == 0 ? endA : endB;
        }

        private void OnDisable() => GameManager.Instance.Unsubscribe(this);

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
            if (!other.gameObject.CompareTag("Arrow")) 
                return;
            
            other.transform.SetParent(transform);
            NumberOfInteractions++;
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
                other.transform.parent.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.parent.SetParent(null);
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
