using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        

        private void Start()
        {
            _bowSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

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
        
        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.canceled)
                return;
            
            Vector2 input = new Vector2();
            InputDevice device = context.control.device;
            switch (device)
            {
                case Keyboard or Mouse:
                    Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
                    Vector2 playerScreenPosition= Camera.main.WorldToScreenPoint(transform.position);
                    input = new Vector2
                    (
                        mouseScreenPosition.x - playerScreenPosition.x, 
                        mouseScreenPosition.y - playerScreenPosition.y
                    );
                    break;
                
                case Gamepad:
                    input = context.ReadValue<Vector2>();
                    if (Mathf.Abs(input.x) <= deadZoneOnLook && Mathf.Abs(input.y) <= deadZoneOnLook)
                        return;
                    break;
            }
                
            Direction = input.normalized;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, angle);
            
            if (!(_arrowScript == null))
                _arrowScript.transform.eulerAngles = rotation;
            gameObject.transform.eulerAngles = rotation;
            
        }
    }
}
