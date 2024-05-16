using siglib_demo;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;



namespace sig_lib
{
    class Siglib_demo
    {
        static void Main(string[] args)
        {
            
            string email = siglib_config.email;
            string apikey = siglib_config.apikey;
            string proxy = siglib_config.proxy;

            // Get Event Tags
            string AllTags = Sig_Lib.GetEventTags(email, apikey, proxy);

            Console.WriteLine("Event Tags:");
            Console.WriteLine(AllTags);

            // find tags
            //var tagsToFind = new List<string> { "Generator", "Transmission", "On" };
            //var requiredAncestorTags = new HashSet<string> { "Equipment" };

            




            // For Event IDs - Example usage

            List<int> eventTagIds = new List<int> { 311, 242 }; // Example event tag IDs
            List<String> data_source = new List<string> {"Provider 9"};
            string eventStart = "2017-01-01 00:00:00"; 
            string eventEnd = "2017-02-28 23:23:59"; 

            List<string> sig_ids = Sig_Lib.GetEventIds(email, apikey, null, eventTagIds, data_source, null, null, null, eventStart, eventEnd,"event", proxy);

            Console.WriteLine("ID results:");
            Console.WriteLine(string.Join(Environment.NewLine, sig_ids));


            // Get the current working directory
            string currentFolder = Directory.GetCurrentDirectory();
            string zipFolder = Path.Combine(currentFolder, "zip");
            string csvFolder = Path.Combine(currentFolder, "csv");
            Console.WriteLine("ZIP Folder Path: " + zipFolder);

            // Check if the zip directory exists, create it if not
            if (!Directory.Exists(zipFolder))
            {
                Directory.CreateDirectory(zipFolder);
                Console.WriteLine("Created directory: " + zipFolder);
            }
            // Check if the csv directory exists, create it if not
            if (!Directory.Exists(csvFolder))
            {
                Directory.CreateDirectory(csvFolder);
                Console.WriteLine($"Created directory: {csvFolder}");
            }

            foreach (var sigId in sig_ids)
            {
                string filedownload_result = Sig_Lib.GetEventData(email, apikey, sigId,null, zipFolder, proxy);
                Console.WriteLine(filedownload_result);
            }


            // List all the ZIP files in the directory
            var zipFiles = Directory.GetFiles(zipFolder, "*.zip");

            // Iterate over the list of ZIP files and extract them into csv folder 
            foreach (var zipFile in zipFiles)
            {
                using (ZipArchive zip = ZipFile.OpenRead(zipFile))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        // Extract only CSV files
                        if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            string destinationPath = Path.Combine(csvFolder, entry.FullName);
                            entry.ExtractToFile(destinationPath, overwrite: true); 
                        }
                    }
                }
                Console.WriteLine($"Extracted {Path.GetFileName(zipFile)}");
            }

        }
    }
}
