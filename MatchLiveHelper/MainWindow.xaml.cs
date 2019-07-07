using ShowcaseHelper.entity;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MapPoolSet mapPoolSet;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        internal static MapPoolSet MapPoolSet { get => mapPoolSet; set => mapPoolSet = value; }
    }
}
