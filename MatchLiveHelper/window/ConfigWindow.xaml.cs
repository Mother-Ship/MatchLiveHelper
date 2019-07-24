using MatchLiveHelper.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MatchLiveHelper
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private MapPoolWindow mapPoolWindow = new MapPoolWindow();
        private MatchWindow matchWindow = new MatchWindow();
        private ScheduleWindow scheduleWindow = new ScheduleWindow();
        private SongInfoWindow songInfoWindow = new SongInfoWindow();
        private const string MATCH_WINDOW = "对阵界面";
        private const string MAP_POOL_WINDOW = "图池界面";
        private const string SCHEDULE_WINDOW = "赛程界面";
        private const string SONG_INFO_WINDOW = "歌曲界面";

        private Window activeWindow;

        private const double WINDOW_WIDTH = 1280;
        private const double MAPPOOL_IMAGE_WIDTH = 375;
        private const double MAPPOOL_IMAGE_HEIGHT = 50;
        private const double MAPPOOL_IMAGE_VERTICAL_SPACE = 10;
        private const double MAPPOOL_IMAGE_HORIZONAL_SPACE = 18;


        public ConfigWindow()
        {
            InitializeComponent();

        }
        private void WindowClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void WindowLoad(object sender, EventArgs e)
        {

            //禁用所有组件
            ConfirmButton.IsEnabled = false;
            WindowComboBox.IsEnabled = false;
            TeamComboBox.IsEnabled = false;
            ResetButton.IsEnabled = false;
            BanRadio.IsEnabled = false;
            PickRadio.IsEnabled = false;

            //设置控制台输出到TextBox
            Console.SetOut(new TextBoxStreamWriter(ConsoleTextBox));

            //读取data目录下的比赛图标、队伍图标、配置文件、赛程图片、图池表格
            ReadData();

            //初始化对阵界面
            matchWindow.Init();
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

            switch (WindowComboBox.SelectedValue.ToString())
            {
                case MATCH_WINDOW:
                    matchWindow.Init();
                    activeWindow = matchWindow;
                    break;
                case MAP_POOL_WINDOW:
                    //根据图池是否需要换行，以及是否是下一个MOD图池来切换纵坐标
                    double y = 140;

                    //根据图池谱面数量不同，计算出水平居中的横坐标
                    double x;

                    var coordinate = new List<Point>();

                    Constant.MapPoolSet.Pool.ForEach(
                    pool =>
                    {

                        bool end = false;
                        int count = pool.Map.Count;
                        while (!end)
                        {

                            end = count < 3;

                            switch (count)
                            {
                                case 1:

                                    x = (WINDOW_WIDTH - MAPPOOL_IMAGE_WIDTH) / 2;
                                    coordinate.Add(new Point(x, y));
                                    break;
                                case 2:

                                    x = WINDOW_WIDTH / 2 - MAPPOOL_IMAGE_WIDTH - MAPPOOL_IMAGE_HORIZONAL_SPACE / 2;
                                    coordinate.Add(new Point(x, y));

                                    x += MAPPOOL_IMAGE_WIDTH + MAPPOOL_IMAGE_HORIZONAL_SPACE;
                                    coordinate.Add(new Point(x, y));
                                    break;
                                case 3:
                                    x = (WINDOW_WIDTH - MAPPOOL_IMAGE_WIDTH) / 2 - MAPPOOL_IMAGE_HORIZONAL_SPACE - MAPPOOL_IMAGE_WIDTH;
                                    coordinate.Add(new Point(x, y));

                                    x += MAPPOOL_IMAGE_WIDTH + MAPPOOL_IMAGE_HORIZONAL_SPACE;
                                    coordinate.Add(new Point(x, y));

                                    x += MAPPOOL_IMAGE_WIDTH + MAPPOOL_IMAGE_HORIZONAL_SPACE;
                                    coordinate.Add(new Point(x, y));
                                    break;
                                default:
                                    x = (WINDOW_WIDTH - MAPPOOL_IMAGE_WIDTH) / 2 - MAPPOOL_IMAGE_HORIZONAL_SPACE - MAPPOOL_IMAGE_WIDTH;
                                    coordinate.Add(new Point(x, y));

                                    x += MAPPOOL_IMAGE_WIDTH + MAPPOOL_IMAGE_HORIZONAL_SPACE;
                                    coordinate.Add(new Point(x, y));

                                    x += MAPPOOL_IMAGE_WIDTH + MAPPOOL_IMAGE_HORIZONAL_SPACE;
                                    coordinate.Add(new Point(x, y));

                                    count -= 3;
                                    y += MAPPOOL_IMAGE_VERTICAL_SPACE;
                                    break;

                            }


                        }
                        //每种图池之间空一行
                        y += MAPPOOL_IMAGE_VERTICAL_SPACE;
                    }
                    );
                    int i = 0;
                    Constant.MapPoolSet.Pool.ForEach(
                    pool =>
                    {
                        pool.Map.ForEach(
                            map=> {
                                Image img = new Image();
                                img.Height = MAPPOOL_IMAGE_HEIGHT;
                                img.Width = MAPPOOL_IMAGE_WIDTH;
                                img.Visibility = Visibility.Visible;
                                var point = coordinate[i];
                                Thickness th = new Thickness(point.X, point.Y, 0, 0);
                                img.Margin = th;


                                img.Source = Constant.BgImages[map.BeatmapId];
                                (this.Content as Grid).Children.Add(img);

                                i++;
                               
                            });
                    });
                    coordinate.ForEach(item => Console.WriteLine(item.X + " " + item.Y));
                    break;
                case SCHEDULE_WINDOW:

                    break;
                case SONG_INFO_WINDOW:

                    break;
                default:
                    return;

            }
        }




        private void MapPoolOperation(object sender, RoutedEventArgs e)
        {
            switch (WindowComboBox.SelectedItem.ToString())
            {
                case MAP_POOL_WINDOW:

                    break;
                default:
                    return;

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
                                    Constant.BgImages.Add(file, LoadImage(file));
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
