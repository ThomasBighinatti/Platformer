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
            Idle = 1,
            Trans = 2,
            Prepared = 4,
            Shoot = 8
        }

        public static ButterflyState CurrentButterflyState { get; private set; } = ButterflyState.Idle;

        public static void ToIdleState() => CurrentButterflyState = ButterflyState.Idle;
        public static void ToTransState() => CurrentButterflyState = ButterflyState.Trans;
        public static void ToPreparedState() => CurrentButterflyState = ButterflyState.Prepared;
        public static void ToShootState() => CurrentButterflyState = ButterflyState.Shoot;

        private void Update()
        {
            if (CurrentButterflyState == ButterflyState.Idle)
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
