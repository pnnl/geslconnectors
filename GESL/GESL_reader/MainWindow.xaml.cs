using GESL_reader.ViewModels;
using System.Diagnostics.Tracing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GESL_reader.ViewModels;
using System.Text.Json;
using sig_lib;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Xml.Linq;




namespace GESL_reader
{




    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; set; }

        public MainWindow()
        {

            ViewModel = new ViewModel();
            this.DataContext = ViewModel;

            ViewModel.SelectedDate2 = DateTime.Now;
            ViewModel.SelectedDate = new DateTime(2017, 1, 1);

            ViewModel.CsvFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "csv");

            ViewModel.tags = sig_lib.Sig_Lib.GetEventTags(ViewModel.Email, ViewModel.ApiKey, ViewModel.Proxy);
            try
            {
                var myData = JsonSerializer.Deserialize<RootObject>(ViewModel.tags);
                ViewModel.TreeData = myData;
            }
            catch (Exception ex)
            {

            }
        }



        private void Find_IDs_Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> sig_ids;
            if (!string.IsNullOrWhiteSpace(ViewModel.SigIDs_txtbox))
            {
                sig_ids = ViewModel.SigIDs_txtbox.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(id => id.Trim())  // Ensure to trim spaces
                                             .ToList();
            }
            else
            {
                List<int> eventTagIds = ViewModel.GetSelectedIds();



                List<String> data_source = ViewModel.GetSelectedProviderNames();
                List<String>? sensor = ViewModel.GetSelectedSensorNames();


                string eventStart = "";
                string eventEnd = "";
                if (ViewModel.ApplyDateRange & ViewModel.IsEventSigsEnabled)
                {
                    eventStart = ViewModel.SelectedDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Date Not Set";
                    eventEnd = ViewModel.SelectedDate2?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Date Not Set";
                }

                string sigtype;
                if (ViewModel.IsEventSigsEnabled) { sigtype = "event"; } else { sigtype = "data quality"; }

                sig_ids = Sig_Lib.GetEventIds(ViewModel.Email, ViewModel.ApiKey, null, eventTagIds, data_source, null, sensor, null, eventStart, eventEnd, sigtype, ViewModel.Proxy);
            }
            ViewModel.SelectedIds.Clear();
            foreach (var id in sig_ids)
            {
                ViewModel.SelectedIds.Add(id);
            }
        }

        private void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the current working directory
            string currentFolder = Directory.GetCurrentDirectory();
            string zipFolder = System.IO.Path.Combine(currentFolder, "zip");
            string csvFolder = ViewModel.CsvFolder; //System.IO.Path.Combine(currentFolder, "csv");
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            // Check if the zip directory exists, create it if not
            if (!Directory.Exists(zipFolder))
            {
                Directory.CreateDirectory(zipFolder);
            }
            else
            {
                string[] files = Directory.GetFiles(zipFolder);

                foreach (string file in files)
                {
                    try
                    {
                        // Delete each file
                        File.Delete(file);
                    }
                    catch { }                    
                }
            }

            // Check if the csv directory exists, create it if not
            if (!Directory.Exists(csvFolder))
            {
                Directory.CreateDirectory(csvFolder);
            }
            foreach (var sigId in ViewModel.SelectedIds)
            {
                try
                {
                    string filedownload_result = Sig_Lib.GetEventData(ViewModel.Email, ViewModel.ApiKey, sigId, null, zipFolder, ViewModel.Proxy);
                }
                catch (Exception ex)
                { System.Windows.MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);}
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
                            string destinationPath = System.IO.Path.Combine(csvFolder, entry.FullName);
                            entry.ExtractToFile(destinationPath, overwrite: true);
                        }
                    }
                }
            }
            Mouse.OverrideCursor = null;
        }

        private void CSVPathButton_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = Environment.CurrentDirectory;
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    ViewModel.CsvFolder = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void AW_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "MyPresetFile";  // Default file name
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();  

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveToXmlFile(saveFileDialog.FileName);
            }
        }


        public void SaveToXmlFile(string filePath)
        {
            var presets = new XElement("Presets");

            foreach (var id in ViewModel.SelectedIds)
            {
                var presetElement = new XElement("Preset",
                    new XAttribute("name", id),
                    new XElement("Signature",
                        new XAttribute("Email", ViewModel.Email),
                        new XAttribute("APIkey", ViewModel.ApiKey),
                        new XAttribute("TempFileLocation", ViewModel.CsvFolder),
                        "" // This forces the XML serializer to format it with an opening and a closing tag. AW requires it.
                    )
                );
                presets.Add(presetElement);
            }

            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                presets);

            doc.Save(filePath);

        }


    }
}