using MatchLiveHelper.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace MatchLiveHelper
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private const double MAPPOOL_IMAGE_WIDTH = 375;
        private const double MAPPOOL_IMAGE_HEIGHT = 50;
        private const double BANNED_OPACITY = 0.8;
        private const double UNCHECKED_OPACITY = 0.5;
        private MapPoolWindow mapPoolWindow = new MapPoolWindow();
        private MatchWindow matchWindow = new MatchWindow();
        private ScheduleWindow scheduleWindow = new ScheduleWindow();
        private SongInfoWindow songInfoWindow = new SongInfoWindow();
        private const string MATCH_WINDOW = "对阵界面";
        private const string MAP_POOL_WINDOW = "图池界面";
        private const string SCHEDULE_WINDOW = "赛程界面";
        private const string SONG_INFO_WINDOW = "歌曲界面";
        private const string RED_TEAM = "红队";
        private const string BLUE_TEAM = "蓝队";

        private Window activeWindow;




        public ConfigWindow()
        {
            InitializeComponent();

        }
        private void WindowClose(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Init(object sender, EventArgs e)
        {

            //禁用所有组件

            WindowComboBox.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
            RedTeamRadio.IsEnabled = false;
            BlueTeamRadio.IsEnabled = false;
            ResetButton.IsEnabled = false;
            BanRadio.IsEnabled = false;
            PickRadio.IsEnabled = false;

            //设置控制台输出到TextBox
            Console.SetOut(new TextBoxStreamWriter(ConsoleTextBox));

            //读取data目录下的比赛图标、队伍图标、配置文件、赛程图片、图池表格
            ReadData();

            //初始化对阵界面
            matchWindow.Show();
            activeWindow = matchWindow;

            //输出提示语
            Console.WriteLine("初始化完成！");
            WindowComboBox.IsEnabled = true;





        }



        private void WindowSwitch(object sender, SelectionChangedEventArgs e)
        {
            //处理解析XAML时，在Combox添加项目进而触发本方法的情况
            if (activeWindow == null)
            {
                return;
            }
            activeWindow.Visibility = Visibility.Hidden;

            ConfirmButton.IsEnabled = false;
            RedTeamRadio.IsEnabled = false;
            BlueTeamRadio.IsEnabled = false;
            ResetButton.IsEnabled = false;
            BanRadio.IsEnabled = false;
            PickRadio.IsEnabled = false;

            switch (WindowComboBox.SelectedValue.ToString())
            {
                case MATCH_WINDOW:
                    activeWindow = matchWindow;
                    break;
                case SCHEDULE_WINDOW:
                    activeWindow = scheduleWindow;
                    break;
                case MAP_POOL_WINDOW:
                    ConfirmButton.IsEnabled = true;
                    RedTeamRadio.IsEnabled = true;
                    BlueTeamRadio.IsEnabled = true;
                    ResetButton.IsEnabled = true;
                    BanRadio.IsEnabled = true;
                    PickRadio.IsEnabled = true;
                    activeWindow = mapPoolWindow;
                    break;
                case SONG_INFO_WINDOW:
                    activeWindow = songInfoWindow;
                    break;
                default:
                    return;

            }
            activeWindow.Show();

        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void OptimizeWindow(object sender, EventArgs e)
        {

            //取消osu!小窗口置顶，并且将小窗口绑定到管理器大窗口
            TournamentModifyUtil.CancelOsuTopMost();


        }

        private void MapPoolOperation(object sender, RoutedEventArgs e)
        {
            if (Constant.CurrentGrid == null)
            {
                Console.WriteLine("你没有选择谱面！");
                return;
            }
            if (Constant.CurrentGrid.Children[Constant.CurrentGrid.Children.Count - 1] is Rectangle)
            {
                Console.WriteLine("请不要重复操作！");
                return;
            }
            Rectangle rectangle = new Rectangle();
            rectangle.RadiusX = 25;
            rectangle.RadiusY = 25;
            rectangle.StrokeThickness = 5;
            rectangle.Height = MAPPOOL_IMAGE_HEIGHT;
            rectangle.Width = (Constant.CurrentGrid.Children[1] as Rectangle).Width;
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Top;

            if (BlueTeamRadio.IsChecked == true)
            {
                rectangle.Stroke = Brushes.Blue;
                BlueTeamRadio.IsChecked = false;
                RedTeamRadio.IsChecked = true;
            }
            else
            {
                BlueTeamRadio.IsChecked = true;
                RedTeamRadio.IsChecked = false;
                rectangle.Stroke = Brushes.Red;
            }


            if (BanRadio.IsChecked == true)
            {
                rectangle.Fill = new SolidColorBrush(Colors.Gray);
                rectangle.Opacity = BANNED_OPACITY;
            }

            (Constant.CurrentGrid.Children[1] as Rectangle).Opacity = UNCHECKED_OPACITY;
            Constant.CurrentGrid.Children.Add(rectangle);

            if (PickRadio.IsChecked == true)
            {
                (songInfoWindow.Content as Grid).Children.Clear();
                string gridXaml = XamlWriter.Save(Constant.CurrentGrid);
                StringReader stringReader = new StringReader(gridXaml);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                Grid newGrid = (Grid)XamlReader.Load(xmlReader);
                newGrid.Width = 300;
                (newGrid.Children[0] as Rectangle).Width = 300;
                (newGrid.Children[1] as Rectangle).Width = 300;
                (newGrid.Children[5] as Rectangle).Width = 300;
                newGrid.Margin = new Thickness(980, 660, 0, 0);
                (songInfoWindow.Content as Grid).Children.Add(newGrid);

            }

            
            Constant.CurrentGrid = null;
        }


        private void MapPoolStatusReset(object sender, RoutedEventArgs e)
        {
            if (Constant.CurrentGrid == null)
            {
                Console.WriteLine("你没有选择谱面！");
            }
            if (Constant.CurrentGrid.Children[Constant.CurrentGrid.Children.Count - 1] is Rectangle)
            {

                Constant.CurrentGrid.Children.RemoveAt(Constant.CurrentGrid.Children.Count - 1);
            }

        }

        private void ReadData()
        {

            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\data", "*.*", SearchOption.AllDirectories);
            var fileList = new List<string>(files);
            if (fileList.BinarySearch(Environment.CurrentDirectory + "\\data\\config.ini") < 0)
            {
                FileStream fs = new FileStream(Environment.CurrentDirectory + "\\data\\config.ini", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write("[Match]\r\n" +
                    "Stage = 小组赛\r\n" +
                    "Time = 2019-7-22 19:43\r\n" +
                    "[Blue]\r\n" +
                    "Name = UNITED KINGDOM\r\n" +
                    "Player = Bubbleman,Doomsday\r\n" +
                    "[Red]\r\n" +
                    "Name = UNITED STATES\r\n" +
                    "Player = Apraxia,Toy\r\n"
                );
                sw.Flush();
                sw.Close();
                fs.Close();

            }
            fileList.ForEach(file =>
            {
                if (file.ToLower().EndsWith("match.png"))
                {
                    Constant.MatchLogo = LoadImage(file);
                }
                if (file.ToLower().EndsWith("red_logo.png"))
                {
                    Constant.RedLogo = LoadImage(file);
                }
                if (file.ToLower().EndsWith("red_logo.png"))
                {
                    Constant.BlueLogo = LoadImage(file);
                }
                if (file.ToLower().EndsWith("schedule.png"))
                {
                    Constant.Schedule = LoadImage(file);
                }
                if (file.ToLower().EndsWith("xls") || file.ToLower().EndsWith("xlsx"))
                {
                    Constant.MapPoolSet = Mp5MapPoolReadUtil.Read(file);
                }

            });
            Constant.Stage = IniUtil.ReadIniData("Match", "Stage");
            Constant.Time = IniUtil.ReadIniData("Match", "Time");

            Constant.BlueTeamName = (IniUtil.ReadIniData("Blue", "Name"));
            Constant.RedTeamName = (IniUtil.ReadIniData("Red", "Name"));

            Constant.BluePlayers = (IniUtil.ReadIniData("Blue", "Player"));
            Constant.RedPlayers = (IniUtil.ReadIniData("Red", "Player"));

            //根据图池表格中的谱面ID读取子文件夹下的图片文件
            fileList.ForEach(file =>
            {
                Constant.MapPoolSet.Pool.ForEach(
                    pool =>
                    {
                        pool.Map.ForEach(
                            map =>
                            {
                                if (file.ToLower().EndsWith(map.BeatmapId + ".png"))
                                {
                                    Constant.BgImages.Add(map.BeatmapId, LoadImage(file));
                                }
                            }
                            );
                    }
                    );

            }
            );

        }

        private BitmapImage LoadImage(string file)
        {
            var bitmapImage1 = new BitmapImage();
            bitmapImage1.BeginInit();
            bitmapImage1.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage1.UriSource = new Uri(file);
            bitmapImage1.EndInit();
            bitmapImage1.Freeze();
            return bitmapImage1;
        }


    }
}
