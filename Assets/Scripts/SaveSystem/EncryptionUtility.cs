using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SaveSystem
{
   public class EncryptionUtility
   {
      private const string EncryptionKey = "7e8f9a2b4c6d8e0f1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f";
      
      public static string EncryptString(string plainText)
      {
         byte[] key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 32)); // Ensure key is 32 bytes long for AES256
         using (Aes aes = Aes.Create())
         {
            aes.Key = key;
            aes.GenerateIV();
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var msEncrypt = new MemoryStream())
            {
               msEncrypt.Write(aes.IV, 0, aes.IV.Length);
               using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
               using (var swEncrypt = new StreamWriter(csEncrypt))
               {
                  swEncrypt.Write(plainText);
               }

               return "TBH$" + Convert.ToBase64String(msEncrypt.ToArray());
            }
         }
      }

      public static string DecryptString(string saveText)
      {
         if (IsEncrypted(saveText))
            saveText = saveText.Substring(4);

         byte[] fullCipher = Convert.FromBase64String(saveText);
         byte[] iv = new byte[16];
         byte[] cipher = new byte[fullCipher.Length - 16];

         Array.Copy(fullCipher, iv, iv.Length);
         Array.Copy(fullCipher, 16, cipher, 0, cipher.Length);

         byte[] key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 32));
         using (Aes aesAlg = Aes.Create())
         {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipher))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
               return srDecrypt.ReadToEnd(); // Return the decrypted text
            }
         }
      }
   
      public static bool IsEncrypted(string inputFile)
      {
         return inputFile.StartsWith("TBH$");
      }
   }
}