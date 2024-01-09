using System.ComponentModel;

namespace RedBlackTreeVisualizer.ViewModels
{
    public class Developer : INotifyPropertyChanged
    {
        private int _key;
        private string? _name;
        private string? _surname;

        public int Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged(nameof(Key));
                }
            }
        }

        public string? Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string? Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Developer(int key, string name, string surname)
        {
            Key = key;
            Name = name;
            Surname = surname;
        }

        public void Update(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}