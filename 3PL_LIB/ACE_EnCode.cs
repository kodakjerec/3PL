using System;
using System.Text;
using System.Security.Cryptography;

namespace _3PL_LIB
{
    public class ACE_EnCode
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">加密文字</param>
        /// <param name="key">加密金鑰</param>
        /// <returns></returns>
        public string AESEn(string EnText, string key)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] plainTextData = Encoding.Unicode.GetBytes(EnText);
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            ICryptoTransform transform = AES.CreateEncryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(plainTextData, 0, plainTextData.Length);
            return Convert.ToBase64String(outputData);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="DcText">解密文字</param>
        /// <param name="key">加密金鑰</param>
        /// <returns></returns>
        public string AESDc(string DcText, string key)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            ICryptoTransform transform = AES.CreateDecryptor(keyData, IVData);
            byte[] encryptedData = Convert.FromBase64String(DcText);
            byte[] outputData = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.Unicode.GetString(outputData);
        }
    }
}
