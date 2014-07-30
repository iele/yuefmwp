using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Alexis.WindowsPhone.Social.Resources;

namespace Alexis.WindowsPhone.Social
{
    public partial class AccountControl : UserControl
    {
        public Action<SocialType> BindAction;
        public AccountControl()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(AccountControl_Loaded);
        }       

        void AccountControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent();
        }

        public void SetContent()
        {
            // Avoid handing off to the worker thread (can cause problems for design tools)
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }
            
            btnWeibo.Content = SocialAPI.WeiboAccessToken == null ? LangResource.BindWeibo: LangResource.UnBind;
            tbkWeibo.Text = SocialAPI.WeiboAccessToken == null ? LangResource.UnBinded : LangResource.Binded;

            btnTencent.Content = SocialAPI.TencentAccessToken == null ? LangResource.BindTencent: LangResource.UnBind;
            tbkTencent.Text = SocialAPI.TencentAccessToken == null ? LangResource.UnBinded : LangResource.Binded;

            btnDouban.Content = SocialAPI.DoubanAccessToken == null ? LangResource.BindDouban : LangResource.UnBind;
            tbkDouban.Text = SocialAPI.DoubanAccessToken == null ? LangResource.UnBinded : LangResource.Binded;
        }

        private void btnWeibo_Click(object sender, RoutedEventArgs e)
        {
            if (tbkWeibo.Text == LangResource.UnBinded)
            {
                BindAction(SocialType.Weibo);
            }
            else
            {
                SocialAPI.LogOff(SocialType.Weibo);
            }

            btnWeibo.Content = SocialAPI.WeiboAccessToken == null ? LangResource.BindWeibo : LangResource.UnBind;
            tbkWeibo.Text = SocialAPI.WeiboAccessToken == null ? LangResource.UnBinded : LangResource.Binded;
        }

        private void btnTencent_Click(object sender, RoutedEventArgs e)
        {
            if (tbkTencent.Text == LangResource.UnBinded)
            {
                BindAction(SocialType.Tencent);
            }
            else
            {
                SocialAPI.LogOff(SocialType.Tencent);
            }

            btnTencent.Content = SocialAPI.TencentAccessToken == null ? LangResource.BindTencent : LangResource.UnBind;
            tbkTencent.Text = SocialAPI.TencentAccessToken == null ? LangResource.UnBinded : LangResource.Binded;
        }

        private void btnDouban_Click(object sender, RoutedEventArgs e)
        {
            if (tbkDouban.Text == LangResource.UnBinded)
            {
                BindAction(SocialType.Douban);
            }
            else
            {
                SocialAPI.LogOff(SocialType.Douban);
            }

            btnDouban.Content = SocialAPI.DoubanAccessToken == null ? LangResource.BindDouban : LangResource.UnBind;
            tbkDouban.Text = SocialAPI.DoubanAccessToken == null ? LangResource.UnBinded : LangResource.Binded;
        }
    }
}
