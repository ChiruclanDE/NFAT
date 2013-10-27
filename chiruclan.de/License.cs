/* 
 * == DotNetLibrary from Chiruclan - Simple steps for learning C# ==
 * Copyright (C) 2013  Chiruclan
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
