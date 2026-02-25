using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> arrowList;
        [SerializeField, Range(0f, 1f)] private float deadZoneOnLook;
        
        public static Vector2 Direction = Vector2.right;
        private Arrow _arrowScript;
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && _arrowScript == null)
            {

                GameObject arrow = Instantiate(arrowList[0],transform.position,Quaternion.identity);
                float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                Vector3 rotation = new Vector3(0,0, angle);
                _arrowScript = arrow.GetComponent<Arrow>();
                _arrowScript.transform.eulerAngles = rotation;
            }

            if (context.canceled && _arrowScript != null)
            {
                _arrowScript.CanStartMoving = true;
                _arrowScript = null;
            }
        }

        private Vector2 _lastDirection = Vector2.right;
        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            if (Mathf.Abs(input.x) <= deadZoneOnLook && Mathf.Abs(input.y) <= deadZoneOnLook)
                return;
            Direction = context.ReadValue<Vector2>().normalized;
            
            if (Direction != Vector2.zero)
            {
                _lastDirection = Direction;
                if (_arrowScript == null) 
                    return;
                float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                Vector3 rotation = new Vector3(0,0, angle);
                _arrowScript.transform.eulerAngles = rotation;
            } 
            
            else
            {
                Direction = _lastDirection;
            }
        }
    }
}
