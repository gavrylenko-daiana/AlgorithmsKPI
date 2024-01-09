using RedBlackTreeVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTreeVisualizer.ViewModels
{
    public class UpdateViewModel
    {
        public static string GetErrorFields(string name, string surname)
        {
            StringBuilder incorrectFields = new();

            if (name == string.Empty)
            {
                if (incorrectFields.Length == 0)
                {
                    incorrectFields.Append("name");
                }
            }

            if (surname == string.Empty)
            {
                if (incorrectFields.Length == 0)
                {
                    incorrectFields.Append("surname");
                }
                else
                {
                    incorrectFields.Append(", surname");
                }
            }

            return incorrectFields.ToString();
        }
    }
}
