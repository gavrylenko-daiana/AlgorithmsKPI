using RedBlackTreeVisualizer.ViewModels;
using System;
using System.Windows;
using RedBlackTreeVisualizer.Models;

namespace RedBlackTreeVisualizer.Views
{
    public partial class UpdateWindow : Window
    {
        private Developer _selectedDeveloper;

        public UpdateWindow(Developer selectedDeveloper)
        {
            InitializeComponent();

            _selectedDeveloper = selectedDeveloper;
            NameTextBox.Text = _selectedDeveloper.Name;
            SurnameTextBox.Text = _selectedDeveloper.Surname;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = UpdateViewModel.GetErrorFields(NameTextBox.Text, SurnameTextBox.Text);

            if (errors != string.Empty)
            {
                MessageTextBlock.Text = errors;
                return;
            }

            var originName = _selectedDeveloper.Name!;
            var originSurname = _selectedDeveloper.Surname!;

            var newName = NameTextBox.Text;
            var newSurname = SurnameTextBox.Text;

            if (originName == newName &&
                originSurname == newSurname)
            {
                MessageTextBlock.Text = "The data was not changed!";
                return;
            }

            if (MainViewModel.Developers.Contains(_selectedDeveloper) == false ||
                MainViewModel.Tree.HasId(_selectedDeveloper.Key) == false)
            {
                MessageTextBlock.Text = "Tuple cannot be modified!";
                return;
            }

            UpdateDevelopers(newName, newSurname);
            UpdateTree(newName, newSurname);

            Hide();
        }

        private void UpdateDevelopers(string newName, string newSurname)
        {
            foreach (var employee in MainViewModel.Developers)
            {
                if (employee.Key == _selectedDeveloper.Key)
                {
                    employee.Update(newName, newSurname);
                }
            }
        }

        private void UpdateTree(string newName, string newSurname)
        {
            foreach (var (key, developer) in MainViewModel.Tree)
            {
                if (key == _selectedDeveloper.Key)
                {
                    developer.Modify(newName, newSurname);
                }
            }
        }
    }
}
