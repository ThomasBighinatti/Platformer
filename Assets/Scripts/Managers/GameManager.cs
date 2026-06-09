using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
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
            if (LevelManager.Instance != null && _currentGameState == GameState.Game)
            {
                _player = LevelManager.Instance.Player;
                _playerRb = _player.GetComponent<Rigidbody2D>();
            }
            else
            {
                Debug.LogWarning("GameManager : No LevelManager or player is in menu");
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            if (context.started)
            {
                HandlePause();
            }
        }

        public void HandlePause()
        {
            if (_currentGameState == GameState.Pause)
                    ChangeStateToGame();
            else if (_currentGameState == GameState.Game)
                ChangeStateToPause();
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
                
                _player.SetActive(true);
                _player.transform.position = spawnPosition; 
                _playerRb.linearVelocity = Vector2.zero;
                _playerScript.DeactivateExplosionAnimator();
                 
                
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
            Menu,
            Pause
        }

        private bool _wasInPause;
        private GameState _currentGameState;

        public GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {
                _currentGameState = value;

                if (_currentGameState == GameState.Pause)
                    Pause();

                else
                {
                    if (_wasInPause && _currentGameState == GameState.Menu)
                    {
                        Resume();
                        SceneManager.LoadScene(GetSceneByState());
                    }
                    else if (_wasInPause)
                        Resume();
                    
                    else 
                        SceneManager.LoadScene(GetSceneByState());
                }
            }
        }

        private string GetSceneByState()
        {
            return CurrentGameState switch
            {
                GameState.Game => "asemblage", //"asemblage" pour tester
                GameState.Menu => "Menu",
                _ => ""
            };
        }

        public void Pause()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.pauseMenu.SetActive(true);
            }
            Time.timeScale = 0f;
            _wasInPause = true;
        }

        public void Resume()
        {
            LevelManager.Instance.pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            _wasInPause = false;
        }
        public void ChangeStateToGame() => CurrentGameState = GameState.Game;
        public void ChangeStateToMenu() => CurrentGameState = GameState.Menu;

        public void ChangeStateToPause() => CurrentGameState = GameState.Pause;
        #endregion


        private PlayerController _playerScript;
            
        private void Start()
        {
            _currentGameState = gameState;
            _playerScript = _player.GetComponent<PlayerController>();
        }

        public void StartNewGame()
        {
            SaveSystem.SaveSystem.DeleteSave();
            CurrentGameState = GameState.Game;
        }
        
        public async void ContinueGame()
        {
            try
            {
                CurrentGameState = GameState.Game;
                await Task.Delay(10);
                RespawnPlayer();
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }
        
        public async void StartFromLevel(int checkpointIndex)
        {
            try
            {
                SaveSystem.SaveSystem.DeleteSave();
                CurrentGameState = GameState.Game;
                SaveManager.Instance.ForceSetCheckpoint(checkpointIndex);
                await Task.Delay(10); // pour attendre le chargement et faire spawn le player (jsp pourquoi mais yield return null n'etait pas suffisant)
                RespawnPlayer();
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

        public void QuitGame() =>  Application.Quit();
        
    }
}
