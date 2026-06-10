using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

        [SerializeField] public GameObject pauseMenu;
        
        [SerializeField] private GameObject defaultSelectedButton;
        
        
        [SerializeField] private CinematicPlayer cinematicPlayer;
        public CinematicPlayer CinematicPlayer => cinematicPlayer;

        public void GoToMainMenu() => GameManager.Instance.ChangeStateToMenu();
        
        public void Quit() => GameManager.Instance.QuitGame();
        
        public void Resume() => GameManager.Instance.ChangeStateToGame();
        
        public void RespawnPlayer() => GameManager.Instance.RespawnPlayer();

        public void Pause(InputAction.CallbackContext context) => GameManager.Instance.OnPause(context);
        
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

        public void ShowPauseMenu()
        {
            pauseMenu.SetActive(true);
            StartCoroutine(SelectDefaultButtonCoroutine());
        }

        public void HidePauseMenu()
        {
            pauseMenu.SetActive(false);
        }

        private IEnumerator SelectDefaultButtonCoroutine()
        {
            yield return null;
            if (defaultSelectedButton == null)
            {
                Debug.Log("LevelManager : no defaultSelectedButton");
                yield break;
            }
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        }
    }
}
