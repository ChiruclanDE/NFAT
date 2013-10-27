using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;

namespace chiruclan_de
{
    class Cryptography
    {
        /// <summary>
        /// Hash as md5
        /// </summary>
        /// <param name="StringToHash">String to hash</param>
        /// <returns>Hash of the string</returns>
        public string MD5Hash(string StringToHash)
        {
            if (string.IsNullOrEmpty(StringToHash))
                return string.Empty;

            MD5 CryptoService = new MD5CryptoServiceProvider();

            byte[] BytesToHash = Encoding.Default.GetBytes(StringToHash);
            byte[] HashResult = CryptoService.ComputeHash(BytesToHash);

            StringBuilder Result = new StringBuilder();

            foreach (byte b in HashResult)
                Result.Append(b.ToString("x2"));

            return Result.ToString();
        }

        /// <summary>
        /// Hash as sha1
        /// </summary>
        /// <param name="StringToHash">String to hash</param>
        /// <returns>Hash of the string</returns>
        public string SHA1Hash(string StringToHash)
        {
            if (string.IsNullOrEmpty(StringToHash))
                return string.Empty;

            SHA1 CryptoService = new SHA1CryptoServiceProvider();

            byte[] BytesToHash = Encoding.Default.GetBytes(StringToHash);
            byte[] HashResult = CryptoService.ComputeHash(BytesToHash);

            StringBuilder Result = new StringBuilder();

            foreach (byte b in HashResult)
                Result.Append(b.ToString("x2"));

            return Result.ToString();
        }

        public string SHA512Hash(string StringToHash)
        {
            if (string.IsNullOrEmpty(StringToHash))
                return string.Empty;

            SHA512 CryptoService = new SHA512CryptoServiceProvider();

            byte[] BytesToHash = Encoding.Default.GetBytes(StringToHash);
            byte[] HashResult = CryptoService.ComputeHash(BytesToHash);

            StringBuilder Result = new StringBuilder();

            foreach (byte b in HashResult)
                Result.Append(b.ToString("x2"));

            return Result.ToString();
        }
    }
}
