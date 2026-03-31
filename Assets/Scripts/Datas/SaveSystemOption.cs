using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = "SaveSystemOption", menuName = "Scriptable Objects/SaveSystemOption")]
    public class SaveSystemOption : ScriptableObject
    {
        public SaveType saveType;
        public string saveFolderName;
        public string saveExtensionName;
    }
}
