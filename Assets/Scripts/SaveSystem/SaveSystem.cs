using System.IO;
using Datas;
using UnityEngine;

namespace SaveSystem
{
   public class SaveSystem
   {
      private static string _folderName;
      private static string _extensionName;
      private static string _stringSaveType;
   
      public SaveSystem(SaveSystemOption saveSystemOption)
      {
         _folderName = saveSystemOption.saveFolderName;
         _extensionName = saveSystemOption.saveExtensionName;
         if (saveSystemOption.saveType == SaveType.PersistentDataPath)
         {
            _stringSaveType = Application.persistentDataPath;
         }
         else
         {
            _stringSaveType = Application.dataPath;
         }
      }
   
      public static void SaveData(object currentDataToSave, string fileName = "DefaultSave")
      {
         if (currentDataToSave == null)
         {
            return;
         }
      
         string folderPath = _stringSaveType + "/" + _folderName;
         string filePath = folderPath + "/" + fileName +"."+  _extensionName;
         if (!Directory.Exists(folderPath))
         {
            Directory.CreateDirectory(folderPath);
         }

         if (SaveManager.Instance && SaveManager.Instance.encryptData)
         {
            string json = EncryptionUtility.EncryptString( JsonUtility.ToJson(currentDataToSave));
            File.WriteAllText(filePath, json);
         }
         else
         {
            string json = JsonUtility.ToJson(currentDataToSave);
            File.WriteAllText(filePath, json);
         }
      }

      public static DataToSave LoadData(string saveName = "DefaultSave")
      {
         DataToSave emptyData = new DataToSave();
         if (saveName == string.Empty)
            return emptyData;
            
         string folderPath = _stringSaveType + "/" + _folderName;
         string filePath = folderPath + "/" + saveName + "." + _extensionName;
            
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
         string folderPath = _stringSaveType + "/" + _folderName;
         string filePath = folderPath + "/" + saveName + "." + _extensionName;
         File.Delete(filePath);
      }
   }
}
