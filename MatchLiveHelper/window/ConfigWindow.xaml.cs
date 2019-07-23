using MatchLiveHelper.util;
using ShowcaseHelper.util;
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


            //输出提示语
            Console.WriteLine("初始化完成！");
            WindowComboBox.IsEnabled = true;


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

        private void WindowSwitch(object sender, SelectionChangedEventArgs e)
        {

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

        private void TeamSwitch(object sender, SelectionChangedEventArgs e)
        {
            if (WindowComboBox.SelectedValue.)
        }
    }
}
