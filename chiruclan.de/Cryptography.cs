/* Copyright 2013 Pascal Vahlberg
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
