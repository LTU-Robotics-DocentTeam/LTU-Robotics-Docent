using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMTest.Model;
using System.Collections.ObjectModel;

namespace MVVMTest.ViewModel
{
    public class ViewModel
    {

        public ObservableCollection<Student> Students
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

            Students = thingie;
        }
    } 
}
