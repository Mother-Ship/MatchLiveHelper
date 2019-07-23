using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MatchLiveHelper
{
    /// <summary>
    /// MatchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MatchWindow : Window
    {
        public MatchWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            logo.Source = Constant.MatchLogo;
            logo.Width = Constant.MatchLogo.Width;
            logo.Height = Constant.MatchLogo.Height;

            RedLogo.Source = Constant.RedLogo;

            BlueLogo.Source = Constant.BlueLogo;


            RedPlayers.Content = Constant.RedPlayers.Replace(",", "\r\n");
            BluePlayers.Content = Constant.BluePlayers.Replace(",", "\r\n");
            RedTeamName.Content = Constant.RedTeamName;
            BlueTeamName.Content = Constant.BlueTeamName;
            Time.Content = Constant.Time;
            Stage.Content = Constant.Stage;
            RotateBlue();
            RotateRed();
            Show();
        }

        public void RotateBlue()
        {
            Storyboard storyboard = new Storyboard();//创建故事板
            DoubleAnimation doubleAnimation = new DoubleAnimation();//实例化一个Double类型的动画
            RotateTransform rotate = new RotateTransform();//旋转转换实例
            this.BlueCircle.RenderTransform = rotate;//给图片空间一个转换的实例
            this.BlueCircle.RenderTransformOrigin = new Point(0.5, 0.5);//围绕中心旋转
            storyboard.RepeatBehavior = RepeatBehavior.Forever;//设置重复为 一直重复
            storyboard.SpeedRatio = 0.2;//播放的数度
            //设置从0 旋转360度
            doubleAnimation.From = 0;
            doubleAnimation.To = 360;
            doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 2));//播放时间长度为2秒
            Storyboard.SetTarget(doubleAnimation, this.BlueCircle);//给动画指定对象
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Angle"));//给动画指定依赖的属性
            storyboard.Children.Add(doubleAnimation);//将动画添加到动画板中
            storyboard.Begin(this.BlueCircle);//启动动画


        }
        public void RotateRed()
        {
            Storyboard storyboard = new Storyboard();//创建故事板
            DoubleAnimation doubleAnimation = new DoubleAnimation();//实例化一个Double类型的动画
            RotateTransform rotate = new RotateTransform();//旋转转换实例
            this.RedCircle.RenderTransform = rotate;//给图片空间一个转换的实例
            this.RedCircle.RenderTransformOrigin = new Point(0.5, 0.5);//围绕中心旋转
            storyboard.RepeatBehavior = RepeatBehavior.Forever;//设置重复为 一直重复
            storyboard.SpeedRatio = 0.2;//播放的数度
            //设置从0 旋转360度
            doubleAnimation.From = 0;
            doubleAnimation.To = 360;
            doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 2));//播放时间长度为2秒
            Storyboard.SetTarget(doubleAnimation, this.RedCircle);//给动画指定对象
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Angle"));//给动画指定依赖的属性
            storyboard.Children.Add(doubleAnimation);//将动画添加到动画板中
            storyboard.Begin(this.RedCircle);//启动动画


        }
    }
}
