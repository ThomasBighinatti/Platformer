using Controllers;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        
        public GameObject player;
        public ButterflyController butterfly;
        public Arrows.Momentum momentumPrefab;
        public GameObject pointerParent;
        public GameObject pointer;
        public Transform playerTransform;
        
    }
}
