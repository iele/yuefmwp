using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using YueFM.Managers;

namespace YueFM.Controls
{
    public partial class FooterControl : UserControl
    {
        public FooterControl(int likes)
        {
            InitializeComponent();

            this.likesText.FontSize = double.Parse(SettingManager.GetInstance().article_size) - 2;
            this.likesText.Text = "有" + likes.ToString() + "人推荐";
        }
    }
}
