using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Moz.Exceptions;
using Moz.Service.Security;

namespace Moz.Bus.Services.Security
{
    public class EncryptionService : IEncryptionService
    {
        #region Fields

        

        #endregion

        #region Ctor

        /// <summary>
        ///     Ctor
        /// </summary>
        public EncryptionService()
        {
            
        }

        #endregion

        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
                    CryptoStreamMode.Write))
                {
                    var toEncrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv),
                    CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual string CreateSaltKey(int size)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltKey"></param>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        public virtual string CreatePasswordHash(string password, string saltKey, string passwordFormat)
        {
            return CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltKey)), passwordFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashAlgorithm"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual string CreateHash(byte[] data, string hashAlgorithm)
        {
            if (string.IsNullOrEmpty(hashAlgorithm))
                throw new ArgumentNullException(nameof(hashAlgorithm));

            byte[] bytes;
            switch (hashAlgorithm.ToLower())
            {
                case "md5":
                {
                    var sha = MD5.Create();
                    bytes = sha.ComputeHash(data);
                    break;
                }
                case "sha1":
                {
                    var sha = SHA1.Create();
                    bytes = sha.ComputeHash(data);
                    break;
                }
                case "sha256":
                {
                    var sha = SHA256.Create();
                    bytes = sha.ComputeHash(data);
                    break;
                }
                case "sha384":
                {
                    var sha = SHA384.Create();
                    bytes = sha.ComputeHash(data);
                    break;
                }
                case "sha512":
                {
                    var sha = SHA512.Create();
                    bytes = sha.ComputeHash(data);
                    break;
                }
                default:
                {
                    var sha = SHA512.Create();
                    bytes = sha.ComputeHash(data);
                    break;
                }
            }

            return BitConverter.ToString(bytes).Replace("-", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="encryptionPrivateKey"></param>
        /// <returns></returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                throw new AlertException("加密Key不能为空");

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                var encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
                return Convert.ToBase64String(encryptedBinary);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="encryptionPrivateKey"></param>
        /// <returns></returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                throw new AlertException("加密Key不能为空");

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                var buffer = Convert.FromBase64String(cipherText);
                return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
            }
        }

        #endregion
    }
}