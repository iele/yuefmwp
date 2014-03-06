using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace YueFM.Contents
{

    public class LikeContent
    {
        private DateTime date_time;
        private String date_string;

        public String date_liked  {
            get {
                return date_string;
            }
            set {
                DateTime dt = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dt = dt.ToLocalTime();
                date_time = dt;
                date_string = "添加于 " + date_time.ToString();
            }
        }
        public String article_id { get; set; }
        public String article_title { get; set; }
    }
}
