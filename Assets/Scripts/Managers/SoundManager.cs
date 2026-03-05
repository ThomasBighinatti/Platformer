
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [Header("Sounds")]
        [SerializeField] private AudioClip[] vfx;
        [SerializeField] private AudioClip mainMusic;
        

        [Header("Audio Sources")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource vfxSource;

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

        private AudioSource FindEmptyAudioSource()
        {
            if (!vfxSource.isPlaying)
            {
                return vfxSource;
            }

            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            return newSource;
            
            // TODO systeme de pulling des audiosources et ajouter à la liste si ça en crée une nouvelle
            // faire un test d'audio source tous les temps de temps pour voir combien il y en a et en supprimer
        }

        public void SoundExample()
        {
            var soundToPlay = vfx[0];
            AudioSource source = FindEmptyAudioSource();
            source.clip = soundToPlay;
            source.Play();
        }
    }
}
