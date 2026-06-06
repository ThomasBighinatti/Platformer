using System;
using System.Collections.Generic;
using GPE;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class GameManager : MonoBehaviour, ISubject
    {
    
        public static GameManager Instance;
        
        private GameObject _player;
        private Rigidbody2D _playerRb;

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
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (LevelManager.Instance != null)
            {
                _player = LevelManager.Instance.Player;
                _playerRb = _player.GetComponent<Rigidbody2D>();
            }
            else
            {
                Debug.LogWarning("GameManager : No LevelManager");
            }
        }
        
        public void OnRetry(InputAction.CallbackContext context)
        {
            if (!context.started) 
                return;
            
            
            if (_player != null)
            {
                Debug.Log("GameManager : Respawn");
                RespawnPlayer();
            }
            else
            {
                Debug.LogWarning("GameManager : No Player");
            }
        }
        
        private List<IResettable> _resettables = new List<IResettable>();
        
        public void Subscribe(IResettable resettable) => _resettables.Add(resettable);
        public void Unsubscribe(IResettable resettable)
        {
            try
            {
                _resettables.Remove(resettable);
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError("Not in subscriber list" + exception);
            }
        }
        
        public void ResetNotify()
        {
            for (int i = _resettables.Count - 1; i >= 0; i--)
            {
                _resettables[i].ResetToInitialState();
            }
        }

        public void RespawnPlayer()
        {
            if (_player == null)
            {
                Debug.LogWarning("GameManager : No Player");
                return;
            }

            if (SaveManager.Instance != null)
            {
                if (!SaveManager.Instance.checkpointPositions.TryGetValue(SaveManager.Instance.CurrentCheckpointIndex, out Vector3 spawnPosition))
                {
                    Debug.LogWarning("GameManager : No Checkpoint Found");
                    return;
                }
                
                ResetNotify();

                switch (_player.activeSelf)
                {
                    case false:
                        _player.transform.position = spawnPosition;
                        _player.SetActive(true);
                        break;
                    
                    case true:
                        _player.transform.position = spawnPosition;
                        _playerRb.linearVelocity = Vector2.zero;
                        break;
                }
            }
            else
            {
                Debug.LogWarning("GameManager : No SaveManager");
            }
            
        }

        [SerializeField] private GameState gameState;
        #region "State Machine"

        public enum GameState
        {
            Game,
            Menu
        }

        private GameState _currentGameState;
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
        public void ChangeStateToGame() => CurrentGameState = GameState.Game;
        public void ChangeStateToMenu() => CurrentGameState = GameState.Menu;

        #endregion

        private void Start()
        {
            _currentGameState = gameState;
        }

        public void StartNewGame()
        {
            SaveSystem.SaveSystem.DeleteSave();
            CurrentGameState = GameState.Game;
        }
        
        public void ContinueGame()
        {
            CurrentGameState = GameState.Game;
        }
        
        public void StartFromLevel(int checkpointIndex)
        {
            SaveSystem.SaveSystem.DeleteSave();
            SaveManager.Instance.ForceSetCheckpoint(checkpointIndex);
            CurrentGameState = GameState.Game;
        }
        
        public void QuitGame() =>  Application.Quit();
    }
}
