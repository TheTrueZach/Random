using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BC = BCrypt.Net.BCrypt;

namespace Password_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string url = "https://www.youtube.com/";
        private int index = 0;
        private string[] visited = new string[1];
        private int x = 0;
        private bool pass = false;
        private string passHash = BC.HashPassword("4letters98", workFactor: 15);
        private string favFilePath = "C:\\Users\\zandrews.JOI\\Desktop\\Favorites";
        private string readFile = "C:\\Users\\zandrews.JOI\\Desktop\\Encriped.txt";
        private string taskFile = "C:\\Users\\zandrews.JOI\\Desktop\\Tasks\\tasks.txt";
        private string steamFile = "C:\\Users\\zandrews.JOI\\Desktop\\Steam";
        private string blizzFile = "C:\\Users\\zandrews.JOI\\Desktop\\Blizzard";
        private string epicFile = "C:\\Users\\zandrews.JOI\\Desktop\\Epic";
        private DataTable temp = new DataTable();
        private List<TaskMaker> tasks = new List<TaskMaker>();
        private long tick;
        private long todaysTicks;
        private long remaining;
        private TimeSpan timeSpan;
        private string[] fileText;
        private Thread thread;
        private bool run = true;
        private bool runfinished = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
            URL.Text = url;
            visited[index] = url;
            string[] red = File.ReadAllLines(readFile);
            string column1 = "UserN";
            string column2 = "UserP";
            string column3 = "UserE";
            DataTable dataGrid = DataGridMakers(temp, "UserN", "UserP", "UserE");
            List<string> green = new List<string>();
            DataRow row;
            foreach (string lines in red)
            {
                string[] line = lines.Split('-');
                string unline1 = Converter(line[0], true);
                string unline2 = Converter(line[1], true);
                string unline3 = Converter(line[2], true);
                green.Add(lines);
                row = dataGrid.NewRow();
                row[column1] = unline1;
                row[column2] = unline2;
                row[column3] = unline3;
                dataGrid.Rows.Add(row);
            }
            DG.DataContext = dataGrid;
            try
            {
                string ExistingTasks = File.ReadAllText(taskFile);
                ExistingTasks = ExistingTasks.Substring(0, ExistingTasks.Length - 1);
                fileText = ExistingTasks.Split('?');
                foreach (string item in fileText)
                {
                    TaskMaker taskMaker = new TaskMaker(item);
                    tasks.Add(taskMaker);
                    selector.ItemsSource = new List<Object>();
                    selector.ItemsSource = tasks;
                }
            }
            catch { fileText = new string[1]; fileText[0] = ""; }
        }

        async void InitializeAsync()
        {
            await webViewFull.EnsureCoreWebView2Async(null);
        }

        private void MiniScreen_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            Bound.IsOpen = true;
            Mini.PlacementTarget = Bound;
            Mini.IsOpen = true;
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            FullScreen1.Visibility = Visibility.Visible;
            FullScreen2.Visibility = Visibility.Visible;
        }

        private void webView_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void URL_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                try
                {
                    url = URL.Text;
                    webViewFull.Source = new Uri(url);
                    IncreaseArray();
                }
                catch
                {

                }
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if(index < visited.Length - 1) 
            {
                index++;
                try
                {
                    url = visited[index];
                    webViewFull.Source = new Uri(url);
                    URL.Text = url;
                }
                catch
                {
                    index--;
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0)
            {
                index--;
                try
                {
                    url = visited[index];
                    webViewFull.Source = new Uri(url);
                    URL.Text = url;
                }
                catch
                {
                    index++;
                }
            }
        }

        private void IncreaseArray()
        {
            index = visited.Length - 1;
            string[] temp = new string[visited.Length + 1];
            visited.CopyTo(temp, 0);
            visited = temp;
            index++;
            visited[index] = url;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            url = "https://google.com";
            URL.Text = url;
            webViewFull.Source = new Uri(url);
            IncreaseArray();
        }

        private void CloseAll()
        {
            FullScreen1.Visibility = Visibility.Collapsed;
            FullScreen2.Visibility = Visibility.Collapsed;
            Mini.IsOpen = false;
            Bound.IsOpen = false;
            Secret.Visibility = Visibility.Collapsed;
            DG.Visibility = Visibility.Collapsed;
            GamesShowCase.Visibility = Visibility.Collapsed;
            Tasks.Visibility = Visibility.Collapsed;
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
          
            Point xy = Mouse.GetPosition(Mini);
            double x = xy.X;
            double y = xy.Y;
            Mini.HorizontalOffset = x;
            Mini.VerticalOffset = y;
        }

        private void Fav_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            GamesShowCase.Visibility = Visibility.Visible;
            string[] dirs = Directory.GetDirectories(favFilePath);
            List<GameObject> list = new List<GameObject>();
            foreach (string item in dirs)
            {
                GameObject temp = new GameObject(item, true);
                list.Add(temp);
            }
            GamesShowCase.ItemsSource = list;
        }

        private void Steam_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            GamesShowCase.Visibility = Visibility.Visible;
            string[] dirs = Directory.GetDirectories(steamFile);
            List<GameObject> list = new List<GameObject>();
            foreach (string item in dirs)
            {
                GameObject temp = new GameObject(item, false);
                list.Add(temp);
            }
            GamesShowCase.ItemsSource = list;
        }

        private void Epic_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            GamesShowCase.Visibility = Visibility.Visible;
            string[] dirs = Directory.GetDirectories(epicFile);
            List<GameObject> list = new List<GameObject>();
            foreach (string item in dirs)
            {
                GameObject temp = new GameObject(item, false);
                list.Add(temp);
            }
            GamesShowCase.ItemsSource = list;
        }

        private void Blizz_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            GamesShowCase.Visibility = Visibility.Visible;
            string[] dirs = Directory.GetDirectories(blizzFile);
            List<GameObject> list = new List<GameObject>();
            foreach (string item in dirs)
            {
                GameObject temp = new GameObject(item, false);
                list.Add(temp);
            }
            GamesShowCase.ItemsSource = list;
        }

        private void Password_Click(object sender, RoutedEventArgs e)
        {
            if (pass == false)
            {
                PassEnter.IsOpen = true;
                Secret.Visibility = Visibility.Visible;        
            }
            else
            {
                CloseAll();
                DG.Visibility = Visibility.Visible;
            }
        }

        private void PasswordReturn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(BC.Verify(PasswordReturn.Text ,passHash))
                {
                    PassEnter.IsOpen = false;
                    pass = true;
                    CloseAll();
                    DG.Visibility = Visibility.Visible;
                }
            }
        }

        private void GamesShowCase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GameObject game = (GameObject)GamesShowCase.SelectedItem;
            System.Diagnostics.Process.Start(game.ExePath);
        }

        private void AddFav_Click(object sender, RoutedEventArgs e)
        {
            GameObject game = (GameObject)GamesShowCase.SelectedItem;
            if (!game.Favorite)
            {
                string current = game.ExePath.Substring(0, game.ExePath.LastIndexOf('\\'));
                string[] stuff = Directory.GetFiles(current);
                string newPath = current.Substring(current.LastIndexOf('\\'), current.Length - current.LastIndexOf('\\'));
                newPath = favFilePath + newPath;
                Directory.CreateDirectory(newPath);
                File.Copy(stuff[0], newPath + '\\' + stuff[0].Substring(stuff[0].LastIndexOf('\\') + 1, stuff[0].Length - stuff[0].LastIndexOf('\\') - 1), true);
                File.Copy(stuff[1], newPath + '\\' + stuff[1].Substring(stuff[1].LastIndexOf('\\') + 1, stuff[1].Length - stuff[1].LastIndexOf('\\') - 1), true);
            }
        }

        public string Converter(string pack, bool inverse)
        {
            if (!inverse)
            {
                List<char> retorn = new List<char>();
                foreach (char item in pack)
                {
                    if (item == 'a') { retorn.Add('S'); };
                    if (item == 'b') { retorn.Add('P'); };
                    if (item == 'c') { retorn.Add('D'); };
                    if (item == 'd') { retorn.Add(','); };
                    if (item == 'e') { retorn.Add('M'); };
                    if (item == 'f') { retorn.Add('a'); };
                    if (item == 'g') { retorn.Add('F'); };
                    if (item == 'h') { retorn.Add('e'); };
                    if (item == 'i') { retorn.Add('m'); };
                    if (item == 'j') { retorn.Add('V'); };
                    if (item == 'k') { retorn.Add('W'); };
                    if (item == 'l') { retorn.Add('u'); };
                    if (item == 'm') { retorn.Add('N'); };
                    if (item == 'n') { retorn.Add('U'); };
                    if (item == 'o') { retorn.Add('A'); };
                    if (item == 'p') { retorn.Add('n'); };
                    if (item == 'q') { retorn.Add('v'); };
                    if (item == 'r') { retorn.Add('p'); };
                    if (item == 's') { retorn.Add('5'); };
                    if (item == 't') { retorn.Add('7'); };
                    if (item == 'u') { retorn.Add('^'); };
                    if (item == 'v') { retorn.Add('h'); };
                    if (item == 'w') { retorn.Add('g'); };
                    if (item == 'x') { retorn.Add('z'); };
                    if (item == 'y') { retorn.Add('Z'); };
                    if (item == 'z') { retorn.Add('y'); };

                    if (item == 'A') { retorn.Add('$'); };
                    if (item == 'B') { retorn.Add('('); };
                    if (item == 'C') { retorn.Add('3'); };
                    if (item == 'D') { retorn.Add('d'); };
                    if (item == 'E') { retorn.Add('2'); };
                    if (item == 'F') { retorn.Add(')'); };
                    if (item == 'G') { retorn.Add('Y'); };
                    if (item == 'H') { retorn.Add('Q'); };
                    if (item == 'I') { retorn.Add('J'); };
                    if (item == 'J') { retorn.Add('x'); };
                    if (item == 'K') { retorn.Add('L'); };
                    if (item == 'L') { retorn.Add('@'); };
                    if (item == 'M') { retorn.Add('I'); };
                    if (item == 'N') { retorn.Add('8'); };
                    if (item == 'O') { retorn.Add('w'); };
                    if (item == 'P') { retorn.Add('O'); };
                    if (item == 'Q') { retorn.Add('q'); };
                    if (item == 'R') { retorn.Add('i'); };
                    if (item == 'S') { retorn.Add('b'); };
                    if (item == 'T') { retorn.Add('X'); };
                    if (item == 'U') { retorn.Add('j'); };
                    if (item == 'V') { retorn.Add('k'); };
                    if (item == 'W') { retorn.Add('l'); };
                    if (item == 'X') { retorn.Add('!'); };
                    if (item == 'Y') { retorn.Add('H'); };
                    if (item == 'Z') { retorn.Add('T'); };

                    if (item == '1') { retorn.Add('E'); };
                    if (item == '2') { retorn.Add('C'); };
                    if (item == '3') { retorn.Add('s'); };
                    if (item == '4') { retorn.Add('9'); };
                    if (item == '5') { retorn.Add('t'); };
                    if (item == '6') { retorn.Add('&'); };
                    if (item == '7') { retorn.Add('f'); };
                    if (item == '8') { retorn.Add('.'); };
                    if (item == '9') { retorn.Add('0'); };
                    if (item == '0') { retorn.Add('R'); };

                    if (item == '!') { retorn.Add('#'); };
                    if (item == '@') { retorn.Add('?'); };
                    if (item == '#') { retorn.Add('K'); };
                    if (item == '$') { retorn.Add('B'); };
                    if (item == '%') { retorn.Add('6'); };
                    if (item == '^') { retorn.Add('%'); };
                    if (item == '&') { retorn.Add('1'); };
                    if (item == '*') { retorn.Add('c'); };
                    if (item == '(') { retorn.Add('4'); };
                    if (item == ')') { retorn.Add('*'); };
                    if (item == ',') { retorn.Add('o'); };
                    if (item == '.') { retorn.Add('G'); };
                    if (item == '?') { retorn.Add('r'); };
                    if (item == ' ') { retorn.Add(' '); };
                }
                string finish = new string(retorn.ToArray());
                return finish;
            }
            if (inverse)
            {
                List<char> retorn = new List<char>();
                foreach (char item in pack)
                {
                    if (item == 'S') { retorn.Add('a'); };
                    if (item == 'P') { retorn.Add('b'); };
                    if (item == 'D') { retorn.Add('c'); };
                    if (item == ',') { retorn.Add('d'); };
                    if (item == 'M') { retorn.Add('e'); };
                    if (item == 'a') { retorn.Add('f'); };
                    if (item == 'F') { retorn.Add('g'); };
                    if (item == 'e') { retorn.Add('h'); };
                    if (item == 'm') { retorn.Add('i'); };
                    if (item == 'V') { retorn.Add('j'); };
                    if (item == 'W') { retorn.Add('k'); };
                    if (item == 'u') { retorn.Add('l'); };
                    if (item == 'N') { retorn.Add('m'); };
                    if (item == 'U') { retorn.Add('n'); };
                    if (item == 'A') { retorn.Add('o'); };
                    if (item == 'n') { retorn.Add('p'); };
                    if (item == 'v') { retorn.Add('q'); };
                    if (item == 'p') { retorn.Add('r'); };
                    if (item == '5') { retorn.Add('s'); };
                    if (item == '7') { retorn.Add('t'); };
                    if (item == '^') { retorn.Add('u'); };
                    if (item == 'h') { retorn.Add('v'); };
                    if (item == 'g') { retorn.Add('w'); };
                    if (item == 'z') { retorn.Add('x'); };
                    if (item == 'Z') { retorn.Add('y'); };
                    if (item == 'y') { retorn.Add('z'); };

                    if (item == '$') { retorn.Add('A'); };
                    if (item == '(') { retorn.Add('B'); };
                    if (item == '3') { retorn.Add('C'); };
                    if (item == 'd') { retorn.Add('D'); };
                    if (item == '2') { retorn.Add('E'); };
                    if (item == ')') { retorn.Add('F'); };
                    if (item == 'Y') { retorn.Add('G'); };
                    if (item == 'Q') { retorn.Add('H'); };
                    if (item == 'J') { retorn.Add('I'); };
                    if (item == 'x') { retorn.Add('J'); };
                    if (item == 'L') { retorn.Add('K'); };
                    if (item == '@') { retorn.Add('L'); };
                    if (item == 'I') { retorn.Add('M'); };
                    if (item == '8') { retorn.Add('N'); };
                    if (item == 'q') { retorn.Add('O'); };
                    if (item == 'O') { retorn.Add('P'); };
                    if (item == 'q') { retorn.Add('Q'); };
                    if (item == 'i') { retorn.Add('R'); };
                    if (item == 'b') { retorn.Add('S'); };
                    if (item == 'X') { retorn.Add('T'); };
                    if (item == 'j') { retorn.Add('U'); };
                    if (item == 'k') { retorn.Add('V'); };
                    if (item == 'l') { retorn.Add('W'); };
                    if (item == '!') { retorn.Add('X'); };
                    if (item == 'H') { retorn.Add('Y'); };
                    if (item == 'T') { retorn.Add('Z'); };

                    if (item == 'E') { retorn.Add('1'); };
                    if (item == 'C') { retorn.Add('2'); };
                    if (item == 's') { retorn.Add('3'); };
                    if (item == '9') { retorn.Add('4'); };
                    if (item == 't') { retorn.Add('5'); };
                    if (item == '&') { retorn.Add('6'); };
                    if (item == 'f') { retorn.Add('7'); };
                    if (item == '.') { retorn.Add('8'); };
                    if (item == '0') { retorn.Add('9'); };
                    if (item == 'R') { retorn.Add('0'); };

                    if (item == '#') { retorn.Add('!'); };
                    if (item == '?') { retorn.Add('@'); };
                    if (item == 'K') { retorn.Add('#'); };
                    if (item == 'B') { retorn.Add('$'); };
                    if (item == '6') { retorn.Add('%'); };
                    if (item == '%') { retorn.Add('^'); };
                    if (item == '1') { retorn.Add('&'); };
                    if (item == 'c') { retorn.Add('*'); };
                    if (item == '4') { retorn.Add('('); };
                    if (item == '*') { retorn.Add(')'); };
                    if (item == 'o') { retorn.Add(','); };
                    if (item == 'G') { retorn.Add('.'); };
                    if (item == 'r') { retorn.Add('?'); };
                    if (item == ' ') { retorn.Add(' '); };
                }
                string finish = new string(retorn.ToArray());
                return finish;
            }

            return "";
        } 
        public DataTable DataGridMakers(DataTable table, string column1, string column2, string column3)
        {
            // Create a new DataTable.
            //System.Data.DataTable table = new DataTable("ParentTable");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType,
            // ColumnName and add to DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = column1;
            column.ReadOnly = false;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = column2;
            column.ReadOnly = false;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = column3;
            column.ReadOnly = false;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Make the location column the primary key column.
            return table.Copy();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //File.WriteAllText("C:\\Users\\zandrews.JOI\\Desktop\\Tasks\\tasks.txt", NewTaskName.Text + "!" + NewTaskInfo.Text + "!" + NewTaskDate.SelectedDate.Value + "!" + NewTaskPriority.Text + "!" + NewTaskAssignment.Text + "?");
            string[] fileTextTemp = fileText;
            fileText = new string[fileTextTemp.Length + 1];
            fileTextTemp.CopyTo(fileText, 0);
            int x = fileText.Length - 1;
            fileText[x] = NewTaskName.Text + "!" + NewTaskInfo.Text + "!" + NewTaskDate.SelectedDate.Value + "!" + NewTaskPriority.Text + "!" + NewTaskAssignment.Text + "!" + NewTaskStatus.Text;
            TaskMaker taskMaker = new TaskMaker(NewTaskName.Text, NewTaskInfo.Text, NewTaskDate.SelectedDate.Value, NewTaskPriority.Text, NewTaskAssignment.Text, NewTaskStatus.Text);
            tasks.Add(taskMaker);
            selector.ItemsSource = new List<Object>();
            selector.ItemsSource = tasks;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaskMaker taskMaker = (TaskMaker)selector.SelectedItem;
            Update_Current(taskMaker);
        }

        private void Update_Current(TaskMaker current)
        {
            if (current != null)
            {
                CurrentTaskName.Text = current.TaskName;
                CurrentTaskInfo.Text = current.TaskInfo;
                CurrentTaskDate.Text = current.Date.ToString();
                CurrentPriority.Text = current.Priority.ToString();
                CurrentAssignedBy.Text = current.AssignedBy;
                Stat.Text = "Status Of Current Task: " + current.Status;
                tick = current.Date.Ticks;
                todaysTicks = DateTime.Now.Ticks;
                remaining = tick - todaysTicks;
                timeSpan = TimeSpan.FromTicks(remaining);
                DaysLeft.Text = "Days: " + timeSpan.Days.ToString();
                HoursLeft.Text = "Hours: " + timeSpan.Hours.ToString();
                MinutesLeft.Text = "Minutes: " + timeSpan.Minutes.ToString();
                SecondsLeft.Text = "Seconds: " + timeSpan.Seconds.ToString();
                if (thread != null)
                {
                    run = false;
                    thread.Abort();
                    run = true;
                    runfinished = false;
                }
                thread = new Thread(Update_Time);
                thread.Start();
            }
        }

        private void Update_Time()
        {
            while (run == true)
            {
                todaysTicks = DateTime.Now.Ticks;
                remaining = tick - todaysTicks;
                timeSpan = TimeSpan.FromTicks(remaining);
                DaysLeft.Dispatcher.Invoke(new Action(() => { DaysLeft.Text = "Days: " + timeSpan.Days.ToString(); }));
                HoursLeft.Dispatcher.Invoke(new Action(() => { HoursLeft.Text = "Hours: " + timeSpan.Hours.ToString(); }));
                MinutesLeft.Dispatcher.Invoke(new Action(() => { MinutesLeft.Text = "Minutes: " + timeSpan.Minutes.ToString(); }));
                SecondsLeft.Dispatcher.Invoke(new Action(() => { SecondsLeft.Text = "Seconds: " + timeSpan.Seconds.ToString(); }));
            }
            runfinished = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string temp = "";
            foreach (string item in fileText)
            {
                if (item != "IGNORE" && item != "")
                {
                    temp += item + "?";
                }
            }
            File.WriteAllText(taskFile, temp);
            run = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TaskMaker current = (TaskMaker)selector.SelectedItem;
            tasks.Remove(current);
            selector.ItemsSource = new List<Object>();
            selector.ItemsSource = tasks;
            string check = current.TaskName + "!" + current.TaskInfo + "!" + current.Date + "!" + current.Priority + "!" + current.AssignedBy + "!" + current.Status;
            int x = 0;
            foreach (string item in fileText)
            {
                if (item.Contains(check))
                {
                    fileText[x] = "IGNORE";
                    break;
                }
                x++;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            TaskMaker current = (TaskMaker)selector.SelectedItem;
            tasks.Remove(current);
            selector.ItemsSource = new List<Object>();
            selector.ItemsSource = tasks;
            string check = current.TaskName + "!" + current.TaskInfo + "!" + current.Date + "!" + current.Priority + "!" + current.AssignedBy + "!" + current.Status;
            int x = 0;
            foreach (string item in fileText)
            {
                if (item.Contains(check))
                {
                    fileText[x] = "IGNORE";
                    break;
                }
                x++;
            }
            NewTaskName.Text = current.TaskName;
            NewTaskDate.SelectedDate = current.Date;
            NewTaskInfo.Text = current.TaskInfo;
            NewTaskPriority.Text = current.Priority.ToString();
            NewTaskAssignment.Text = current.AssignedBy;
            NewTaskStatus.Text = current.Status;
        }

        private void Tasked_Click(object sender, RoutedEventArgs e)
        {
            CloseAll();
            Tasks.Visibility = Visibility.Visible;
            Cal.Visibility = Visibility.Visible;
            Stat.Visibility = Visibility.Visible;
            Curr.Visibility = Visibility.Visible;
            selector.Visibility = Visibility.Visible;
            Newer.Visibility = Visibility.Visible;
        }
    }
}
