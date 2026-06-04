using Managers;
using UnityEngine;

namespace Arrows
{
    public class UiArrow : MonoBehaviour
    {
        public void OnShootAnimEnd() => gameObject.SetActive(false);
    }
}
