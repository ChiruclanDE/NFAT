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
using System.Net;
using System.IO;
using System.Collections;

namespace chiruclan_de
{
    class Web
    {
        /// <summary>
        /// Get content from Web
        /// </summary>
        /// <param name="remotePath">The url of the page where to get the content from</param>
        /// <param name="length">The length of the response</param>
        /// <returns>Response from the website of the url</returns>
        public string getContent(string remotePath, long length = 1024)
        {
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                StringBuilder respBody = new StringBuilder();

                request = (HttpWebRequest)WebRequest.Create(remotePath);
                response = (HttpWebResponse)request.GetResponse();

                byte[] buffer = new byte[length];

                Stream respStream = response.GetResponseStream();

                int count = 0;

                do
                {
                    count = respStream.Read(buffer, 0, buffer.Length);

                    if (count != 0)
                        respBody.Append(Encoding.ASCII.GetString(buffer, 0, count));
                } while (count > 0);


                return respBody.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public string postContent(string remotePath, Dictionary<string, string> Parameters, long length = 1024)
        {
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                StringBuilder respBody = new StringBuilder();

                request = (HttpWebRequest)WebRequest.Create(remotePath);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                string parameterString = "";

                foreach (string parameter in Parameters.Keys)
                    parameterString += parameter + "=" + Parameters[parameter] + "&";

                byte[] buffer = new byte[length];
                byte[] byteArray = Encoding.UTF8.GetBytes(parameterString);
                request.ContentLength = byteArray.Length;

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(byteArray, 0, byteArray.Length);
                reqStream.Close();

                response = (HttpWebResponse)request.GetResponse();
                Stream respStream = response.GetResponseStream();

                int count = 0;

                do
                {
                    count = respStream.Read(buffer, 0, buffer.Length);

                    if (count != 0)
                        respBody.Append(Encoding.ASCII.GetString(buffer, 0, count));
                } while (count > 0);


                return respBody.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
    }
}
