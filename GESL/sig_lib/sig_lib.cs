using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;



namespace sig_lib
{


    public class Sig_Lib
    {

        private static string url = "https://gesl.ornl.gov/api/apps/gesl";

        private static  HttpClient client = new HttpClient();

        public static string GetEventTags(string email, string apiKey, string proxyUrl = null)
        {
            string notValid = ValidateApiInput(email, apiKey);
            if (!string.IsNullOrEmpty(notValid))
            {
                return notValid;
            }

            var parameters = new
            {
                email = email,
                apikey = apiKey,
                output = "eventtags"
            };

            return RequestData(parameters, proxyUrl);
        }

        private static string RequestData(object parameters, string proxyUrl = null)
        {
            try
            {
                string jsonInputParams = JsonSerializer.Serialize(parameters, new JsonSerializerOptions { WriteIndented = true });
                var response = client.PostAsync(url, new StringContent(jsonInputParams, System.Text.Encoding.UTF8, "application/json")).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {                
                return $"Error: {ex.Message}";
            }
        }

        private static string ValidateApiInput(string email, string apiKey)
        {
            string regex = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,7}\b";
            if (!Regex.IsMatch(email, regex))
            {
                return "Invalid Email";
            }

            try
            {
                Guid.Parse(apiKey);
            }
            catch (FormatException)
            {
                return "Invalid API Key";
            }

            return string.Empty;
        }


        public static string GetEventData(string email, string apiKey, string sigId, string[] fileTypes = null, string fileSaveDirPath = "", string proxyUrl = null)
        {
            string notValid = ValidateApiInput(email, apiKey);
            if (!string.IsNullOrEmpty(notValid))
            {
                return notValid;
            }

            var parameters = new
            {
                email = email,
                apikey = apiKey,
                sigid = sigId,
                output = "data"
            };

            if (fileTypes != null)
            {
                foreach (var fileType in fileTypes.Select(ft => ft.ToLower()))
                {
                    if (fileType.Length > 4 || (fileType != "raw" && fileType != "scrubbed" && fileType != "demod" && fileType != "demod fft"))
                    {
                        return "Invalid Criteria, Unknown File Type Provided";
                    }
                }
                // Add fileTypes to parameters if valid
            }

            string dirPath = string.IsNullOrEmpty(fileSaveDirPath) ? Directory.GetCurrentDirectory() : fileSaveDirPath;
            if (!Directory.Exists(dirPath))
            {
                return "Invalid Directory Path";
            }

            string savePath = Path.Combine(dirPath, $"sigid-{sigId}.zip");

            // Implement the request_save_zip equivalent in C# to save the zip file
            RequestSaveZip(parameters, savePath, proxyUrl);

            return $"File Download Attempt Completed for: {sigId}";
        }


        private static void RequestSaveZip(object parameters, string savePath, string proxyUrl)
        {
            var jsonInputParams = JsonSerializer.Serialize(parameters);

            var content = new StringContent(jsonInputParams, System.Text.Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(proxyUrl))
            {
                var proxy = new WebProxy(proxyUrl)
                {
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy
                };

                client = new HttpClient(httpClientHandler);
            }

            
            
            var response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                using (var stream = response.Content.ReadAsStreamAsync().Result)
                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }
            else
            {
                throw new InvalidOperationException($"Request failed with status code: {response.StatusCode}");
            }
        }
    





    public static List<string> GetEventIds(
            string email, string apiKey,
            List<int>? eventTypeIds = null, List<int>? eventTagIds = null, List<string>? dataSource = null,
            List<string>? dataSite = null, List<string>? dataSensor = null,
            string eventType = "", string eventStart = "", string eventEnd = "", string sigtype = "event",
            string? proxyUrl = null)
        {
            string notValid = ValidateApiInput(email, apiKey);
            if (!string.IsNullOrEmpty(notValid))
            {
                return new List<string> { notValid };
            }

            // Check other conditions...

            var requestParams = new Dictionary<string, object?>
            {
                ["email"] = email,
                ["apikey"] = apiKey,

               //["eventtagid"] = eventTagIds  ?? new List<int>(),                 
               // ["eventtype"] = eventType,
                ["eventstart"] = eventStart,
                ["eventend"] = eventEnd,
                ["sigtype"] = sigtype,
                ["output"] = "sigids"

            };

            if (eventTagIds != null && eventTagIds.Any())
            {
                requestParams["eventtagid"] = eventTagIds;
            }


            if (dataSensor != null && dataSensor.Any())
            {
                requestParams["sensor"] = dataSensor;
            }

            if (dataSource != null && dataSource.Any())
            {
                requestParams["datasource"] = dataSource;
            }

            if (dataSite != null && dataSite.Any())
            {
                requestParams["site"] = dataSite;
            }

            if (eventTypeIds != null && eventTypeIds.Any())
            {
                requestParams["eventtypeid"] = eventTypeIds;
            }

            string jsonResponse = RequestData(requestParams, proxyUrl);

            try
            {
                
                var ids = JsonSerializer.Deserialize<List<int>>(jsonResponse);
                return ids?.Select(id => id.ToString()).ToList() ?? new List<string>();
            }
            catch (JsonException)
            {
                
                return new List<string> { "Error parsing JSON response." };
            }
        }

        
    }



}





