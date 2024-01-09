using RedBlackTreeVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RedBlackTreeVisualizer.ViewModels
{
    public class AddViewModel
    {
        public static string GetErrorFields(string keyLine, string name, string surname)
        {
            StringBuilder incorrectFields = new();

            if (DataValidator.IsIdValid(keyLine) == false)
            {
                incorrectFields.Append("key");
            }
            
            if (name == string.Empty)
            {
                if (incorrectFields.Length == 0)
                {
                    incorrectFields.Append("name");
                }
                else
                {
                    incorrectFields.Append(", name");
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
