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

namespace YueFM.Contents
{
    public class ArticleContent
    {
        public String body {get; set; }
        public String title { get; set; }
        public String source { get; set; }
        public int likes { get; set; }
        public String date_created { get; set; }
        public int int_id { get; set; }
        public String id { get; set; }
        public String short_id { get; set;}
        public Boolean is_liked{ get; set;}
    }
}
