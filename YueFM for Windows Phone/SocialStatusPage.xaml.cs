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
using Microsoft.Phone.Controls;
using Alexis.WindowsPhone.Social;
using System.Windows.Navigation;
using YueFM.Utils;

namespace YueFM.Pages
{
    public partial class SocialStatusPage : PhoneApplicationPage
    {
        private Action<bool, Exception> action;
        
        private String type, share;

        public SocialStatusPage()
        {
            InitializeComponent();

            if (ResolutionUtils.CurrentResolution == Resolutions.HD720p)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        this.button.Margin = new Thickness(12, 480 + 53, 12, 12);
                        UpdateLayout();
                    });
            }

            AppUtils.FlurryLog("SocialShare");            
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e) 
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.TryGetValue("type", out type)
                && NavigationContext.QueryString.TryGetValue("share", out share))
            {
                switch (type)
                {
                    case "weibo":
                        this.ApplicationTitle.Text = "分享至新浪微博";
                        break;
                    case "douban":
                        this.ApplicationTitle.Text = "分享至豆瓣网";
                        break;
                    case "tencent":
                        this.ApplicationTitle.Text = "分享至腾讯微博";
                        break;
                    default:
                        break;
                }

                this.textBox.Text = share;
            }
            else
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.button.IsEnabled = false;
            this.button.Content = "正在发送..";

            action = new Action<bool, Exception>((b, ex) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (b)
                        AppUtils.ToastPromptShow("阅FM", "文章分享成功~");
                    else
                        AppUtils.ToastPromptShow("阅FM", "文章分享失败，如重复出现，请尝试重新登录");
                });
            });

            switch (type)
            {
                case "weibo":
                    SocialAPI.Client = SocialUtils.GetClient(SocialType.Weibo);
                    SocialAPI.UpdateStatus(SocialType.Weibo, share, action);
                    break;
                case "douban":
                    SocialAPI.Client = SocialUtils.GetClient(SocialType.Douban);
                    SocialAPI.UpdateStatus(SocialType.Douban, share, action);
                    break;
                case "tencent":
                    SocialAPI.Client = SocialUtils.GetClient(SocialType.Tencent);
                    SocialAPI.UpdateStatus(SocialType.Tencent, share, action);
                    break;
                default:
                    break;
            }

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}