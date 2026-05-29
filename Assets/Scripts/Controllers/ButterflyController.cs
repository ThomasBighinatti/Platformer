using System;
using UnityEngine;

namespace Controllers
{
    public class ButterflyController : MonoBehaviour
    {
        
        [Header("Visual")]
        [SerializeField] private GameObject bfVisual;
        [Space(10f)]
        
        [Header("Butterfly Settings")]
        [SerializeField] private float speed = 1;
        [SerializeField] private float distance = 1;
        [SerializeField] private float butterflyPointerOffset;
        [SerializeField] private float shapeModifier = 0.5f;
        [SerializeField] private float smoothTime = 0.3f;
        [Space(10f)]

        [Header("Pointers")]
        [SerializeField] private GameObject playerPointer;
        [SerializeField] private GameObject butterflyPointer;
        [SerializeField] private GameObject pointerRef;
        
        private Vector2 _velocity;
        private float _time;
        
        private SpriteRenderer _bfSprite;
        
        private float _previousXWorldPosition;

        private void Awake()
        {
            try
            {
                _bfSprite = bfVisual.GetComponent<SpriteRenderer>();
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError("No SpriteRenderer / Child" + exception);
            }
        }
        
        private void Update()
        {
            ButterFlyPointerRefresh();
            MoveButterfly();
            MoveButterflyVisual();
        }

        private void ButterFlyPointerRefresh()
        {
            pointerRef.transform.position = playerPointer.transform.position; // suis le player pointer
            
            butterflyPointer.transform.localPosition = new Vector2(
                pointerRef.transform.localPosition.x,
                pointerRef.transform.localPosition.y * shapeModifier + butterflyPointerOffset // applique offset et change la forme (ovale)
            );
        }

        private void MoveButterfly()
        {
            transform.position= Vector2.SmoothDamp(transform.position, butterflyPointer.transform.position, ref _velocity, smoothTime);
        }

        private bool IsMovingRight => bfVisual.transform.position.x > _previousXWorldPosition;
        private void MoveButterflyVisual()
        {
            _time += Time.deltaTime * speed;
            float timeY = _time * 1.23f; 
            float xCoords = Mathf.PerlinNoise(Mathf.Cos(_time) * 1.5f + 10f, Mathf.Sin(_time) * 1.5f + 10f) * 2 - 1;
            float yCoords = Mathf.PerlinNoise(Mathf.Sin(timeY) * 1.5f + 20f, Mathf.Cos(timeY) * 1.5f + 20f) * 2 - 1;

            Vector2 offset = new Vector2(xCoords, yCoords) * distance;

            bfVisual.transform.localPosition = offset;
            _bfSprite.flipX = IsMovingRight;
            
            _previousXWorldPosition = bfVisual.transform.position.x;
        }
        
    }
}
