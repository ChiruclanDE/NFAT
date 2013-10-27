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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace chiruclan_de
{
    /// <summary>
    /// A simple class for using configuration files
    /// </summary>
    public class License
    {
        private SortedDictionary<string, string> KeyList = new SortedDictionary<string, string>();

        public License(string licenseString)
        {
            licenseString = Encoding.UTF8.GetString(Convert.FromBase64String(licenseString));
            Read(licenseString);
        }

        /// <summary>
        /// Gets the data from the configuration file
        /// </summary>
        public void Read(string licenseString)
        {
            try
            {
                string[] Current = null;
                string CurrentKey = null;
                string CurrentValue = null;

                foreach (string CurrentLine in licenseString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(CurrentLine) & !string.IsNullOrWhiteSpace(CurrentLine) & CurrentLine.Contains("="))
                    {
                        Current = CurrentLine.Split(new char[] { '=' }, 2);

                        CurrentKey = Current[0].Trim();
                        CurrentValue = Current[1].Trim();

                        KeyList.Add(CurrentKey, CurrentValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Checks wether a key exists or not
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool KeyExists(string Key)
        {
            if (KeyList.ContainsKey(Key))
                return true;

            return false;
        }

        /// <summary>
        /// Gets the value of a key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetKey(string Key)
        {
            if (KeyList.ContainsKey(Key))
                return KeyList[Key];

            return null;
        }
    }
}
