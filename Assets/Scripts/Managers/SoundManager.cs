
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [Header("Sounds")]
        [SerializeField] private AudioClip[] sfx;
        [SerializeField] private AudioClip mainMusic;
        

        [Header("Audio Sources")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

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
            if (mainMusic == null)
                return;
            musicSource.clip = mainMusic;
            musicSource.Play();
        }

        public void SoundExample()
        {
            var soundToPlay = sfx[0];
            sfxSource.PlayOneShot(soundToPlay);
        }
    }
}
