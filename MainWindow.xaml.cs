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
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace d
{
    /////////////////////////////////////////////////////////////////////////////////
    /// BINDING DATA TO OBJECTS
    /////////////////////////////////////////////////////////////////////////////////
    
    [Table]
    class Media //hold media info from database
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "Title")]
        public String Title;
        [Column(Name = "SourceID")]
        public Int32 SourceID;
        [Column(Name = "GenreID")]
        public Int32 GenreID;
        [Column(Name = "Creator")]
        public String Creator;
        [Column(Name = "TypeID")]
        public Int32 TypeID;
        [Column(Name = "SearchTally")]
        public Int32 SearchTally;
        [Column(Name = "ReleaseDate")]
        public String Date;
    }
    [Table]
    class Genre
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "Gname")]
        public String Gname;

    }
    [Table]
    class MediaSource
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "Sname")]
        public String Sname;
        [Column(Name = "Link")]
        public String Link;

    }
    [Table]
    class MediaTriggers
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "MediaID")]
        public Int32 MediaID;
        [Column(Name = "TriggerID")]
        public Int32 TriggerID;
        [Column(Name = "ReportedBy")]
        public Int32 ReportedBy;
        [Column(Name = "TLocation")]
        public String TLocation;
        [Column(Name = "DateReported")]
        public String DateReported;
        [Column(Name = "Comment")]
        public String Comment;
        [Column(Name = "Upvotes")]
        public Int32 Upvotes;
        [Column(Name = "Downvotes")]
        public Int32 Downvotes;
        [Column(Name = "Severity")]
        public Int32 Severity;

    }
    [Table]
    class MediaType
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "TName")]
        public String TName;


    }

    [Table]
    class Users
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "UserName")]
        public String Uname;
        [Column(Name = "fName")]
        public String fName;
        [Column(Name = "lName")]
        public String lName;
        [Column(Name = "pass")]
        public String pass;


    }
    [Table]
    class UserTriggers
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "UserID")]
        public Int32 UserID;
        [Column(Name = "TrigID")]
        public Int32 TrigID;
        [Column(Name = "Severity")]
        public Int32 Severity;


    }
    [Table]
    class Trig
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public Int32 ID;
        [Column(Name = "tName")]
        public String tName;
        [Column(Name = "descrip")]
        public String descrip;


    }
    /////////////////////////////////////////////////////////////////////////////////
    /// CONNECTING TO DATABASE
    /////////////////////////////////////////////////////////////////////////////////
    class db : DataContext
    {
        public Table<Trig> Trig;
        public Table<UserTriggers> UserTriggers;
        public Table<Users> Users;
        public Table<MediaType> MediaType;
        public Table<MediaTriggers> MediaTriggers;
        public Table<MediaSource> MediaSource;
        public Table<Genre> Genre;
        public Table<Media> Media;

        public db() : base(@"Data Source=CS1; Initial Catalog = Jon; uid = eheim23x; password = 0957845") { }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// ESTABLISH WINDOW
    /////////////////////////////////////////////////////////////////////////////////
    public partial class MainWindow : Window
    {
        /////////////////////////////////////////////////////////////////////////////////
        /// VARIABLES USED THROUGHOUT CODE
        /////////////////////////////////////////////////////////////////////////////////
        static public string searchkey = ""; //whatever is in the search box
        static public string user = "Ty Heim"; //make this get info from login
        static public int currUserID; //the id of the logged in user -> less joins in queries
        static public int showingMediaID; //the id of the media displayed
        static public int currTrigID;
        static public bool loggedIN = false;

        /////////////////////////////////////////////////////////////////////////////////
        /// INITIALIZING WINDOW
        /////////////////////////////////////////////////////////////////////////////////
        public MainWindow()
        {
            InitializeComponent(); //make window
            fillAddMTrigCombo(); //fill the trigger dropdown
            fillGenreCombo(); //fill the genre dropdown
            fillTypeCombo(); //fill the media type dropdown
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// FILLING DROP DOWN WITH TRIGGERS
        /////////////////////////////////////////////////////////////////////////////////
        void fillAddMTrigCombo()
        {
            db dbb = new db();
            var triggers = from t in dbb.Trig //grab all the names of triggers in the database
                           orderby t.tName
                           select new { tname = t.tName };
            foreach (var o in triggers)
            {
                addmtrigcombo.Items.Add(new ComboBoxItem() { Content = o.tname }); // add to combo box
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// FILLING DROP DOWN WITH GENRES
        /////////////////////////////////////////////////////////////////////////////////
        void fillGenreCombo()
        {
            db dbb = new db();
            var genres = from t in dbb.Genre //grab all the genres in the database
                           orderby t.Gname
                           select new { tname = t.Gname };
            foreach (var o in genres)
            {
                GenreCB.Items.Add(new ComboBoxItem() { Content = o.tname }); //add to combobox
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// FILLING DROP DOWN WITH Types
        /////////////////////////////////////////////////////////////////////////////////
        void fillTypeCombo()
        {
            db dbb = new db();
            var genres = from t in dbb.MediaType //grab all the names of types from db
                         orderby t.TName
                         select new { tname = t.TName };
            foreach (var o in genres)
            {
                MediaTypeCB.Items.Add(new ComboBoxItem() { Content = o.tname }); //add to combo box
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// SUGGEST MEDIA
        /////////////////////////////////////////////////////////////////////////////////
        void suggest()
        {
            db dbb = new db();
            var genres = from t in dbb.Media //grab media that fits the selections of the user
                         where t.TypeID == getTypeIDFromName(MediaTypeCB.Text)
                         where t.GenreID == getGenreIDFromName(GenreCB.Text)
                         orderby t.Title
                         select new { name = t.Title, id = t.ID};
            foreach (var o in genres)
            {
                suggestionBox.Items.Add(new ComboBoxItem() { Content = o.name }); //add each suggestion to the suggestion box
                //searchForMedia(o.name);
                if (loggedIN)
                {
                    if (getsafety(o.id))
                    {
                        suggestionBox.Items.Add(new Label() { Content = "                     Safe" });
                    }
                    else
                    {
                        suggestionBox.Items.Add(new Label() { Content = "                       Unsafe"});
                    }
                }
            }
            
        }

        public bool getsafety(int i)
        {
            Safety(true);

            db dbb = new db();
            var trigs = from q in dbb.MediaTriggers //grab triggers user has in common with media
                        from w in dbb.UserTriggers
                        where q.TriggerID == w.TrigID
                        where q.MediaID == i
                        where w.UserID == currUserID
                        orderby w.TrigID
                        select new { tID = w.TrigID, sevM = q.Severity, sevU = w.Severity, d = q.Downvotes, u = q.Upvotes, c = q.Comment };
            int prev = -1; //we want to only put out the label the first time a trigger is outputted
            bool safe = true;
            try
            {
                if (trigs != null)
                {
                    foreach (var o in trigs)
                    {

                        if (o.sevM <= o.sevU)
                        {
                            safe = false;

                        }
                        if (safe == false)
                        {
                            //Safety(false);
                            return false;
                        }
                    }
                }
            }
            catch
            {

            }
            return true;

        }
        /////////////////////////////////////////////////////////////////////////////////
        /// MAIN SEARCH FUNCTION
        /////////////////////////////////////////////////////////////////////////////////
        //Checks what the user searches for, sees if it is in the database, and sets all media info
        void searchForMedia(string s)
        {
            db dbb = new db();
            var searchRes = from p in dbb.Media //grabbing media whose title contains what the user searched for
                            from t in dbb.Genre
                            from m in dbb.MediaType
                            from b in dbb.MediaSource
                            where b.ID == p.SourceID
                            where p.GenreID == t.ID
                            where p.TypeID == m.ID
                            where p.Title.Contains(s)
                            select new { title = p.Title, id = p.ID, g = t.Gname, t = m.TName, b = b.Sname, c = p.Creator };
            foreach (var o in searchRes)
            {
                MediaTitle.Content = o.title; //populate all the spots with the information
                showingMediaID = o.id;
                string st = o.g + " " + o.t + " on " + o.b;
                GenreType.Content = st;
                Creator.Content = o.c;
            }

        }

        /////////////////////////////////////////////////////////////////////////////////
        /// SET VISIBILITY OF SAFETY
        /////////////////////////////////////////////////////////////////////////////////
        private void Safety(bool t)
        {
            if (t) //set to safe, show safe tag
            {
                Safe.Visibility = Visibility.Visible;
                Unsafe.Visibility = Visibility.Hidden;
            }
            else //set to unsafe, show unsafe tag
            {
                Safe.Visibility = Visibility.Hidden;
                Unsafe.Visibility = Visibility.Visible;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// POPULATE USERS TRIGGER INFO
        /////////////////////////////////////////////////////////////////////////////////
        void popTrigInfo(int tID, bool u)
        {
            db dbb = new db();
            var trigInfo = from t in dbb.Trig //grabbing all triggers for the user.
                           where t.ID == tID
                           select new { n = t.tName };
            foreach (var o in trigInfo)
            {
                if (u)
                {
                    YourTrigg.Children.Add(new Label() { Content = o.n }); //if user has it
                }
                else
                {
                    AllTrigg.Children.Add(new Label() { Content = o.n }); //if user doesnt have it
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// POPULATE TRIGGERS SHARED BETWEEN USER AND MEDIA
        /////////////////////////////////////////////////////////////////////////////////
        void popYourTrig()
        {
            Safety(true);

            db dbb = new db();
            var trigs = from q in dbb.MediaTriggers //grab triggers user has in common with media
                        from w in dbb.UserTriggers
                        where q.TriggerID == w.TrigID
                        where q.MediaID == showingMediaID
                        where w.UserID == currUserID
                        orderby w.TrigID
                        select new { tID = w.TrigID, sevM = q.Severity, sevU = w.Severity, d = q.Downvotes, u = q.Upvotes, c = q.Comment };
            int prev = -1; //we want to only put out the label the first time a trigger is outputted
            bool safe = true;

            if (trigs != null)
            {
                foreach (var o in trigs)
                {
                    //MessageBox.Show("Thing");
                    if (prev != o.tID)
                    {
                        AllTrigg.Children.Add(new Separator());
                        popTrigInfo(o.tID, true);
                        //get average rating
                    }
                    if (o.sevM <= o.sevU)
                    {
                        safe = false;
                    }
                    if (safe == false)
                    {
                        Safety(false);
                    }
                    YourTrigg.Children.Add(new ProgressBar() { Value = o.sevM * 10 }); //replace with the average
                    //YourTrigg.Children.Add(new Label() { Content = o.u / (o.u + o.d) });
                    string i = o.u + " Upvotes        " + o.d + " Downvotes";
                    YourTrigg.Children.Add(new Label() { Content = i });
                    YourTrigg.Children.Add(new Label() { Content = o.c });
                    prev = o.tID;
                }
            }


        }

        /////////////////////////////////////////////////////////////////////////////////
        /// BINDING DATA TO OBJECTS
        /////////////////////////////////////////////////////////////////////////////////
        void popAllTrigDet() //detailed version
        {
            db dbb = new db();
            var trigs = from q in dbb.MediaTriggers
                        where q.MediaID == showingMediaID
                        orderby q.TriggerID
                        orderby q.Severity descending
                        select new { tID = q.TriggerID, sevM = q.Severity, d = q.Downvotes, u = q.Upvotes, c = q.Comment };
            int prev = -1;
            int sum = 0;
            int numRating = 0;
            try
            {
                if (trigs != null)
                {
                    foreach (var o in trigs)
                    {
                        //MessageBox.Show("Thing");
                        if (prev != o.tID)
                        {
                            if (numRating != 0)
                            {
                                AllTrigg.Children.Add(new ProgressBar() { Value = sum / numRating * 10 });
                                AllTrigg.Children.Add(new Label() { Content = "Average Rating: " + sum / numRating });
                            }
                            sum = 0;
                            numRating = 0;
                            AllTrigg.Children.Add(new Separator());
                            popTrigInfo(o.tID, false);
                            //get average rating


                        }
                        string s = "Rated: " + o.sevM + "/10";
                        string i = o.u + " Upvotes        " + o.d + " Downvotes";
                        AllTrigg.Children.Add(new Label() { Content = s });
                        AllTrigg.Children.Add(new Label() { Content = o.c });
                        AllTrigg.Children.Add(new Label() { Content = i });
                        sum += o.sevM;
                        numRating += 1;

                        prev = o.tID;
                    }
                }
                AllTrigg.Children.Add(new ProgressBar() { Value = sum / numRating * 10 });
                AllTrigg.Children.Add(new Label() { Content = "Average Rating: " + sum / numRating });
            }
            catch
            {

            }
        }

        void popAllTrig()
        {
            db dbb = new db();
            var trigs = from q in dbb.MediaTriggers
                        where q.MediaID == showingMediaID
                        orderby q.TriggerID
                        select new
                        {
                            tID = q.TriggerID == null ? -1 : q.TriggerID,
                            sevM = q.Severity == null ? 0 : q.Severity,
                            d = q.Downvotes == null ? 0 : q.Downvotes,
                            u = q.Upvotes == null ? 0 : q.Upvotes,
                            c = q.Comment == null ? "No Comment" : q.Comment
                        };

            int prev = -1;
            int sum = 0;
            int numRating = 0;
            try
            {

                if (trigs != null)
                {


                    foreach (var o in trigs)
                    {
                        //MessageBox.Show("Thing");
                        if (prev != o.tID)
                        {
                            if (numRating != 0)
                            {
                                AllTrigg.Children.Add(new ProgressBar() { Value = sum / numRating * 10 });
                                AllTrigg.Children.Add(new Label() { Content = "Average Rating: " + sum / numRating });
                            }
                            sum = 0;
                            numRating = 0;
                            AllTrigg.Children.Add(new Separator());
                            popTrigInfo(o.tID, false);
                            //get average rating


                        }

                        sum += o.sevM;
                        numRating += 1;

                        prev = o.tID;
                    }
                }
                if (numRating != 0)
                {
                    AllTrigg.Children.Add(new ProgressBar() { Value = sum / numRating * 10 });
                }

                AllTrigg.Children.Add(new Label() { Content = "Average Rating: " + sum / numRating });
            }
            catch
            {
                AllTrigg.Children.Add(new Label() { Content = "No Triggers Listed" });
            }
        }

        private void Go(object sender, RoutedEventArgs e) //when the user clicks search
        {
            AllTrigg.Children.Clear(); //clear all previous data
            YourTrigg.Children.Clear();
            searchkey = SearchBox.Text; //store the thing searched
            //this.Title = searchkey; 

            searchForMedia(searchkey); //search for it
            SearchResults.Content = "Showing Results from '" + searchkey + "'..."; //say we are searching for it
            //search query!
            popAllTrig(); //poplate all the triggers in the media
            if (loggedIN) { popYourTrig(); } else { Safety(true); } //if logged in populate shared triggers, if not say it is safe

        }

        private void AddTrig_Click(object sender, RoutedEventArgs e)
        {
            UserTriggerAdder at = new UserTriggerAdder(); //open new window
            at.Show();

        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Trigger1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

       
        private void addTrigger_Click(object sender, RoutedEventArgs e) //add trigger to media
        {

            db dbb = new db();

            MediaTriggers MT = new MediaTriggers();

            MT.MediaID = showingMediaID;
            MT.TriggerID = getTIDFromName(addmtrigcombo.Text); //get selected trigger id
            MT.ReportedBy = currUserID;
            MT.TLocation = AddTime.Text;
            MT.DateReported = DateTime.Today.ToString();
            MT.Comment = AddComment.Text;
            MT.Upvotes = 0;
            MT.Downvotes = 0;
            int s = Convert.ToInt32(AddSeverity.Value); //Convert.ToInt32()
            MT.Severity = s;

            if (MT.TLocation == "TIME STAMP") //check time stamp isnt "TIME STAMP"
            {
                MessageBox.Show("Time is NULL");
            }
            else if (MT.Comment == "COMMENT") //check comment isnt "COMMENT"
            {
                MessageBox.Show("Comment is COMMENT");
            }
            else if (loggedIN != true) //check the login bool global var is true
            {
                MessageBox.Show("No one is logged in >:/ ");
            }
            else if (showingMediaID == null) //check that the showing media id global variable isn't null
            {
                MessageBox.Show("");
            }
            else //if pa yet!ss all checks then do insert and submit
            {
                try
                {

                    dbb.MediaTriggers.InsertOnSubmit(MT);
                    //dbb.MediaTriggers.InsertOnSubmit(MT);
                    dbb.SubmitChanges();
                    MessageBox.Show("Trigger Added, thank you!");
                }
                catch
                {
                    MessageBox.Show("Error on Insertion/Submit");
                }
               
                //dbb.SubmitChanges();
                

            }

            
            AddSeverity.Value = 0;
            AddComment.Text = "Comment";
            AddTime.Text = "Time Stamp";

        }

        ///////////////////////////////
        /// GETTING IDS BASED ON NAMES FOR INSERTING
        /// ///////////////////////////
        private int getTIDFromName(string s) 
        {
            db dbb = new db();
            var id = from i in dbb.Trig
                     where i.tName == s
                     select new { idd = i.ID };
            foreach (var o in id)
            {
                this.Title = Convert.ToString(o.idd);
                return o.idd;
            }
            return -1;
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


        private void login_Click(object sender, RoutedEventArgs e) //log in button
        {
            string enteredName = UName.Text;
            string enteredPass = Pword.Text;
            db dbb = new db();
            var pass = from u in dbb.Users //from Users U
                       where u.Uname == enteredName // where u.uname = enteredname
                       select new { pass = u.pass, i = u.ID, f = u.fName, l = u.lName }; // select u.pass as Pass, u.ID as i, u.fname as f, u.lname as l
            foreach (var o in pass) // go through each result label the result o
            {

                if (o.pass == enteredPass) //if the password is correct
                {
                    currUserID = o.i;
                    user = o.f + " " + o.l;
                    UserName.Content = "User Name: " + user;
                    getUserTriggers(); //populate user info
                    loggedIN = true;
                }
                else //if wrong password
                {
                    MessageBox.Show("Incorrect Login");
                }
            }


        }

        private void getUserTriggers()
        {
            UserTriggersList.Items.Clear(); //clear previous
            db dbb = new db();
            var ts = from u in dbb.UserTriggers //get all the triggers the user has and sort by severity
                     from t in dbb.Trig
                     where u.UserID == currUserID
                     where t.ID == u.TrigID
                     orderby u.Severity descending
                     select new { name = t.tName, s = u.Severity };
            foreach (var o in ts)
            {
                UserTriggersList.Items.Add(new Label() { Content = o.name });
                UserTriggersList.Items.Add(new ProgressBar() { Value = o.s * 10, Height = 10, Width = 250 });
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Mnewtrig_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void addmtrigcombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void detail_click(object sender, RoutedEventArgs e)
        {
            AllTrigg.Children.Clear();
            popAllTrigDet();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser at = new AddUser(); //new window
            at.Show();
            //MessageBox.Show(Convert.ToString(AddSeverity.Value));
        }

        private void AddMedia_Click(object sender, RoutedEventArgs e)
        {
            AddMedia at = new AddMedia();
           at.Show();
        }

        private void suggestbutton_Click(object sender, RoutedEventArgs e)
        {
            suggestionBox.Items.Clear(); //clear previous suggestions
            suggest(); //get new suggestions
        }
    }
}
