using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using GESL_reader.ViewModels;
using System.Text.Json;


namespace GESL_reader.ViewModels 
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            // Initialize providers with checkable state
            for (int i = 1; i <= 11; i++)
            {
                Providers.Add(new ProviderViewModel { Name = $"Provider {i}", IsSelected = false });
            }

            QProviders.Add(new ProviderViewModel { Name = "Eastern Interconnection", IsSelected = false });
            QProviders.Add(new ProviderViewModel { Name = "Western Interconnection", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "Acoustic Emission", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "Frequency Disturbance Recorder", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "None", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "Optical Sensor", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "PMU", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "PT/CT", IsSelected = false });
            Sensors.Add(new ProviderViewModel { Name = "UHF", IsSelected = false });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => Properties.Settings.Default.Email;
            set
            {
                if (Properties.Settings.Default.Email != value)
                {
                    Properties.Settings.Default.Email = value;
                    OnPropertyChanged(nameof(Email));
                    Properties.Settings.Default.Save();

                    tags = sig_lib.Sig_Lib.GetEventTags(Email, ApiKey, Proxy);
                    try
                    {
                        var myData = JsonSerializer.Deserialize<RootObject>(tags);
                        TreeData = myData;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public string ApiKey
        {
            get => Properties.Settings.Default.ApiKey;
            set
            {
                if (Properties.Settings.Default.ApiKey != value)
                {
                    Properties.Settings.Default.ApiKey = value;
                    OnPropertyChanged(nameof(ApiKey));
                    Properties.Settings.Default.Save();



                    tags = sig_lib.Sig_Lib.GetEventTags(Email, ApiKey, Proxy);
                    try
                    {
                        var myData = JsonSerializer.Deserialize<RootObject>(tags);
                        TreeData = myData;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public string Proxy
        {
            get => Properties.Settings.Default.Proxy;
            set
            {
                if (Properties.Settings.Default.Proxy != value)
                {
                    Properties.Settings.Default.Proxy = value;
                    OnPropertyChanged(nameof(Proxy));
                    Properties.Settings.Default.Save();

                    tags = sig_lib.Sig_Lib.GetEventTags(Email, ApiKey, Proxy);
                    try
                    {
                        var myData = JsonSerializer.Deserialize<RootObject>(tags);
                        TreeData = myData;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }


        private string _tags ;
        public string tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged(nameof(tags));
                }
            }
        }


        private RootObject _TreeData;
        public RootObject TreeData
        {
            get => _TreeData;
            set
            {
                if (_TreeData != value)
                {
                    _TreeData = value;
                    OnPropertyChanged(nameof(TreeData));
                }
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private ObservableCollection<string> _selectedIds = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedIds
        {
            get => _selectedIds;
            set
            {
                _selectedIds = value;
                OnPropertyChanged(nameof(SelectedIds));
            }
        }


        public List<int> GetSelectedIds()
        {
            List<int> selectedIds = new List<int>();
            if (TreeData != null && TreeData.data_quality_tags_L1 != null)
            {
                foreach (var item in TreeData.data_quality_tags_L1)
                {
                    CollectSelectedIds(item, selectedIds);
                }
            }

            if (TreeData != null && TreeData.event_tags_L1 != null)
            {
                foreach (var item in TreeData.event_tags_L1)
                {
                    CollectSelectedIds(item, selectedIds);
                }
            }

            return selectedIds;
        }

        private void CollectSelectedIds(DataQualityTagL1 tag, List<int> selectedIds)
        {
            // Check if this tag is selected
            if (tag.IsChecked == true)
            {
                selectedIds.Add(tag.id);
            }

            // Recursively check for nested tags
            if (tag.event_tags_L2 != null)
            {
                foreach (var subTag in tag.event_tags_L2)
                {
                    CollectSelectedIds(subTag, selectedIds);
                }
            }
        }

        private void CollectSelectedIds(DataQualityTagL2 tag, List<int> selectedIds)
        {
            if (tag.IsChecked == true)
            {
                selectedIds.Add(tag.id);
            }

            if (tag.event_tags_L3 != null)
            {
                foreach (var subTag in tag.event_tags_L3)
                {
                    CollectSelectedIds(subTag, selectedIds);
                }
            }
        }

        private void CollectSelectedIds(DataQualityTagL3 tag, List<int> selectedIds)
        {
            if (tag.IsChecked == true)
            {
                selectedIds.Add(tag.id);
            }
            
        }

        private void CollectSelectedIds(GridEventTagL1 tag, List<int> selectedIds)
        {
            // Check if this tag is selected
            if (tag.IsChecked == true)
            {
                selectedIds.Add(tag.id);
            }

            // Recursively check for nested tags
            if (tag.event_tags_L2 != null)
            {
                foreach (var subTag in tag.event_tags_L2)
                {
                    CollectSelectedIds(subTag, selectedIds);
                }
            }
        }

        private void CollectSelectedIds(GridEventTagL2 tag, List<int> selectedIds)
        {
            if (tag.IsChecked == true)
            {
                selectedIds.Add(tag.id);
            }

            if (tag.event_tags_L3 != null)
            {
                foreach (var subTag in tag.event_tags_L3)
                {
                    CollectSelectedIds(subTag, selectedIds);
                }
            }
        }

        private void CollectSelectedIds(GridEventTagL3 tag, List<int> selectedIds)
        {
            if (tag.IsChecked == true)
            {
                selectedIds.Add(tag.id);
            }

        }


        public ObservableCollection<int> Hours { get; } = new ObservableCollection<int>(Enumerable.Range(0, 24));
        public ObservableCollection<int> Minutes { get; } = new ObservableCollection<int>(Enumerable.Range(0, 60));

        private ObservableCollection<ProviderViewModel> _providers = new ObservableCollection<ProviderViewModel>();
        public ObservableCollection<ProviderViewModel> Providers
        {
            get => _providers;
            set
            {
                _providers = value;
                OnPropertyChanged(nameof(Providers));
            }
        }

        private ObservableCollection<ProviderViewModel> _qproviders = new ObservableCollection<ProviderViewModel>();
        public ObservableCollection<ProviderViewModel> QProviders
        {
            get => _qproviders;
            set
            {
                _qproviders = value;
                OnPropertyChanged(nameof(QProviders));
            }
        }


        private ObservableCollection<ProviderViewModel> _sensors = new ObservableCollection<ProviderViewModel>();
        public ObservableCollection<ProviderViewModel> Sensors
        {
            get => _sensors;
            set
            {
                _sensors= value;
                OnPropertyChanged(nameof(Sensors));
            }
        }



        private int _selectedHour;
        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                if (_selectedHour != value)
                {
                    _selectedHour = value;
                    OnPropertyChanged(nameof(SelectedHour));
                    UpdateDateTime();
                }
            }
        }

        private int _selectedMinute;
        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                if (_selectedMinute != value)
                {
                    _selectedMinute = value;
                    OnPropertyChanged(nameof(SelectedMinute));
                    UpdateDateTime();
                }
            }
        }

        private int _selectedHour2;
        public int SelectedHour2
        {
            get => _selectedHour2;
            set
            {
                if (_selectedHour2 != value)
                {
                    _selectedHour2 = value;
                    OnPropertyChanged(nameof(SelectedHour2));
                    UpdateDateTime2();
                }
            }
        }

        private int _selectedMinute2;
        public int SelectedMinute2
        {
            get => _selectedMinute2;
            set
            {
                if (_selectedMinute2 != value)
                {
                    _selectedMinute2 = value;
                    OnPropertyChanged(nameof(SelectedMinute2));
                    UpdateDateTime2();
                }
            }
        }

        private void UpdateDateTime()
        {
            if (SelectedDate.HasValue)
            {
                SelectedDate = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, SelectedDate.Value.Day, SelectedHour, SelectedMinute, 0);
            }
        }


        private void UpdateDateTime2()
        {
            if (SelectedDate2.HasValue)
            {
                SelectedDate2= new DateTime(SelectedDate2.Value.Year, SelectedDate2.Value.Month, SelectedDate2.Value.Day, SelectedHour2, SelectedMinute2, 0);
            }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }

        private DateTime? _selectedDate2;
        public DateTime? SelectedDate2
        {
            get => _selectedDate2;
            set
            {
                if (_selectedDate2 != value)
                {
                    _selectedDate2 = value;
                    OnPropertyChanged(nameof(SelectedDate2));
                }
            }
        }

        private bool _applyDateRange;
        public bool ApplyDateRange
        {
            get => _applyDateRange;
            set
            {
                if (_applyDateRange != value)
                {
                    _applyDateRange = value;
                    OnPropertyChanged(nameof(ApplyDateRange));
                }
            }
        }


        private string _selectedProvider;
        public string SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                if (_selectedProvider != value)
                {
                    _selectedProvider = value;
                    OnPropertyChanged(nameof(SelectedProvider));
                }
            }
        }

        public List<string> GetSelectedProviderNames()
        {
            if (IsEventSigsEnabled) { return Providers.Where(p => p.IsSelected).Select(p => p.Name).ToList(); }
            else { return QProviders.Where(p => p.IsSelected).Select(p => p.Name).ToList(); }

        }


        public List<string>? GetSelectedSensorNames()
        {
            if (IsEventSigsEnabled) { return Sensors.Where(p => p.IsSelected).Select(p => p.Name).ToList(); }
            else { return null; }

        }


        private string _csvFolder;
        public string CsvFolder
        {
            get => _csvFolder;
            set
            {
                if (_csvFolder != value)
                {
                    _csvFolder = value;
                    OnPropertyChanged(nameof(CsvFolder));
                }
            }
        }


        private string _SigIDs_txtbox;
        public string SigIDs_txtbox
        {
            get => _SigIDs_txtbox;
            set
            {
                if (_SigIDs_txtbox != value)
                {
                    _SigIDs_txtbox = value;
                    OnPropertyChanged(nameof(SigIDs_txtbox));
                }
            }
        }


        private bool _isEventSigsEnabled = true; // Default to enable Event Sigs
        private bool _isQualitySigsEnabled = false;
        //private bool _isSettingProperty = false;

        public bool IsEventSigsEnabled
        {
            get => _isEventSigsEnabled;
            set
            {
                _isEventSigsEnabled = value;
                OnPropertyChanged(nameof(IsEventSigsEnabled));
            }
        }

        public bool IsQualitySigsEnabled
        {
            get => _isQualitySigsEnabled;
        
           set
            {
                _isQualitySigsEnabled = value;
                OnPropertyChanged(nameof(IsQualitySigsEnabled));
            }
        }

        


    }




    public class RootObject
    {
        public ObservableCollection<DataQualityTagL1> data_quality_tags_L1 { get; set; }
        public ObservableCollection<GridEventTagL1> event_tags_L1 { get; set; }


    }



    public class ProviderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
