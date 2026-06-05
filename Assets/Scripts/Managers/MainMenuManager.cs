using System;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject selectLevelPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private List<StageData> stageList;
    [SerializeField] private Image stageImage;
    private int stageIndex = 1;

    public void NewGame() => GameManager.Instance.StartNewGame();
    public void ContinueGame() => GameManager.Instance.ContinueGame();

    private void Start()
    {
        UpdateStageDisplay();
    }

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

    public void SelectNextStage()
    {
        if (stageIndex < stageList.Count)
        {
            stageIndex++;
            UpdateStageDisplay();
        }
        else return;
    }

    public void SelectPreviousStage()
    {
        if (stageIndex > 0)
        {
            stageIndex--;
            UpdateStageDisplay();
        }
        else return;
    }

    private void UpdateStageDisplay()
    {
        stageImage.sprite = stageList[stageIndex].stageSprite;
    }
}
