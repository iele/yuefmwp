using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace YueFM.Controls
{
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();
        }

        public void SetTitleSize(bool narrow)
        {
            title.Width = 340;
        }


        public void SetTitle(string title,string src, int size)
        {
            this.title.Text = title;
            this.source.Text = "来源：" + src;
            this.title.FontSize = size + 4;
            this.source.FontSize = size;
        }
    }
}
