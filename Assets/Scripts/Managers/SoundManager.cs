using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        #region Main
        
        private Coroutine _musicCoroutine;
        private int? _nextMusicIndex;

        private int? _currentMusicToPlay;
        public int? CurrentMusicToPlay
        {
            get => _currentMusicToPlay;
            set
            {
                if (_currentMusicToPlay == value) 
                    return;

                bool isComingFromMenu = _currentMusicToPlay is 0 or null;
                bool isGoingToMenu = value == 0;
                _currentMusicToPlay = value;

                if (_currentMusicToPlay == null) 
                    return;

                if (isComingFromMenu || isGoingToMenu)
                {
                    _nextMusicIndex = null;
                    if (_musicCoroutine != null) 
                        StopCoroutine(_musicCoroutine);
                    
                    musicSource.Stop();
                    _musicCoroutine = StartCoroutine(PlayMusic((int)_currentMusicToPlay));
                }
                else
                {
                    _nextMusicIndex = _currentMusicToPlay;
                }
            }
        }

        private IEnumerator PlayMusic(int index)
        {
            if (index < 0 || index >= mainMusics.Length || mainMusics[index] is null)
                yield break;

            musicSource.volume = 1f;
            musicSource.clip = mainMusics[index];
            musicSource.loop = index == 0;
            musicSource.Play();

            if (index == 0) 
                yield break; 

            while (_nextMusicIndex == null)
            {
                yield return new WaitForSecondsRealtime(musicSource.clip.length);

                if (_nextMusicIndex != null)
                    break;
                
                musicSource.Play();
            }
            
            int next = (int)_nextMusicIndex;
            _nextMusicIndex = null;
            _musicCoroutine = StartCoroutine(PlayMusic(next));
        }
        
        #endregion
        
        #region SFX
        
        public enum MainSfx
        {
            Running = 0,
            StartJump = 1,
            EndJump = 2,
            Death = 3,
            Respawn = 4,
            ArrowPlanted = 5,
            ArrowReturn = 6,
            ArrowShot = 7,
            ArrowAim = 8,
            MovingPlat = 9,
            StickyPlanted = 10,
            StickyReturn = 11,
            CrystalBroken = 12,
            CrystalImpact = 13,
            CrystalStartBreak = 14,
            ScreenSelect = 15,
            ArrowMenu = 16,
            InputMenu = 17
        }
        
        public void SoundPlay(MainSfx sound)
        {
            AudioClip soundToPlay = sfx[(int)sound];
            sfxSource.PlayOneShot(soundToPlay);
        }
        
        public void Vibration(float low, float high, float duration)
        {
            if (Gamepad.current == null) 
                return;
            
            StartCoroutine(VibrationCoroutine(low, high, duration));
        }

        private IEnumerator VibrationCoroutine(float low, float high, float duration)
        {
            Gamepad.current.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(duration);
            Gamepad.current.ResetHaptics();
        }
        
        
        #endregion
    }
}
