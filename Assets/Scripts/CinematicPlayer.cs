using Managers;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject cinematicCanvas;
    private string nextScene = "Menu";
    

    public void PlayCinematic()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.Pause;
        cinematicCanvas.SetActive(true);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd; // loopPointReached = qunad la video se finit
    }
    public void SkipCinematic() => EndCinematic();

    private void OnVideoEnd(VideoPlayer vp) => EndCinematic();

    private void EndCinematic()
    {
        cinematicCanvas.SetActive(false);
        GameManager.Instance.CurrentGameState = GameManager.GameState.Menu;
    }
}
