using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HENRY.Model;
using System.Collections.ObjectModel;

namespace HENRY.ViewModel
{
    public class ViewModel
    {

        public ObservableCollection<Student> Things
        {
            get;
            set;
        }

        public void LoadStudents(ObservableCollection<Student> thingie)
        {
            /*
            ObservableCollection<Student> students = new ObservableCollection<Student>();

            students.Add(new Student { FirstName = "Mark", LastName = "Allain" });
            students.Add(new Student { FirstName = "Allen", LastName = "Brown" });
            students.Add(new Student { FirstName = "Linda", LastName = "Hamerski" });
            */

            Things = thingie;
        }
    } 
}
