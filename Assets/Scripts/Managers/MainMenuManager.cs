using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject selectLevelPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private List<StageData> levelList;
        [SerializeField] private Image levelImage;
        [SerializeField] private int[] levelStartCheckpoints = { 0, 7, 13 };
        
        private int levelIndex = 0;
    
    

        public void NewGame() => GameManager.Instance.StartNewGame();
        public void ContinueGame() => GameManager.Instance.ContinueGame();

        private void Start()
        {
            UpdateStageDisplay();
        }

        public void SelectLevel()
        {
            if (selectLevelPanel != null && mainMenuPanel != null)
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
            if (levelIndex < levelList.Count - 1)
            {
                levelIndex++;
                UpdateStageDisplay();
            }
            else return;
        }

        public void SelectPreviousStage()
        {
            if (levelIndex > 0)
            {
                levelIndex--;
                UpdateStageDisplay();
            }
            else return;
        }

        private void UpdateStageDisplay()
        {
            levelImage.sprite = levelList[levelIndex].levelSprite;
        }
    
        public void StartFromSelectedLevel()
        {
            int startCheckpoint = levelStartCheckpoints[levelIndex];
            GameManager.Instance.StartFromLevel(startCheckpoint);
        }
    }
}
