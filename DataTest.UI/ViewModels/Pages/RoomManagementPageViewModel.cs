using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;

namespace DataTest.UI.ViewModels.Pages
{
    public class RoomManagementViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<RoomViewModel> _rooms;
        private string _selectedFloor;
        private string _searchText;
        private string _selectedFilter;
        private string _selectedSorting;

        public RoomManagementViewModel()
        {
            InitializeCommands();
            LoadSampleData();
        }

        public ObservableCollection<RoomViewModel> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFloor
        {
            get => _selectedFloor;
            set
            {
                _selectedFloor = value;
                OnPropertyChanged();
                FilterRooms();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterRooms();
            }
        }

        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                FilterRooms();
            }
        }

        public string SelectedSorting
        {
            get => _selectedSorting;
            set
            {
                _selectedSorting = value;
                OnPropertyChanged();
                SortRooms();
            }
        }

        public ICommand EditRoomCommand { get; private set; }
        public ICommand ChangeStatusCommand { get; private set; }

        private void InitializeCommands()
        {
            EditRoomCommand = new RelayCommand<RoomViewModel>(EditRoom);
            ChangeStatusCommand = new RelayCommand<RoomViewModel>(ChangeStatus);
        }

        private void LoadSampleData()
        {
            Rooms = new ObservableCollection<RoomViewModel>
            {
                // Kat 1
                new RoomViewModel { RoomNumber = "101", Floor = "1", RoomType = "Tek Kişilik", Status = "Müsait" },
                new RoomViewModel { RoomNumber = "102", Floor = "1", RoomType = "Tek Kişilik", Status = "Dolu" },
                new RoomViewModel { RoomNumber = "103", Floor = "1", RoomType = "Çift Kişilik", Status = "Temizleniyor" },
                new RoomViewModel { RoomNumber = "104", Floor = "1", RoomType = "Tek Kişilik", Status = "Müsait" },
                new RoomViewModel { RoomNumber = "105", Floor = "1", RoomType = "Çift Kişilik", Status = "Dolu" },
                new RoomViewModel { RoomNumber = "106", Floor = "1", RoomType = "Tek Kişilik", Status = "Bakımda" },

                // Kat 2
                new RoomViewModel { RoomNumber = "201", Floor = "2", RoomType = "Çift Kişilik", Status = "Müsait" },
                new RoomViewModel { RoomNumber = "202", Floor = "2", RoomType = "Tek Kişilik", Status = "Dolu" },
                new RoomViewModel { RoomNumber = "203", Floor = "2", RoomType = "Çift Kişilik", Status = "Müsait" },
                new RoomViewModel { RoomNumber = "204", Floor = "2", RoomType = "Tek Kişilik", Status = "Temizleniyor" },
                new RoomViewModel { RoomNumber = "205", Floor = "2", RoomType = "Çift Kişilik", Status = "Bakımda" },
                new RoomViewModel { RoomNumber = "206", Floor = "2", RoomType = "Tek Kişilik", Status = "Müsait" },

                // Kat 3
                new RoomViewModel { RoomNumber = "301", Floor = "3", RoomType = "Çift Kişilik", Status = "Dolu" },
                new RoomViewModel { RoomNumber = "302", Floor = "3", RoomType = "Tek Kişilik", Status = "Müsait" },
                new RoomViewModel { RoomNumber = "303", Floor = "3", RoomType = "Çift Kişilik", Status = "Temizleniyor" },
                new RoomViewModel { RoomNumber = "304", Floor = "3", RoomType = "Tek Kişilik", Status = "Dolu" },
                new RoomViewModel { RoomNumber = "305", Floor = "3", RoomType = "Çift Kişilik", Status = "Müsait" },
                new RoomViewModel { RoomNumber = "306", Floor = "3", RoomType = "Tek Kişilik", Status = "Bakımda" }
            };
        }

        private void FilterRooms()
        {
            // Filtreleme mantığı burada uygulanacak
        }

        private void SortRooms()
        {
            // Sıralama mantığı burada uygulanacak
        }

        private void EditRoom(RoomViewModel room)
        {
            // Oda düzenleme mantığı burada uygulanacak
        }

        private void ChangeStatus(RoomViewModel room)
        {
            // Oda durumu değiştirme mantığı burada uygulanacak
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RoomViewModel : INotifyPropertyChanged
    {
        private string _roomNumber;
        private string _floor;
        private string _roomType;
        private string _status;

        public string RoomNumber
        {
            get => _roomNumber;
            set
            {
                _roomNumber = value;
                OnPropertyChanged();
            }
        }

        public string Floor
        {
            get => _floor;
            set
            {
                _floor = value;
                OnPropertyChanged();
            }
        }

        public string RoomType
        {
            get => _roomType;
            set
            {
                _roomType = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
} 