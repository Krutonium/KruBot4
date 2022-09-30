using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;

namespace KruBot4
{
    public static class Helpers
    {
        public static BitmapImage getBadge(string Url, int size)
        {
            var sourceImage = new BitmapImage();
            sourceImage.BeginInit();
            sourceImage.UriSource = new Uri(Url + size);
            sourceImage.EndInit();
            return sourceImage;
        }
    }
    public static class Badges
    {
        public static string Broadcaster = ("https://static-cdn.jtvnw.net/badges/v1/5527c58c-fb7d-422d-b71b-f309dcb85cc1/");
        public static string Subscriber = ("https://static-cdn.jtvnw.net/badges/v1/5d9f2208-5dd8-11e7-8513-2ff4adfae661/");
        public static string Moderator = ("https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/");
    }
}
