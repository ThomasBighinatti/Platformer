using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject cinematicCanvas;
    

    public void PlayCinematic()
    {
        SoundManager.Instance.StopSound();
        Time.timeScale = 0f;
        //GameManager.Instance.CurrentGameState = GameManager.GameState.Pause;
        cinematicCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd; // loopPointReached = qunad la video se finit
    }
    public void SkipCinematic() => EndCredits();

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (SceneManager.GetActiveScene().name == "asemblage")
            EndCredits();
        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            Debug.Log(" OnvideoEnd");
            EndIntro();
        }
        
    }

    private void EndIntro()
    {
        Debug.Log(" End Intro");
        Time.timeScale = 1f;
        SoundManager.Instance.VolumeNormal();
        cinematicCanvas.SetActive(false);
        GameManager.Instance.StartNewGame();
        Debug.Log("End intro fini et la normalement tu lances une new game ?");
    }
    private void EndCredits()
    {
        SoundManager.Instance.VolumeNormal();
        cinematicCanvas.SetActive(false);
        GameManager.Instance.CurrentGameState = GameManager.GameState.Menu;
        Time.timeScale = 1f;
    }
}
