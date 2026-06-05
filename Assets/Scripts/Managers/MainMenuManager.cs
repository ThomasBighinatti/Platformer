using System;
using Managers;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject selectLevelPanel;
    [SerializeField] private GameObject mainMenuPanel;
    

    public void NewGame() => GameManager.Instance.StartNewGame();
    public void ContinueGame() => GameManager.Instance.ContinueGame();

    public void SelectLevel()
    {
        if (selectLevelPanel == null && mainMenuPanel == null)
        {
            mainMenuPanel.SetActive(false);
            selectLevelPanel.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("Menu Manager : missing panel");
        }
    } 

   
}
