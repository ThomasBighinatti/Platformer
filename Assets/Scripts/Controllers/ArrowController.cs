using System.Collections.Generic;
using Arrows;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class ArrowController : MonoBehaviour
    {
        
        [Header("Settings")]
        [SerializeField, Range(0f, 1f)] private float deadZoneOnLook;
        
        private Arrow _arrowScript;
        
        private Vector2 _inputOnLook = Vector2.right;
        private Vector2 _currentInputOnLook = Vector2.right;
        private Camera _camera;
        private bool _deadZoneUse;
        private bool _isUsingMouse;
        private Vector2 _mouseScreenPosition;
        private Vector2 _playerScreenPosition;
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && ArrowManager.Instance.ArrowScriptIsNull())
            {
                ArrowManager.Instance.CreateArrow();
            }

            if (context.canceled && !ArrowManager.Instance.ArrowScriptIsNull())
            {
                ArrowManager.Instance.ShootArrow();
            }
        }
        
        private void Start()
        {
            if (Camera.main is not null)
            {
                _camera = Camera.main;
            }
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

            ArrowManager.Instance.LookingTowards = _currentInputOnLook.normalized;
            
        }
        
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
            ArrowManager.Instance.RecallArrow();
        }
    }
}
