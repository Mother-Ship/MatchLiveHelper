using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MatchLiveHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapPoolWindow : Window
    {
        private const double WINDOW_WIDTH = 1280;
        private const double MAPPOOL_IMAGE_WIDTH = 375;
        private const double MAPPOOL_TB_IMAGE_WIDTH = 450;
        private const double MAPPOOL_IMAGE_HEIGHT = 50;
        private const double MAPPOOL_IMAGE_VERTICAL_SPACE = 10;
        private const double MAPPOOL_IMAGE_HORIZONAL_SPACE = 17;
        private const double CHECKED_OPACITY = 0.2;
        private const double UNCHECKED_OPACITY = 0.5;
        public MapPoolWindow()
        {
            InitializeComponent();
        }
        public void Init(object sender, EventArgs e)
        {


            //根据图池是否需要换行，以及是否是下一个MOD图池来切换纵坐标
            double y = 130;

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

                    end = count <= 3;

                    switch (count)
                    {
                        case 1:
                            if (pool.Type.Equals("Tiebreaker"))
                            {
                                x = (WINDOW_WIDTH - MAPPOOL_TB_IMAGE_WIDTH) / 2;
                            }
                            else
                            {
                                x = (WINDOW_WIDTH - MAPPOOL_IMAGE_WIDTH) / 2;
                            }
                          
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
                            y += MAPPOOL_IMAGE_HORIZONAL_SPACE + MAPPOOL_IMAGE_HEIGHT;
                            break;

                    }


                }
                //每种图池之间空一行
                y += MAPPOOL_IMAGE_HORIZONAL_SPACE + MAPPOOL_IMAGE_HEIGHT;
            }
            );


            try
            {
                int i = 0;
                Constant.MapPoolSet.Pool.ForEach(
                    pool =>
                    {
                        double width;
                        if (pool.Type.Equals("Tiebreaker"))
                        {
                            width = MAPPOOL_TB_IMAGE_WIDTH;
                        }
                        else
                        {
                            width = MAPPOOL_IMAGE_WIDTH;
                        }
                        pool.Map.ForEach(
                            map =>
                            {

                                Grid nodeGrid = new Grid();

                                ImageBrush image = new ImageBrush();
                                image.ImageSource = Constant.BgImages[map.BeatmapId];
                                image.Stretch = Stretch.UniformToFill;

                                var point = coordinate[i];
                                Thickness th = new Thickness(point.X, point.Y, 0, 0);

                                Rectangle rectangle = new Rectangle();
                                rectangle.RadiusX = 25;
                                rectangle.RadiusY = 25;
                                rectangle.Fill = image;
                                rectangle.Height = MAPPOOL_IMAGE_HEIGHT;
                                rectangle.Width = width;
                                rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                                rectangle.VerticalAlignment = VerticalAlignment.Top;

                                Rectangle semiTransparent = new Rectangle();
                                semiTransparent.RadiusX = 25;
                                semiTransparent.RadiusY = 25;
                                semiTransparent.Fill = new SolidColorBrush(Colors.Black); ;
                                semiTransparent.Height = MAPPOOL_IMAGE_HEIGHT;
                                semiTransparent.Width = width;
                                semiTransparent.HorizontalAlignment = HorizontalAlignment.Left;
                                semiTransparent.VerticalAlignment = VerticalAlignment.Top;
                                semiTransparent.Opacity = UNCHECKED_OPACITY;

                                TextBlock title = new TextBlock();
                                title.Foreground = new SolidColorBrush(Colors.White);
                                title.Inlines.Add(new Bold(new Italic(new Run(map.Title))));
                                title.HorizontalAlignment = HorizontalAlignment.Center;
                                title.VerticalAlignment = VerticalAlignment.Top;
                                title.TextAlignment = TextAlignment.Center;
                                title.Margin = new Thickness(0, 9, 0, 0);
                                title.FontFamily =  new FontFamily("等线");

                                TextBlock mapperAndDifficulty = new TextBlock();
                                mapperAndDifficulty.Foreground = new SolidColorBrush(Colors.White);
                                mapperAndDifficulty.Inlines.Add(new Italic(new Run("mapper ")));
                                mapperAndDifficulty.Inlines.Add(new Bold(new Italic(new Run(map.Mapper))));

                                mapperAndDifficulty.Inlines.Add(new Italic(new Run("    difficulty ")));
                                mapperAndDifficulty.Inlines.Add(new Bold(new Italic(new Run(map.Difficulty))));

                                mapperAndDifficulty.HorizontalAlignment = HorizontalAlignment.Center;
                                mapperAndDifficulty.VerticalAlignment = VerticalAlignment.Top;
                                mapperAndDifficulty.TextAlignment = TextAlignment.Center;
                                mapperAndDifficulty.Margin = new Thickness(0, 28, 0, 0);


                               
                                nodeGrid.Height = MAPPOOL_IMAGE_HEIGHT;
                                nodeGrid.Width = width;
                                nodeGrid.Margin = th;
                                nodeGrid.HorizontalAlignment = HorizontalAlignment.Left;
                                nodeGrid.VerticalAlignment = VerticalAlignment.Top;


                                Image img = new Image();
                                switch (pool.Type) {
                                    case "No Mod":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/NM.png"));
                                        break;
                                    case "Force Mod":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/FM.png"));
                                        break;
                                    case "Free Mod":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/FM.png"));
                                        break;
                                    case "Hard Rock":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/HR.png"));
                                        break;
                                    case "Hidden":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/HD.png"));
                                        break;
                                    case "Double Time":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/DT.png"));
                                        break;
                                    case "Tiebreaker":
                                        img.Source = new BitmapImage(new Uri("pack://application:,,,/Image/TB.png"));
                                        break;

                                }

                                img.Width = 40;
                                img.Height = 20;
                                img.Margin = new Thickness(width-55, 0, 0, 0);

                                nodeGrid.Children.Add(rectangle);
                                nodeGrid.Children.Add(semiTransparent);
                                nodeGrid.Children.Add(img);
                                nodeGrid.Children.Add(title);
                                nodeGrid.Children.Add(mapperAndDifficulty);
                                nodeGrid.MouseDown += MapPoolChosen;
                                
                               (Content as Grid).Children.Add(nodeGrid);

                                i++;

                            });
                    });
            }
            catch (KeyNotFoundException)
            {
                (Content as Grid).Children.Clear();
                Console.WriteLine("图池缩略图与表格不符，请检查");
                return;
            }
            


            logo.Source = Constant.MatchLogo;
            logo.Width = Constant.MatchLogo.Width;
            logo.Height = Constant.MatchLogo.Height;

        }

        private void MapPoolChosen(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < (Content as Grid).Children.Count; i++)
            {
                if ((Content as Grid).Children[i] is Grid)
                {
                    (((Content as Grid).Children[i] as Grid).Children[1] as Rectangle).Opacity = UNCHECKED_OPACITY;
                }

            }


            ((sender as Grid).Children[1] as Rectangle).Opacity = CHECKED_OPACITY;
            Constant.CurrentGrid = (sender as Grid);


        }



    }
}



