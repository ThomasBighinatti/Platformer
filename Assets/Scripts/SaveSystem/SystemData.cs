using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public struct ObjectData
    {
        public List<KeyValue> arrows;
        public int checkpointIndex;
    }

    [Serializable]
    public struct DataToSave
    {
        public List<ObjectData> datasToSave;
    }

    [Serializable]
    public struct KeyValue
    {
        public int key;
        public string value;
    }
    
}