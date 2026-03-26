using System;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManagerV2 : MonoBehaviour
{
    public static SaveManagerV2 SINGLETON;
    public SystemOption systemOption;
    public NumCheckPoint currentCheckpoint = NumCheckPoint.Checkpoint0;
    
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
        
        ObjectData test = new ObjectData();
        test.arrows = new List<KeyValue>();
        KeyValue currentKey = new KeyValue();
        currentKey.key = 0;
        currentKey.value = "dirt";
        test.arrows.Add(currentKey);
        
        Data.DatasToSave.Add(test);
    }

    public void ChangeCurrentCheckpoint()
    {
        currentCheckpoint = NumCheckPoint.Checkpoint1;
    }
}
