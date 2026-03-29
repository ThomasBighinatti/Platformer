using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int index;

    private void Start()
    {
        SaveManagerV2.SINGLETON.RegisterCheckpoint(index, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isNew = SaveManagerV2.SINGLETON.ChangeCurrentCheckpoint(index);
        if (!isNew) return;

        SaveManagerV2.SINGLETON.Save();
    }
}