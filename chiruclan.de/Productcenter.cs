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
using System.Reflection;
using System.IO;
using Microsoft.Win32;

namespace chiruclan_de
{
    public class Productcenter
    {
        private readonly long _productID;
        private readonly string _productVersion;
        private Cryptography _Crypt = new Cryptography();
        private Web _Web = new Web();
        private string _productUsername;
        private string _productSerial;
        private string _authHash;
        private bool _productActivated = false;
        private Dictionary<string, object> _productInfo = new Dictionary<string, object>();
        private RegistryKey lic;
        private Dictionary<string, string> Params = new Dictionary<string, string>();

        public Dictionary<string, object> Information
        {
            get { return _productInfo; }
        }

        public bool isValid
        {
            get { return _productActivated; }
        }

        public string Username
        {
            get { return _productUsername; }
            set { if (value.Length <= 32) { if (value.Length <= 32) { _productUsername = value; lic.SetValue("Username", value, RegistryValueKind.String); } } }
        }

        public string Password
        {
            set { _authHash = _Crypt.SHA1Hash(_productUsername + _Crypt.SHA512Hash(value)); lic.SetValue("AuthHash", _authHash, RegistryValueKind.String); }
        }

        public string Serial
        {
            get { return _productSerial; }
            set { if (value.Length == 29) { if (value.Length == 29) { _productSerial = value; lic.SetValue("SerialKey", value, RegistryValueKind.String); } } }
        }

        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public Productcenter(long productID, string productVersion = null)
        {
            _productID = productID;

            lic = Registry.CurrentUser.CreateSubKey("Software\\chiruclan.de\\NFAT\\" + _productID.ToString());
            if (lic.GetValue("Username") != null)
                _productUsername = lic.GetValue("Username").ToString();
            if (lic.GetValue("SerialKey") != null)
                _productSerial = lic.GetValue("SerialKey").ToString();
            if (lic.GetValue("AuthHash") != null)
                _authHash = lic.GetValue("AuthHash").ToString();

            if (productVersion == null)
            {
                if (lic.GetValue("Version") != null)
                    productVersion = lic.GetValue("Version").ToString();
                else
                    productVersion = "unknown";
            }
            else
                lic.SetValue("Version", productVersion, RegistryValueKind.String);

            _productVersion = productVersion;
        }

        /// <summary>
        /// Gets username and serial from a Licensefile
        /// </summary>
        /// <param name="licensePath">Path to a .lic-file</param>
        /// <returns>true, false</returns>
        public bool UseLicense(string licensePath)
        {
            if (!File.Exists(licensePath))
                return false;

            string licenseString = null;
            using (FileStream fs = new FileStream(licensePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                    licenseString = sr.ReadToEnd();
            }

            if (licenseString == null)
                return false;

            License licstr = new License(licenseString);
            if (licstr.KeyExists("Username") & licstr.KeyExists("Serial-Key"))
            {
                _productUsername = licstr.GetKey("Username");
                _productSerial = licstr.GetKey("Serial-Key");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the latest version string
        /// </summary>
        /// <returns>latest version string</returns>
        public string getLatestVersion()
        {
            Params.Clear();
            Params.Add("id", _productID.ToString());

            string response;
            response = _Web.postContent("http://version.products.chiruclan.de/", Params);

            return response;
        }

        /// <summary>
        /// checks whether there is a new update available or not
        /// </summary>
        /// <returns>result as boolean</returns>
        public bool newUpdateAvailable()
        {
            string response;
            response = getLatestVersion();

            if (response != _productVersion)
                return true;

            return false;
        }

        /// <summary>
        /// Get the update string if you don't know what to display to the user
        /// </summary>
        /// <returns>update string</returns>
        public string getUpdateString()
        {
            Params.Clear();
            Params.Add("id", _productID.ToString());
            Params.Add("current", _productVersion);

            string response;
            response = _Web.postContent("http://update.products.chiruclan.de/", Params);

            return response;
        }

        /// <summary>
        /// Get the latest update (as date)
        /// </summary>
        /// <returns>latest version as string</returns>
        public string getLastUpdate()
        {
            Params.Clear();
            Params.Add("id", _productID.ToString());
            Params.Add("get", "last");

            string response;
            response = _Web.postContent("http://update.products.chiruclan.de/", Params);

            return response;
        }

        /// <summary>
        /// Activation without Serial using authentication with an auth hash
        /// </summary>
        /// <returns>true, false</returns>
        private bool checkLogin()
        {
            Params.Clear();
            Params.Add("auth_hash", _authHash);
            Params.Add("app_id", _productID.ToString());

            string authHash;
            ReverseString(_authHash, out authHash);
            string correct = _Crypt.MD5Hash(authHash);
            string response;
            response = _Web.postContent("http://authenticate.products.chiruclan.de/", Params);

            if (response == correct)
                return true;

            return false;
        }

        /// <summary>
        /// activate Product using a serial key
        /// </summary>
        /// <returns>true, false</returns>
        private bool checkSerialKey()
        {
            Params.Clear();
            Params.Add("id", _productID.ToString());
            Params.Add("user", _productUsername);
            Params.Add("serial", _productSerial);

            string response;
            response = _Web.postContent("http://activate.products.chiruclan.de/", Params);

            string validHash;
            validHash = _Crypt.MD5Hash(Username + "." + Serial + "." + _productID);
            validHash = _Crypt.SHA1Hash(validHash);

            if (response == validHash)
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether product license is valid (either with Serial or AuthHash)
        /// </summary>
        public void validateProduct()
        {
            if (_authHash != null)
                _productActivated = checkLogin();
            else
                _productActivated = checkSerialKey();
        }

        /// <summary>
        /// Get information like expiration date
        /// </summary>
        public void getSerialInfo()
        {
            Params.Clear();
            Params.Add("id", _productID.ToString());
            Params.Add("user", _productUsername);
            Params.Add("var", null);

            string request = "http://serial.products.chiruclan.de/";
            string response;

            Dictionary<string, object> Info = new Dictionary<string, object>();
            Info.Add("date", "expired");
            Info.Add("timestamp", "0");

            foreach (string Key in Info.Keys.ToList<string>())
            {
                Params["var"] = Key;
                response = _Web.postContent(request, Params);
                Info[Key] = response;
            }

            _productInfo = Info;
        }

        private void ReverseString(string Input, out string Output)
        {
            char[] charArray = Input.ToCharArray();
            Array.Reverse(charArray);
            Output = new string(charArray);
        }
    }
}
