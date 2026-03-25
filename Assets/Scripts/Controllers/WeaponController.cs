using System.Collections.Generic;
using Arrows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class WeaponController : MonoBehaviour
    {
        
        [Header("Settings")]
        [SerializeField] private List<GameObject> arrowList;
        [SerializeField, Range(0f, 1f)] private float deadZoneOnLook;
        
        [SerializeField] private Sprite bowPlaceHolder1;
        [SerializeField] private Sprite bowPlaceHolder2;
        private SpriteRenderer _bowSpriteRenderer;
        
        public static Vector2 Direction = Vector2.right;
        private Arrow _arrowScript;
        
        public static GameObject Player;
        
        // TODO adapter le script en fonction des différentes flèches tirées
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && _arrowScript == null)
            {
                Debug.Log("Shoot");
                GameObject arrow = Instantiate(arrowList[0],transform);

                float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                Vector3 rotation = new Vector3(0,0, angle);
                _arrowScript = arrow.GetComponent<Arrow>();
                _arrowScript.transform.eulerAngles = rotation;
                gameObject.transform.eulerAngles = rotation;
                
                //Placeholder
                _bowSpriteRenderer.sprite = bowPlaceHolder2;
            }

            if (context.canceled && _arrowScript != null)
            {
                _arrowScript.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                _arrowScript.gameObject.transform.parent = null;
                _arrowScript.CanStartMoving = true;
                
                _arrowScript = null;
                
                // Placeholder 
                _bowSpriteRenderer.sprite = bowPlaceHolder1;
            }
        }
        
        private void Start()
        {
            _bowSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (Camera.main is not null)
            {
                _camera = Camera.main;
            }

            Player = transform.parent.gameObject;
        }

        private void Update()
        {
            
            if (_isUsingMouse)
            {
                _mouseScreenPosition = Mouse.current.position.ReadValue();
                _playerScreenPosition= _camera.WorldToScreenPoint(transform.position);
                _inputOnLook = new Vector2
                (
                    _mouseScreenPosition.x - _playerScreenPosition.x, 
                    _mouseScreenPosition.y - _playerScreenPosition.y
                );
            }
            
            if (_deadZoneUse)
                return;
            
            if (_currentInputOnLook == _inputOnLook)
                return;
            
            _currentInputOnLook = _inputOnLook;

            Direction = _currentInputOnLook.normalized;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, angle);
            
            if (_arrowScript is not null)
                _arrowScript.transform.eulerAngles = rotation;
            gameObject.transform.eulerAngles = rotation;
        }

        
        private Vector2 _inputOnLook = Vector2.right;
        private Vector2 _currentInputOnLook = Vector2.right;
        private Camera _camera;
        private bool _deadZoneUse;
        private bool _isUsingMouse;
        private Vector2 _mouseScreenPosition;
        private Vector2 _playerScreenPosition;
        
        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.canceled)
                return;
            
            InputDevice device = context.control.device;
            switch (device)
            {
                case Keyboard or Mouse:
                    _deadZoneUse = false;
                    _isUsingMouse = true;
                    break;
                
                case Gamepad:
                    _inputOnLook = context.ReadValue<Vector2>();
                    _deadZoneUse = false;
                    _isUsingMouse = false;
                    if (Mathf.Abs(_inputOnLook.x) <= deadZoneOnLook && Mathf.Abs(_inputOnLook.y) <= deadZoneOnLook)
                    {
                        _deadZoneUse = true;
                    }
                    break;
            }
           
        }

        public void OnRecall(InputAction.CallbackContext context)
        {
            // TODO passer le recall de arrow dans le script momentum
            if (MomentumArrowShot.Count <= 0) 
                return;
            Debug.Log("t'as cliqué frr");
            Arrow arrowCalled = MomentumArrowShot.Dequeue();
            arrowCalled.Recall();
        }
    }
}
