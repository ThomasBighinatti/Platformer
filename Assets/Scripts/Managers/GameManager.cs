using UnityEngine;

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
