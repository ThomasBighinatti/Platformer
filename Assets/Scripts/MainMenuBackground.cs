using Managers;
using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    void StartGame()
    {
        GameManager.Instance.StartNewGame();
    }

    void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }
}
