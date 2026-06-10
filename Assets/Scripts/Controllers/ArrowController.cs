using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    
    public class ArrowController : MonoBehaviour
    {
        
        [Header("Settings")]
        [SerializeField, Range(0f, 1f)] private float deadZoneOnLook;
        
        private Vector2 _inputOnLook = Vector2.right;
        private Vector2 _currentInputOnLook = Vector2.right;
        private Camera _camera;
        private bool _deadZoneUse;
        private bool _isUsingMouse;
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Pause)
                return;
            
            InputDevice device = context.control.device;
            if (device is Keyboard or Mouse && !_isUsingMouse) return;
            if (device is Gamepad && _isUsingMouse) return;
            
            if (ArrowManager.Instance != null)
            {
                if (context.started && !SoundManager.Instance.AimSoundIsPlaying())
                {
                    SoundManager.Instance.AimStartSound();
                }
                if (context.canceled && SoundManager.Instance.AimSoundIsPlaying())
                {
                    SoundManager.Instance.AimEndSound();
                    ArrowManager.Instance.CreateArrow();
                    ArrowManager.Instance.ShootArrow();
                }
            }
            else
            {
                Debug.LogWarning("ArrowController : No ArrowManager");
            }
        }
        
        private void Start()
        {
            if (Camera.main is not null)
            {
                _camera = Camera.main;
            }
            else
            {
                Debug.LogError("ArrowController : Camera Problem");
            }
        }

        private void Update()
        {
            SetInputOnLook();
        }

        private void SetInputOnLook()
        {
            
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Pause)
                return;
            
            if (_isUsingMouse)
            {
                _inputOnLook = Mouse.current.position.ReadValue() - (Vector2)_camera.WorldToScreenPoint(transform.position);
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
            
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Pause)
                return;
            
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
                    _isUsingMouse = false;
                    _deadZoneUse = _inputOnLook.magnitude <= deadZoneOnLook;
                    break;
            }
        }

        public void OnRecall(InputAction.CallbackContext context)
        {
            
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.Pause)
                return;
            
            if (!context.performed)
                return;
            
            if(ArrowManager.Instance != null)
            {
                ArrowManager.Instance.RecallArrow();
            }
            else
            {
                Debug.LogWarning("ArrowController : No ArrowManager");
            }
        }
        
    }
}
