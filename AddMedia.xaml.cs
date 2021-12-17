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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace d
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AddMedia : Window
    {
        public AddMedia() { InitializeComponent(); populateGenre(); populateSource(); populateType(); }

        public void populateGenre()
        {
            db dbb = new db();
            var genres = from t in dbb.Genre //grab all the genres in the database
                         orderby t.Gname
                         select new { gname = t.Gname };
            foreach (var o in genres)
            {
                mediaGenre.Items.Add(new ComboBoxItem() { Content = o.gname }); //add to combobox
            }

        }

        public void populateSource()
        {
            db dbb = new db();
            var sources = from t in dbb.MediaSource //grab all sources in database 
                          orderby t.Sname
                          select new { sname = t.Sname };
            foreach (var o in sources)
            {
                mediaSource.Items.Add(new ComboBoxItem() { Content = o.sname }); //add to combo box
            }
        }

        public void populateType()
        {
            db dbb = new db();
            var mType = from t in dbb.MediaType //grab all types from database 
                        orderby t.TName
                        select new { tname = t.TName };
            foreach (var o in mType)
            {
                mediaType.Items.Add(new ComboBoxItem() { Content = o.tname }); //add to combo box
            }
        }


        private int getTypeIDFromName(string s)
        {
            db dbb = new db();
            var id = from i in dbb.MediaType
                     where i.TName == s
                     select new { idd = i.ID };
            foreach (var o in id)
            {
                this.Title = Convert.ToString(o.idd);
                return o.idd;
            }
            return -1;
        }

        private int getGenreIDFromName(string s)
        {
            db dbb = new db();
            var id = from i in dbb.Genre
                     where i.Gname == s
                     select new { idd = i.ID };
            foreach (var o in id)
            {
                this.Title = Convert.ToString(o.idd);
                return o.idd;
            }
            return -1;
        }

        private int getSourceIDFromName(string s)
        {
                        db dbb = new db();
            var id = from i in dbb.MediaSource
                     where i.Sname == s
                     select new { idd = i.ID };
            foreach (var o in id)
            {
                this.Title = Convert.ToString(o.idd);
                return o.idd;
            }
            return -1;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            db dbb = new db();

            Media MT = new Media();

            MT.Title = MediaTitle.Text;
            MT.Creator = Creator.Text;
            MT.Date = ReleaseDate.Text;
            MT.SourceID = getSourceIDFromName(mediaSource.Text);
            MT.TypeID = getTypeIDFromName(mediaType.Text);
            MT.GenreID = getGenreIDFromName(mediaGenre.Text);

            if (MT.Date == "NULL")
            {
                MessageBox.Show("Time is NULL");
            }
            else if (MainWindow.loggedIN != true) //check the login bool global var is true
            {
                MessageBox.Show("No one is logged in >:/ ");
            }
            else //if pass all checks then do insert and submit
            {
                try
                {

                    dbb.Media.InsertOnSubmit(MT);
                    //dbb.MediaTriggers.InsertOnSubmit(MT);
                    dbb.SubmitChanges();
                    MessageBox.Show("Media Added, thank you!");
                }
                catch
                {
                    MessageBox.Show("Error on Insertion/Submit");
                }


            }


        }

    }
}