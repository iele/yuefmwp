﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Alexis.WindowsPhone.Social
{
    public partial class AuthControl : UserControl
    {
        public Action<AccessToken> action;
        private SocialType currentType= SocialType.Weibo;
        private ClientInfo client;

        /// <summary>
        /// 社交平台登录授权控件
        /// 需要SetData()设置当前社交平台与相应的key信息
        /// </summary>
        public AuthControl()
        {
            InitializeComponent();
        }

        public void SetData(SocialType type, ClientInfo client)
        {
            string path = "";
            if (type == SocialType.Weibo)
            {
                path = "/Alexis.WindowsPhone.Social;component/Images/weibo.png";
            }
            else if (type == SocialType.Tencent)
            {
                path = "/Alexis.WindowsPhone.Social;component/Images/tencent.png";
            }
            else if (type == SocialType.Douban)
            {
                path = "/Alexis.WindowsPhone.Social;component/Images/douban.png";
            }
            this.imgLogo.Source = new BitmapImage { UriSource = new Uri(path, UriKind.Relative) };
            SocialAPI.Client = client;
            this.client = client;
            currentType = type;
            webbrowser.Source = new Uri(SocialKit.GetAuthorizeUrl(currentType, client), UriKind.Absolute);
        }

        private void BrowserNavigating(object sender, NavigatingEventArgs e)
        {
            if (!e.Uri.AbsoluteUri.Contains("code=") && !e.Uri.AbsoluteUri.Contains("code ="))
            {
                return;
            }            
            var arguments = e.Uri.AbsoluteUri.Split('?');
            string code  = arguments[1]; 
            if (currentType== SocialType.Tencent)
            {
                var sp = arguments[1].Split('&');
                code = sp[0];

                //open id
                client.Tag = sp[1].Split('=')[1]; 
            }          
            
            SocialKit.GetToken(currentType, client, code, (p) =>
            {
                action(p);
            });
        }

        private void BrowserLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            grdLoading.Visibility = System.Windows.Visibility.Collapsed;
            try
            {
                webbrowser.InvokeScript("eval",
                @"
                window.ScanTelTag=function(elem) {
                if (elem.getAttribute('target') != null && elem.getAttribute('target').indexOf('_parent') == 0) {
                    elem.setAttribute('target', '_self');
                    }
                }
            
                window.Initialize=function() {
                var elems = document.getElementsByTagName('a');
                for (var i = 0; i < elems.length; i++)
                ScanTelTag(elems[i]);
                }");
                webbrowser.InvokeScript("Initialize");
            }
            catch (Exception)
            {
            }
        }
    }
}
