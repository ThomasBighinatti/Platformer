using UnityEngine;

[CreateAssetMenu(fileName = "SystemOption", menuName = "Scriptable Objects/SystemOption")]
public class SystemOption : ScriptableObject
{
    public SaveType SaveType;
    public string SaveFolderName;
    public string SaveExtentionName;
}
