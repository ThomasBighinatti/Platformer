using Managers;
using UnityEngine;

namespace SaveSystem
{
    
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        
        [Tooltip("Number of the checkpoint (first checkpoint must be set to 1)")]
        [SerializeField] private int index;

        private void Start()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.RegisterCheckpoint(index, transform.position);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            bool isNew = SaveManager.Instance.ChangeCurrentCheckpoint(index);
            if (!isNew) 
                return;

            SaveManager.Instance.Save();
        }
    }
}