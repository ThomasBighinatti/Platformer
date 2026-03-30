using System;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManagerV2 : MonoBehaviour
{
    public static SaveManagerV2 SINGLETON;
    public SystemOption systemOption;
    private NumCheckPoint _currentCheckpoint = NumCheckPoint.Checkpoint0;
    
    [Header("Settings")]
    public bool EncryptData = false;
    
    public DataToSave Data;
    public void Awake()
    {
        if (SINGLETON == null)
        {
            SINGLETON = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SaveSystemV2 save = new SaveSystemV2(systemOption);
        
        ObjectData data1 = new ObjectData();
        data1.arrows = new List<KeyValue>();
        KeyValue currentKey = new KeyValue();
        currentKey.key = 0;
        currentKey.value = "dirt";
        data1.arrows.Add(currentKey);
        
        
        Data.DatasToSave.Add(data1);
    }

    public void ChangeCurrentCheckpoint(NumCheckPoint checkpoint)
    {
        if (checkpoint > _currentCheckpoint)
            _currentCheckpoint = checkpoint;
    }

    public void Update()
    {
        //print(_currentCheckpoint);
    }
}
