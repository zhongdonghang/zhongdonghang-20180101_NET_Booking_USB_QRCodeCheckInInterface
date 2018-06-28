using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PosObject
{
    public class AESAlgorithm
    {
        //默认密钥向量    
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static string key = "SeatManage_WeiCharCode";
        /// <summary>      
        /// AES加密算法   
        /// </summary>   
        /// <param name="plainText">明文字符串</param>   
        /// <param name="strKey">密钥</param>   
        /// <returns>返回加密后的密文字节数组</returns>   
        public static string AESEncrypt(string plainText)
        {
            return AESEncrypt(plainText, key);
        }
        /// <summary>      
        /// AES加密算法   
        /// </summary>   
        /// <param name="plainText">明文字符串</param>   
        /// <param name="strKey">密钥</param>   
        /// <returns>返回加密后的密文字节数组</returns>   
        public static string AESEncrypt(string plainText, string strKey)
        {        //分组加密算法           
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组       
            //设置密钥及密钥向量   
            des.Key = Encoding.UTF8.GetBytes(strKey.Substring(0, 16));
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组   
            cs.Close();
            ms.Close();
            string strEncrypt = Convert.ToBase64String(cipherBytes);
            return strEncrypt;

        }
        /// <summary>   
        /// AES解密   
        /// </summary>   
        /// <param name="cipherText">密文字节数组</param>   
        /// <param name="strKey">密钥</param>   
        /// <returns>返回解密后的字符串</returns>   
        public static string AESDecrypt(string plainText)
        {
            return AESDecrypt(plainText, key);
        }


        /// <summary>   
        /// AES解密   
        /// </summary>   
        /// <param name="cipherText">密文字节数组</param>   
        /// <param name="strKey">密钥</param>   
        /// <returns>返回解密后的字符串</returns>   
        public static string AESDecrypt(string plainText, string strKey)
        {
            try
            {
                byte[] cipherText = Convert.FromBase64String(plainText);
                SymmetricAlgorithm des = Rijndael.Create();
                des.Key = Encoding.UTF8.GetBytes(strKey.Substring(0, 16));
                des.IV = _key1;
                byte[] decryptBytes = new byte[cipherText.Length];
                MemoryStream ms = new MemoryStream(cipherText);
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                cs.Read(decryptBytes, 0, decryptBytes.Length);
                cs.Close();
                ms.Close();
                string strDecrypt = Encoding.UTF8.GetString(decryptBytes);
                return strDecrypt.Replace("\0", "");
            }
            catch (Exception ex)
            {
                return "";
            }
        }


    }
}
