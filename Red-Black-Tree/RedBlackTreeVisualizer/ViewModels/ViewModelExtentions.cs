using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTreeVisualizer.ViewModels
{
    public static class ViewModelExtentions
    {
        public static int GetKeyCount(this ObservableCollection<Developer> employees, int key)
        {
            var count = 0;

            foreach (var employee in employees)
            {
                if (employee.Key == key)
                {
                    count += 1;
                }
            }

            return count;
        }

        public static List<Developer> ToDevelopers(this IList rows)
        {
            List<Developer> employees = new();

            foreach (Developer employee in rows)
            {
                employees.Add(employee);
            }

            return employees;
        }
    }
}
