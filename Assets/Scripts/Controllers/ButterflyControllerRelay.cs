using UnityEngine;

namespace Controllers
{
    public class ButterflyControllerRelay : MonoBehaviour
    {
        [SerializeField] private ButterflyController butterflyController;

        public void ToPreparedState() => butterflyController.ToPreparedState();
        public void ToIdleState() => butterflyController.ToIdleState();
        
    }
}
