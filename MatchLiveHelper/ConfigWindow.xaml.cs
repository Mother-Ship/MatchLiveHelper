using MatchLiveHelper.util;
using ShowcaseHelper.entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace MatchLiveHelper
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {

        public ConfigWindow()
        {
            InitializeComponent();
            var mw = new MainWindow();
            mw.Show();
            switchBtn.IsEnabled = false;
            redBanBtn.IsEnabled = false;
            redPickBtn.IsEnabled = false;
            blueBanBtn.IsEnabled = false;
            bluePickBtn.IsEnabled = false;
            redWarmUpBtn.IsEnabled = false;
            bluedWarmUpBtn.IsEnabled = false;
        }

        private void windowLoad(object sender, EventArgs e)
        {
            Console.SetOut(new TextBoxStreamWriter(consoleTextBox));
            Console.WriteLine("赛事助手启动成功，请选择图池表格文件");
        }

        private void windowClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void onSelectMapPool(object sender, EventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel 表格 (*.xls/*.xlsx)|*.xls;*.xlsx"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                MapPoolSet set = Mp5MapPoolReadUtil.read(openFileDialog.FileName);
                if (set != null)
                {
                    Console.WriteLine("图池加载成功——" + set.PoolName + "，正在等待osu!启动...");
                    MainWindow.MapPoolSet = set;
                }
            }
            /**
             * 由于UI更新在主线程，而获取osu!进程状态需要另起线程并阻塞线程直到获取成功，进而调用主线程方法更新UI
             * 如果由主线程直接调用获取状态的线程，会导致UI无法更新
             * 因此新建线程，让被阻塞的线程是这个新建的线程
             */
            new Thread(new ThreadStart(() => readOsuInfo())).Start();

        }

        private void readOsuInfo()
        {
            //获取osu!文件夹
            string osuPath = "";
            Thread t = new Thread(new ThreadStart(() =>
            {
                while (osuPath.Equals(""))
                {
                    Process[] ps = Process.GetProcesses();
                    foreach (Process p in ps)
                    {
                        if (p.ProcessName.Equals("osu!"))
                        {
                            osuPath = p.MainModule.FileName;
                            osuPath = osuPath.Substring(0, osuPath.LastIndexOf("\\"));
                            Console.WriteLine("检测到osu!启动！,进程ID："+p.Id);
                            break;
                        }
                    }
                    if (osuPath.Equals(""))
                    {
                        Thread.Sleep(1000);
                    }
                }
            }));
            t.Start();
            t.Join();
            //检测到osu!进程后，暂时不允许更换图池文件
            switchBtn.Dispatcher.Invoke(new Action(() => switchBtn.IsEnabled = false));
            //检查图池完整性
            string[] files = Directory.GetFiles(@osuPath+"\\Songs", "*.*", SearchOption.AllDirectories);
            var filesList = new List<string>();
            foreach (string file in files)
            {
                if (file.EndsWith(".osu")) {
                    filesList.Add(file);
                }
            }
            MainWindow.MapPoolSet.Pool.ForEach(mapPool => {
                mapPool.Map.ForEach(map =>
                {
                    filesList.ForEach(file =>
                    {

                    });
                });
            });
            //获取正在播放的歌曲信息

        }

     
    }
}
