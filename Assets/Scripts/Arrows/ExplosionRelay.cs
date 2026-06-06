using UnityEngine;

namespace Arrows
{
    public class ExplosionRelay : MonoBehaviour
    {
        public void ExplosionEndDeactivation()
        {
            Debug.Log("ExplosionRelay : Deact");
            gameObject.SetActive(false);
        }
    }
}
