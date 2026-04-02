using System;
using System.IO;
using Datas;
using Managers;
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
         _stringSaveType = saveSystemOption.saveType switch
         {
            SaveType.ProjectFolder => Application.dataPath,
            SaveType.PersistentDataPath => Application.persistentDataPath,
            _ => throw new ArgumentOutOfRangeException()
         };
      }
   
      public static void SaveData(object currentDataToSave, string fileName = "DefaultSave")
      {
         if (fileName == string.Empty)
            return;
            
         if (currentDataToSave == null)
            return;
      
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
