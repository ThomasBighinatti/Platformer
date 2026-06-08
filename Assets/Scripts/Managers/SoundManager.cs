using System.Collections;
using UnityEngine;

namespace Managers
{
    
    public class SoundManager : MonoBehaviour
    {
        
        public static SoundManager Instance;

        [Header("Sounds")]
        [SerializeField] private AudioClip[] sfx;
        [SerializeField] private AudioClip[] mainMusics;
        

        [Header("Audio Sources")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private int musicIndexAtSpawn;

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

        private void Start()
        {
            _currentMusicTooPlay = musicIndexAtSpawn;
            PlayMainMusic();
        }
        
        #region Main

        private int _currentMusicTooPlay;

        public void ChangeMainMusic(int index)
        {
            if (index < 0 || index >= mainMusics.Length)
                return;
            
            _currentMusicTooPlay = index;
            PlayMainMusic();
        }

        private void PlayMainMusic()
        {
            if (_currentMusicTooPlay == 0)
            {
                PlayFirstMusic();
            }
            else
            {
                StopMusic();
            }
        }
        
        private void PlayFirstMusic()
        {
            Debug.Log(_currentMusicTooPlay);
            if (mainMusics[0] == null)
            {
                return;
            }

            musicSource.clip = mainMusics[0];
            musicSource.loop = true;
            musicSource.Play();
        }

        private IEnumerator PlayMusicSequence(int index)
        {
            Debug.Log(_currentMusicTooPlay + "caca");
            
            if (index < 0 || index >= mainMusics.Length || mainMusics[index] is null)
                yield break;

            musicSource.volume = 100f;

            musicSource.clip = mainMusics[index];
            musicSource.Play();
            
            yield return new WaitForSeconds(musicSource.clip.length);
            
            StartCoroutine(PlayMusicSequence(_currentMusicTooPlay));
        }

        private void StopMusic()
        {
            StartCoroutine(StartFade(3f, 0f));
        }
        
        private IEnumerator StartFade(float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = musicSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            musicSource.Stop();
            StartCoroutine(PlayMusicSequence(_currentMusicTooPlay));
        }
        
        #endregion
        
        #region SFX

        public void SoundExample()
        {
            var soundToPlay = sfx[0];
            sfxSource.PlayOneShot(soundToPlay);
        }
        #endregion
    }
}
