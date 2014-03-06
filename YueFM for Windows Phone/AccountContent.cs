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
    public class AccountContent
    {
        public String name { get; set; }
        public String text { get; set; }

        public AccountContent(String name, String text)
        {
            this.name = name;
            this.text = text;
        }
    }
}
