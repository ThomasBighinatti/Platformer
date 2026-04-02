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
        
        public SaveSystemOption saveSystemOption;
        private int _currentCheckpointIndex = 0;

        private readonly Dictionary<int, Vector3> _checkpointPositions = new Dictionary<int, Vector3>();

        [Header("Settings")]
        public bool encryptData = false;


        [SerializeField] private GameObject player;
        public DataToSave data;

        public void Awake()
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
            ObjectData objectData = data.datasToSave[0];
            objectData.checkpointIndex = _currentCheckpointIndex;
            data.datasToSave[0] = objectData;

            SaveSystem.SaveSystem.SaveData(data);
        }

        private void Load()
        {
            DataToSave loadedData = SaveSystem.SaveSystem.LoadData();

            if (loadedData.datasToSave == null || loadedData.datasToSave.Count == 0)
                return;

            data = loadedData;
            _currentCheckpointIndex = data.datasToSave[0].checkpointIndex;

            if (!_checkpointPositions.TryGetValue(_currentCheckpointIndex, out Vector3 spawnPosition))
                return;
            
            if (player is not null)
                player.transform.position = spawnPosition;
        }
    }
}