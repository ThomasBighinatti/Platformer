using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] private GameObject pinPointer;
        public GameObject PinPointer => pinPointer; 
        
        [SerializeField] private List<Animator> playerUiArrows;
        public List<Animator> PlayerUiArrows => playerUiArrows; 
        
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
            if (pinPointer == null) Debug.LogWarning("LevelManager : No Pin Pointer ");
            if (playerUiArrows == null) Debug.LogWarning("LevelManager : No Player Ui Arrows ");
        }
    }
}
