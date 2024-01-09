using RedBlackTreeVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTreeVisualizer.ViewModels
{
    public class SearchViewModel
    {
        public static ObservableCollection<Developer> FilteredDevelopers { get; set; } = new();

        public static string GetErrorField(string keyLine)
        {
            var error = string.Empty;

            if (DataValidator.IsIdValid(keyLine) == false)
            {
                error = "key";
            }

            return error;
        }
    }
}
