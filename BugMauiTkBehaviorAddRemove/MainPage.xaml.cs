using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BugMauiTkBehaviorAddRemove
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void ChangeNotify([CallerMemberName] string? prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                ChangeNotify(propertyName);
                return true;
            }
            return false;
        }
    }
    public class CollectionVM : BaseVM
    {
        public string? Pressed
        {
            get => _pressed;
            set => SetProperty(ref _pressed, value);
        }
        public ObservableCollection<ItemVM> Items { get; } = [];
        public ICommand CmdRefresh { get; }

        public CollectionVM()
        {
            CmdRefresh = new Command(OnRefresh);
        }
        string? _pressed;
        void OnRefresh()
        {
            Items.Clear();
            for (int i = 0; i < 3; i++)
                Items.Add(new ItemVM(this));
        }
    }

    public class ItemVM : BaseVM
    {
        public string Data { get; }
        public Command CmdPress { get; }
        public ItemVM(CollectionVM owner)
        {
            _owner = owner;
            Data = _data.ToString();
            CmdPress = new Command(OnPress);
            _data++;
        }
        CollectionVM _owner;
        static int _data = 0;
        void OnPress()
        {
            _owner.Pressed = Data;
        }
    }

    public partial class MainPage : ContentPage
    {
        CollectionVM _vm;
        public MainPage()
        {
            InitializeComponent();
            _vm = new CollectionVM();
            BindingContext = _vm;
        }
    }

}
