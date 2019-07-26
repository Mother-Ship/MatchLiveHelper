using MatchLiveHelper.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MatchLiveHelper
{
    public class Constant
    {
        public static BitmapImage MatchLogo { get; set; }
        public static BitmapImage RedLogo { get; set; }
        public static BitmapImage BlueLogo { get; set; }
        public static BitmapImage Schedule { get; set; }
        public static string Time { get; set; }
        public static string Stage { get; set; }
        public static string BluePlayers { get; set; }
        public static string RedPlayers { get; set; }
        
        public static Grid CurrentGrid { get; set; }
        public static MapPoolSet MapPoolSet { get; set; }
        public static Dictionary<string, BitmapImage> BgImages { get; set; } = new Dictionary<string, BitmapImage>();
        public static string BlueTeamName { get;  set; }
        public static string RedTeamName { get; set; }
    }
}
