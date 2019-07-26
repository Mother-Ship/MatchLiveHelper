using MatchLiveHelper.entity;
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

namespace MatchLiveHelper
{
    /// <summary>
    /// ScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ScheduleWindow()
        {
            InitializeComponent();
        }
        public void Init(object sender, EventArgs e) {
            schedule.Source = Constant.Schedule;
            schedule.Margin = new Thickness(0, 28, 0, 0);
            
            logo.Source = Constant.MatchLogo;
            logo.Width = Constant.MatchLogo.Width;
            logo.Height = Constant.MatchLogo.Height;
            
           
        }
        
    }
}
