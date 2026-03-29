using System;
using Datas;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public NumCheckPoint checkpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        SaveManagerV2.SINGLETON.ChangeCurrentCheckpoint(checkpoint);
    }
}
