using System.Collections;
using System.Collections.Generic;
using Datas;
using SaveSystem;
using UnityEngine;

namespace Managers
{
    
    public class SaveManager : MonoBehaviour
    {
        
        public static SaveManager Instance;
        
        

        [Header("Settings")]
        public SaveSystemOption saveSystemOption;
        public int CurrentCheckpointIndex { get; private set; }
        
        public readonly Dictionary<int, Vector3> checkpointPositions = new Dictionary<int, Vector3>();
        
        public bool encryptData;
        public DataToSave data;
        
        private GameObject _player;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(transform.parent.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(transform.parent);

            SaveSystem.SaveSystem saveSystem = new SaveSystem.SaveSystem(saveSystemOption);
            data.datasToSave = new List<ObjectData> { new ObjectData() };
        }

        private void Start()
        {
            StartCoroutine(LoadAfterRegistration());

            if (LevelManager.Instance != null)
            {
                _player = LevelManager.Instance.Player;
            }
            else
            {
                Debug.LogWarning("SaveManager : No LevelManager");
            }
        }

        private IEnumerator LoadAfterRegistration()
        {
            yield return null; // attend que les checkpoints enregistrent leur position dans le start
            Load();
        }

        public void RegisterCheckpoint(int index, Vector3 position)
        {
            checkpointPositions[index] = position;
        }

        public bool ChangeCurrentCheckpoint(int index)
        {
            if (index <= CurrentCheckpointIndex)
                return false;

            CurrentCheckpointIndex = index;
            return true;
        }

        public void Save()
        {
            ObjectData objectData = data.datasToSave[0];
            objectData.checkpointIndex = CurrentCheckpointIndex;
            data.datasToSave[0] = objectData;

            SaveSystem.SaveSystem.SaveData(data);
        }

        public void Load()
        {
            DataToSave loadedData = SaveSystem.SaveSystem.LoadData();

            if (loadedData.datasToSave == null || loadedData.datasToSave.Count == 0)
                return;

            data = loadedData;
            CurrentCheckpointIndex = data.datasToSave[0].checkpointIndex;

            if (!checkpointPositions.TryGetValue(CurrentCheckpointIndex, out Vector3 spawnPosition))
                return;

            if (_player is not null)
            {
                _player.transform.position = spawnPosition;
            }
        }
        
        public void ForceSetCheckpoint(int index)
        {
            CurrentCheckpointIndex = index;
        }
    }
}