using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
        [SerializeField] private GameObject nextLevelButton;
        [SerializeField] private GameObject defaultButton;
        [SerializeField] private Animator animator; 
        
        private int levelIndex = 0;



        public void NewGame()
        {
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.InputMenu);
            animator.Play("PressStartMenu");
        }
        public void ContinueGame()
        {
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.InputMenu);
            animator.Play("PressContinueMenu");
        } 

        private void Start()
        {
            UpdateStageDisplay();
            SoundManager.Instance.CurrentMusicToPlay = 0;
        }

        public void EnableSelect()
        {
            if (selectLevelPanel != null && mainMenuPanel != null)
            {
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.InputMenu);
                mainMenuPanel.SetActive(false);
                selectLevelPanel.SetActive(true);
                StartCoroutine(SelectDefaultButton());
            }
            else
            {
                Debug.LogWarning("Menu Manager : missing panel");
            }
        }
        public void EnableMenu()
        {
            if (selectLevelPanel != null && mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
                selectLevelPanel.SetActive(false);
                StartCoroutine(MenuDefaultButton());
            }
            else
            {
                Debug.LogWarning("Menu Manager : missing panel");
            }
        }

        IEnumerator SelectDefaultButton()
        {
            yield return null; // attend une frame
            EventSystem.current.SetSelectedGameObject(nextLevelButton);
        }
        
        IEnumerator MenuDefaultButton()
        {
            yield return null; // attend une frame
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        public void SelectNextStage()
        {
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.ArrowMenu);
            if (levelIndex < levelList.Count - 1)
            {
                levelIndex++;
                UpdateStageDisplay();
            }
            else return;
        }

        public void SelectPreviousStage()
        {
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.ArrowMenu);
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
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.InputMenu);
            int startCheckpoint = levelStartCheckpoints[levelIndex];
            GameManager.Instance.StartFromLevel(startCheckpoint);
        }
        
        public void QuitGame() => GameManager.Instance.QuitGame();
    }
}
