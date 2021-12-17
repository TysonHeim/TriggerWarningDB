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
    /// Interaction logic for UserTriggerAdder.xaml
    /// </summary>
    public partial class UserTriggerAdder : Window
    {
        public UserTriggerAdder()
        {
            InitializeComponent();
            db dbb = new db();
            var triggers = from t in dbb.Trig
                           orderby t.tName
                           select new { tname = t.tName };
            foreach (var o in triggers)
            {
                addmtrigcombo.Items.Add(new ComboBoxItem() { Content = o.tname });
            }
        }

        private int getTIDFromName(string s)
        {
            db dbb = new db();
            var id = from i in dbb.Trig
                     where i.tName == s
                     select new { idd = i.ID };
            foreach (var o in id)
            {
                return o.idd;
            }
            return -1;
        }
        private void AddUTrigger_Click(object sender, RoutedEventArgs e)
        {
            db dbb = new db();

            UserTriggers MT = new UserTriggers();

            MT.TrigID = getTIDFromName(addmtrigcombo.Text); //get selected trigger id
            MT.UserID = MainWindow.currUserID;
            int s = Convert.ToInt32(AddSeverity.Value); //Convert.ToInt32()
            MT.Severity = s;

            if(MT.Severity == 0)
            {
                MessageBox.Show("Cannot add a trigger with severity 0");
            } else if (!MainWindow.loggedIN)
            {
                MessageBox.Show("Log in first!");
            } else
            {
                try
                {
                    dbb.UserTriggers.InsertOnSubmit(MT);

                    dbb.SubmitChanges();
                    MessageBox.Show("Trigger Added, thank you!");
                } catch
                {
                    MessageBox.Show("Your trigger selection or User ID is not valid");
                }
            }
        }
    }
}
