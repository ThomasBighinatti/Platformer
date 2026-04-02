using UnityEngine;

namespace SaveSystem
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private int index;

        private void Start()
        {
            SaveManager.Instance.RegisterCheckpoint(index, transform.position);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            bool isNew = SaveManager.Instance.ChangeCurrentCheckpoint(index);
            if (!isNew) return;

            SaveManager.Instance.Save();
        }
    }
}