using Arrows;
using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    
    public class LevelManager : MonoBehaviour
    {
        
        public static LevelManager Instance;
        
        [SerializeField] private GameObject player;
        public GameObject Player => player;

        [SerializeField] private GameObject pointerParent;
        public GameObject PointerParent => pointerParent;

        [SerializeField] private GameObject pointer;
        public GameObject Pointer => pointer;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            Init();
        }

        private void Init()
        {
            if (player == null) Debug.LogWarning("LevelManager : No Player");
            if (pointerParent == null) Debug.LogWarning("LevelManager : No Pointer Parent");
            if (pointer == null) Debug.LogWarning("LevelManager : No Pointer");
        }
    }
}
