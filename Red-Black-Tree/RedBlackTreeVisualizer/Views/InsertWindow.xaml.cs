using RedBlackTreeVisualizer.ViewModels;
using RedBlackTreeVisualizer.Models;
using System.Windows;
using System;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RedBlackTreeVisualizer.Views
{
    public partial class InsertWindow : Window
    {
        public InsertWindow()
        {
            InitializeComponent();
        }

        private void AddCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = AddViewModel.GetErrorFields(KeyInput.Text, NameInput.Text, SurnameInput.Text);

            if (errors != string.Empty)
            {
                MessageTextBlock.Text = errors;
                return;
            }

            var key = Convert.ToInt32(KeyInput.Text);
            var name = NameInput.Text;
            var surname = SurnameInput.Text;

            if (MainViewModel.Developers.GetKeyCount(key) == 0)
            {
                MainViewModel.Developers!.Add(new Developer(key, name, surname));
                MainViewModel.Tree!.Add(key, new SoftwareEngineer(name, surname));
                Close();
            }
            else
            {
                MessageTextBlock.Text = "Cannot insert a duplicate!";
            }
        }
    }
}
