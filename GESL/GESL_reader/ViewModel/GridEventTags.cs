using GESL_reader.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESL_reader.ViewModels
{
    public class GridEventTagL1 : INotifyPropertyChanged


    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool? _isChecked = false;
        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                   
                    if (event_tags_L2 != null)
                    {
                        foreach (var child in event_tags_L2)
                        {
                            child.SetIsChecked(value, true);
                        }
                    }
                }
            }
        }

        private int _id;
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }

        private string _tag;
        public string tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged(nameof(tag));
            }
        }

        public string DisplayText => $"ID: {id} - Tag: {tag}";

        private ObservableCollection<GridEventTagL2> _event_tags_L2;
        public ObservableCollection<GridEventTagL2> event_tags_L2
        {
            get => _event_tags_L2;
            set
            {
                _event_tags_L2 = value;
                OnPropertyChanged(nameof(event_tags_L2));
            }
        }

    }


    public class GridEventTagL2 : INotifyPropertyChanged


    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool? _isChecked = false;
        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                    // Update all children if any
                    if (event_tags_L3 != null)
                    {
                        foreach (var child in event_tags_L3)
                        {
                            child.SetIsChecked(value);
                        }
                    }
                }
            }
        }

        private int _id;
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }

        private string _tag;
        public string tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged(nameof(tag));
            }
        }

        public string DisplayText => $"ID: {id} - Tag: {tag}";

        private ObservableCollection<GridEventTagL3> _event_tags_L3;
        public ObservableCollection<GridEventTagL3> event_tags_L3
        {
            get => _event_tags_L3;
            set
            {
                _event_tags_L3 = value;
                OnPropertyChanged(nameof(event_tags_L3));
            }
        }

        public void SetIsChecked(bool? value, bool updateChildren)
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                if (updateChildren && event_tags_L3 != null)
                {
                    foreach (var child in event_tags_L3)
                    {
                        child.IsChecked = value; 
                    }
                }
            }
        }

    }

    public class GridEventTagL3 : INotifyPropertyChanged


    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool? _isChecked = false;
        public bool? IsChecked
        {
            get => _isChecked;
            set
            {
                SetIsChecked(value);
            }
        }


        public void SetIsChecked(bool? value)
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));

            }
        }

        private int _id;
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }

        private string _tag;
        public string tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged(nameof(tag));
            }
        }

        public string DisplayText => $"ID: {id} - Tag: {tag}";




    }

}
