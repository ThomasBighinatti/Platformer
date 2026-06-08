using UnityEngine;

namespace Arrows
{
    public class ExplosionRelay : MonoBehaviour
    {
        public void ExplosionEndDeactivation()
        {
            gameObject.SetActive(false);
        }
    }
}
