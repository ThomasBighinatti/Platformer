using System.Collections.Generic;
using UnityEngine;
using System;
using Datas;

[Serializable]
public struct ObjectData
{
    public List<KeyValue> arrows;
    //POUR AJOUTER DE LA DATA A SAVE C4EST ICI
    public NumCheckPoint checkpoint;
}

[Serializable]
public enum SaveType
{
    PersistentDataPath,
    ProjectFolder
}

[Serializable]
public struct DataToSave
{
    public List<ObjectData> DatasToSave;

}

//SI ON VEUT AJOUTER DE LA DATA + COMPLEXE ON PEUT CREER DES STRUCTS/ENUM/CLASS ICI

[Serializable]
public class KeyValue
{
    public int key;
    public string value;
}

