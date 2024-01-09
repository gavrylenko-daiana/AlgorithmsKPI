using RedBlackTreeVisualizer.Models;
using RedBlackTreeVisualizer.RedBlackTree;
using RedBlackTreeVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RedBlackTreeVisualizer.Views
{
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void SearchCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SearchSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SearchViewModel.FilteredDevelopers.Clear();
            var error = SearchViewModel.GetErrorField(SearchKeyTextBox.Text);

            if (error != string.Empty)
            {
                MessageTextBlock.Text = error;
                return;
            }

            var key = Convert.ToInt32(SearchKeyTextBox.Text);

            (IEnumerable<RedBlackNode<SoftwareEngineer>> nodes, var count) = MainViewModel.Tree.Search(key);

            if (!nodes.Any())
            {
                MessageTextBlock.Text = "There is no such key in the tree!";
                return;
            }

            foreach (RedBlackNode<SoftwareEngineer> node in nodes)
            {
                SearchViewModel.FilteredDevelopers.Add(new Developer(key, node.Value.Name, node.Value.Surname));
            }

            CountTextBlock.Text = count.ToString();
        }
    }
}
