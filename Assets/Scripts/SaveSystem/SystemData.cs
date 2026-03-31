using System.Collections.Generic;
using System;
using Datas;

[Serializable]
public struct ObjectData
{
    public List<KeyValue> arrows;
    public int checkpointIndex;
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

[Serializable]
public class KeyValue
{
    public int key;
    public string value;
}