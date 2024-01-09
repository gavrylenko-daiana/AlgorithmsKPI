using RedBlackTreeVisualizer.Models;
using RedBlackTreeVisualizer.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace RedBlackTreeVisualizer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDataBase();
        }

        private void LoadDataBase()
        {
            try
            {
                if (JsonConvert.DeserializeObject<ObservableCollection<Developer>>(File.ReadAllText(Settings.DataBasePath)) is ObservableCollection<Developer> data)
                {
                    foreach (var employee in data)
                    {
                        if (data.GetKeyCount(employee.Key) == 1)
                        {
                            MainViewModel.Developers.Add(employee);
                        }
                    }

                    if (data.Count > MainViewModel.Developers.Count)
                    {
                        MessageTextBlock.Text = "Duplicates in the file are skipped!";
                    }
                }

                foreach (var employee in MainViewModel.Developers)
                {
                    MainViewModel.Tree.Add(employee.Key, new SoftwareEngineer(employee.Name!, employee.Surname!));
                }
            }
            catch (JsonReaderException e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("File is not found");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageTextBlock.Text = "";
            InsertWindow window = new();
            window.Show();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.DataBasePath) == false)
            {
                MessageTextBlock.Text = "This file does not exist!";
            }
            else
            {
                File.WriteAllText(Settings.DataBasePath, JsonConvert.SerializeObject(MainViewModel.Developers));
                MessageTextBlock.Text = "";
            }
        }

        private void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.DataBasePath) == true)
            {
                MessageTextBlock.Text = "The file already exists!";
            }
            else
            {
                File.Create(Settings.DataBasePath).Dispose();
                MessageTextBlock.Text = "";
            }
        }

        private void DropFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.DataBasePath) == false)
            {
                MessageTextBlock.Text = "This file does not exist!";
            }
            else
            {
                File.Delete(Settings.DataBasePath);
                MessageTextBlock.Text = "";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageTextBlock.Text = "";
            SearchWindow window = new();
            window.Show();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageTextBlock.Text = "";

            var selectedRows = DataTable.SelectedItems;

            if (selectedRows.Count == 0)
            {
                MessageTextBlock.Text = "There is no rows selected!";
                return;
            }

            List<Developer> employees = selectedRows.ToDevelopers();


            foreach (var employee in employees)
            {
                if (MainViewModel.Developers.Contains(employee))
                {
                    MainViewModel.Developers.Remove(employee);
                }
                else
                {
                    MessageTextBlock.Text += $"Employee #{employee.Key} cannot be deleted from table!";
                }

                if (MainViewModel.Tree.HasId(employee.Key))
                {
                    MainViewModel.Tree.Remove(employee.Key);
                }
                else
                {
                    MessageTextBlock.Text += $"Employee #{employee.Key} cannot be deleted from tree!";
                }
            }

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var rows = DataTable.SelectedItems;

            if (rows.Count != 1)
            {
                MessageTextBlock.Text = "Incorrect number of lines is selected!";
                return;
            }

            var selectedDeveloper = rows.ToDevelopers()[0];

            if (MainViewModel.Developers.Contains(selectedDeveloper) == false ||
                MainViewModel.Tree.HasId(selectedDeveloper.Key) == false)
            {
                MessageTextBlock.Text = "This tuple cannot be modified!";
            }

            UpdateWindow window = new(selectedDeveloper);
            window.Show();
        }
    }
}
