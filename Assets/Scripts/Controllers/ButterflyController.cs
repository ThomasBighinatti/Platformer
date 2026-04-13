using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Controllers
{
    public class ButterflyController : MonoBehaviour
    {
        
        [SerializeField] private GameObject bfVisual;
        [SerializeField] private float speed = 1;
        [SerializeField] private float distance = 1;
        private float _time;

        [SerializeField] private GameObject playerPointer;
        [SerializeField] private GameObject butterflyPointer;
        [SerializeField] private GameObject pointerRef;
        [SerializeField] private float butterflyPointerOffset;
        [SerializeField] private float shapeModifier = 0.5f;

        public enum ButterflyState
        {
            Idle = 0,
            Trans = 1,
            Prepared = 2,
            Shoot = 3
        }

        [SerializeField] private Animator animator;
        
        private ButterflyState _currentButterflyState = ButterflyState.Idle;
        public ButterflyState CurrentButterflyState
        {
            get => _currentButterflyState;
            private set
            {
                _currentButterflyState = value;
                animator.SetInteger(nameof(State), (int)_currentButterflyState);
                Debug.Log(_currentButterflyState);
            }
        }

        public void ToIdleState() => CurrentButterflyState = ButterflyState.Idle;
        public bool IsIdleState => CurrentButterflyState == ButterflyState.Idle;
        
        public void ToTransState() => CurrentButterflyState = ButterflyState.Trans;
        
        public void ToPreparedState() => CurrentButterflyState = ButterflyState.Prepared;
        
        public void ToShootState() => CurrentButterflyState = ButterflyState.Shoot;

        private void Update()
        {
            if (IsIdleState)
            {
                ButterFlyPointerRefresh();
                
                MoveButterfly();
                MoveButterflyVisual();
            }

            if (CurrentButterflyState == ButterflyState.Prepared)
            {
                
            }
        }

        private void ButterFlyPointerRefresh()
        {
            pointerRef.transform.position = playerPointer.transform.position;
            butterflyPointer.transform.localPosition = new Vector2(
                pointerRef.transform.localPosition.x,
                pointerRef.transform.localPosition.y * shapeModifier + butterflyPointerOffset
            );
        }

        private Vector2 _velocity;
        [SerializeField] private float smoothTime = 0.3f;

        private void MoveButterfly()
        {
            Vector2 newPosition = Vector2.SmoothDamp(
                transform.position, 
                butterflyPointer.transform.position, 
                ref _velocity,
                smoothTime);
            transform.position = newPosition;
        }
        
        private bool IsMovingRight => bfVisual.transform.position.x > _previousXWorldPosition;
        private float _previousXWorldPosition;
        

        private void MoveButterflyVisual()
        {
            _time += Time.deltaTime * speed;
            float timeY = _time * 1.23f;
            float xCoords = Mathf.PerlinNoise(Mathf.Cos(_time) * 1.5f + 10f, Mathf.Sin(_time) * 1.5f + 10f) * 2 - 1;
            float yCoords = Mathf.PerlinNoise(Mathf.Sin(timeY) * 1.5f + 20f, Mathf.Cos(timeY) * 1.5f + 20f) * 2 - 1;

            Vector2 offset = new Vector2(xCoords, yCoords) * distance;

            bfVisual.transform.localPosition = offset;
            
            bfVisual.transform.localScale = new Vector3(IsMovingRight ? 1f : -1f, 1f, 1f);
            
            _previousXWorldPosition = bfVisual.transform.position.x;
        }
    }
}
