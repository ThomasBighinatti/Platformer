using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> arrowList;
        public bool isLooking;
        public Vector3 direction;

        public void OnShoot(InputAction.CallbackContext context)
        {
            
            if (context.started)
            {
                Debug.Log("oui" + context);
                isLooking = true;
                GameObject arrow = Instantiate(arrowList[0]); 
            }

            if (context.canceled)
            {
                Debug.Log("non" + context);
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!isLooking) 
                return;
            direction = new Vector3(0, 0);
        }
    }
}
