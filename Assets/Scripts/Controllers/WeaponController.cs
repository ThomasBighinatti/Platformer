using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> arrowList;
        
        public static Vector2 Direction = Vector2.right;
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            
            if (context.started)
            {
                Debug.Log("oui" + context);
                GameObject arrow = Instantiate(arrowList[0],transform.position,Quaternion.Euler(new Vector3(0,0,0.5f)/*Direction.x,0,Direction.y*/)); 
            }

            if (context.canceled)
            {
                Debug.Log("non" + context);
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Direction = context.ReadValue<Vector2>();
        }
    }
}
