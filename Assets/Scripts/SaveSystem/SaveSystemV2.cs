using System.IO;
using UnityEngine;

public class SaveSystemV2
{
   public static string folderName;
   public static string extensionName;
   public static string stringSaveType;
   
   public SaveSystemV2(SystemOption systemOption)
   {
      folderName = systemOption.SaveFolderName;
      extensionName = systemOption.SaveExtentionName;
      if (systemOption.SaveType == SaveType.PersistentDataPath)
      {
         stringSaveType = Application.persistentDataPath;
      }
      else
      {
         stringSaveType = Application.dataPath;
      }
   }
   
   public static void SaveData(object _currentDataToSave, string _fileName = "DefaultSave")
   {
      if (_currentDataToSave == null)
      {
         return;
      }
      
      string folderPath = stringSaveType + "/" + folderName;
      string filePath = folderPath + "/" + _fileName +"."+  extensionName;
      if (!Directory.Exists(folderPath))
      {
         Directory.CreateDirectory(folderPath);
      }

      if (SaveManagerV2.SINGLETON && SaveManagerV2.SINGLETON.EncryptData)
      {
         string json = EncryptionUtility.EncryptString( JsonUtility.ToJson(_currentDataToSave));
         File.WriteAllText(filePath, json);
      }
      else
      {
         string json = JsonUtility.ToJson(_currentDataToSave);
         File.WriteAllText(filePath, json);
      }
   }

   public static DataToSave LoadData(string saveName = "DefaultSave")
   {
      DataToSave emptyData = new DataToSave();
      if (saveName == string.Empty)
         return emptyData;
            
      string folderPath = stringSaveType + "/" + folderName;
      string filePath = folderPath + "/" + saveName + "." + extensionName;
            
      Debug.Log(filePath);
            
      if (!Directory.Exists(folderPath))
      {
         Directory.CreateDirectory(folderPath);
      }

      if (!File.Exists(filePath))
      {
         Debug.LogError("No save file found !");
         return emptyData;
      }
      else
      {
         string savedContent = File.ReadAllText(filePath);
         if (EncryptionUtility.IsEncrypted(savedContent))
         {
            savedContent = EncryptionUtility.DecryptString(savedContent);
         }
         return JsonUtility.FromJson<DataToSave>(savedContent);
      }
   }
   
   public static void DeleteSave(string saveName = "DefaultSave")
   {
      string folderPath = stringSaveType + "/" + folderName;
      string filePath = folderPath + "/" + saveName + "." + extensionName;
      File.Delete(filePath);
   }
}
