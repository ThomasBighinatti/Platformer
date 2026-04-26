using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
    
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(transform.parent.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(transform.parent);
        }

        [SerializeField] private GameObject player;
        
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (player == null)
                player = LevelManager.Instance.player;
        }

        public void RespawnPlayer()
        {
            if (player is null) 
                return;
            
            if (!SaveManager.Instance.CheckpointPositions.TryGetValue(SaveManager.Instance.CurrentCheckpointIndex, out Vector3 spawnPosition))
                return;
            
            player.transform.position = spawnPosition;
        }
    
        /*
    #region "State Machine"

    public enum GameState
    {
        Game,
        Menu
    }

    private GameState _currentGameState = GameState.Game;
    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState == value) return;
            _currentGameState = value;
            SceneManager.LoadScene(GetSceneByState());
        }
    }

    private string GetSceneByState()
    {
        return CurrentGameState switch
        {
            GameState.Game => "Game",
            GameState.Menu => "Menu",
            _ => ""
        };
    }

    #endregion */

    }
}
