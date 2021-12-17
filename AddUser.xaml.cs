using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace d
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();


        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            //add user!
            db dbb = new db();
            Users emp = new Users();
            emp.pass = pWord.Text; //cannot be "password"
            emp.Uname = uName.Text; // cannot already be in the database 
            emp.fName = First.Text; //cannot be first
            emp.lName = Last.Text; //cannot be last


            db ddb = new db();
            var UnameCheck = from u in ddb.Users
                             where u.Uname == emp.Uname
                             select new { n = u.Uname };


            if (emp.pass == "Password")  // MessageBox.Show();
            {
                MessageBox.Show("Your Password can't be password");
            }
            else if (UnameCheck != null)
            {
                MessageBox.Show("User name already exists");
            }
            else if (emp.fName == "First")
            {
                MessageBox.Show("Your first name can't be first");
            }
            else if (emp.lName == "Last")
            {
                MessageBox.Show("Your last name can't be last");
            }
            else
            {
                try
                {
                    dbb.Users.InsertOnSubmit(emp);
                }
                catch
                {
                    MessageBox.Show("Error");
                }
                dbb.SubmitChanges();

            }
        }






    }
}
//The fitness gram pacer test is a multi-stage aerobic capacity test that-
//gets progressively more difficult as it continues