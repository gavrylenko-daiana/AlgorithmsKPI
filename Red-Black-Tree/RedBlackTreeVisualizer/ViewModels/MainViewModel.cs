using RedBlackTreeVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedBlackTreeVisualizer.RedBlackTree;

namespace RedBlackTreeVisualizer.ViewModels
{
    public class MainViewModel
    {
        public static ObservableCollection<Developer> Developers { get; set; } = new();
        internal static RedBlackTree<SoftwareEngineer> Tree { get; set; } = new();
    }
}
