using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public class SaveManagerV2 : MonoBehaviour
{
    public static SaveManagerV2 SINGLETON;
    public SystemOption systemOption;
    private int _currentCheckpointIndex = 0;

    private readonly Dictionary<int, Vector3> _checkpointPositions = new();

    [Header("Settings")]
    public bool EncryptData = false;

    public DataToSave Data;

    public void Awake()
    {
        if (SINGLETON == null)
            SINGLETON = this;
        else
            Destroy(this);

        new SaveSystemV2(systemOption);
        Data.DatasToSave = new List<ObjectData> { new ObjectData() };
    }

    private void Start()
    {
        StartCoroutine(LoadAfterRegistration());
    }

    private IEnumerator LoadAfterRegistration()
    {
        yield return null; // attend que les checkpoints enregistrent leur position dabs le start
        Load();
    }

    public void RegisterCheckpoint(int index, Vector3 position)
    {
        _checkpointPositions[index] = position;
    }

    public bool ChangeCurrentCheckpoint(int index)
    {
        if (index <= _currentCheckpointIndex)
            return false;

        _currentCheckpointIndex = index;
        return true;
    }

    public void Save()
    {
        ObjectData objectData = Data.DatasToSave[0];
        objectData.checkpointIndex = _currentCheckpointIndex;
        Data.DatasToSave[0] = objectData;

        SaveSystemV2.SaveData(Data);
    }

    private void Load()
    {
        DataToSave loadedData = SaveSystemV2.LoadData();

        if (loadedData.DatasToSave == null || loadedData.DatasToSave.Count == 0)
            return;

        Data = loadedData;
        _currentCheckpointIndex = Data.DatasToSave[0].checkpointIndex;

        if (!_checkpointPositions.TryGetValue(_currentCheckpointIndex, out Vector3 spawnPosition))
            return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player is not null)
            player.transform.position = spawnPosition;
    }
}