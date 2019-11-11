using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class Utils
    {
        #region 随机数生成
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <returns></returns>
        public static string GetRandom(int length)
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_"; //自己定义符号
            string r = string.Empty;
            Random random = new Random();
            Enumerable.Repeat<int>(0, length).ToList().ForEach(x => r += s[random.Next(s.Length)]);
            return r;
        }

        /// <summary>
        /// 随机生成字符串数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomNum(int length)
        {
            var num = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                num += rd.Next(0, 10).ToString();
            }
            return num;
        }
        #endregion
        #region MD5加密
        public static string MD5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }
        #endregion

        #region RSA加密解密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="pub"></param>
        /// <returns></returns>
        public static string RSAEncryptCore(string plain, string pub)
        {
            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.FromXmlStr(UTF8Encoding.UTF8.GetString(Convert.FromBase64String(pub)));
                    var bufferSize = (rsa.KeySize / 8 - 11);
                    byte[] buffer = new byte[bufferSize];//待加密块
                    var plainbytes = UTF8Encoding.UTF8.GetBytes(plain);

                    using (MemoryStream msInput = new MemoryStream(plainbytes))
                    {
                        using (MemoryStream msOutput = new MemoryStream())
                        {
                            int readLen;
                            while ((readLen = msInput.Read(buffer, 0, bufferSize)) > 0)
                            {
                                byte[] dataToEnc = new byte[readLen];
                                Array.Copy(buffer, 0, dataToEnc, 0, readLen);
                                byte[] encData = rsa.Encrypt(dataToEnc, RSAEncryptionPadding.Pkcs1);
                                msOutput.Write(encData, 0, encData.Length);
                            }

                            byte[] result = msOutput.ToArray();
                            rsa.Clear();
                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptContent"></param>
        /// <param name="prv"></param>
        /// <returns></returns>
        public static string RSADecryptCore(string encryptContent, string prv)
        {
            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.FromXmlStr(UTF8Encoding.UTF8.GetString(Convert.FromBase64String(prv)));
                    int keySize = rsa.KeySize / 8;
                    byte[] buffer = new byte[keySize];
                    var cipherbytes = Convert.FromBase64String(encryptContent);
                    using (MemoryStream msInput = new MemoryStream(cipherbytes))
                    {
                        using (MemoryStream msOutput = new MemoryStream())
                        {
                            int readLen;

                            while ((readLen = msInput.Read(buffer, 0, keySize)) > 0)
                            {
                                byte[] dataToDec = new byte[readLen];
                                Array.Copy(buffer, 0, dataToDec, 0, readLen);
                                byte[] decData = rsa.Decrypt(dataToDec, RSAEncryptionPadding.Pkcs1);
                                msOutput.Write(decData, 0, decData.Length);
                            }

                            byte[] result = msOutput.ToArray();
                            rsa.Clear();

                            return Encoding.UTF8.GetString(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        /*
         加密算法
         AES
         RSA
         CRC
         MD5
         */
    }
}
