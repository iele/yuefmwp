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
using YueFM.Contents;
using Microsoft.Phone.Tasks;
using YueFM.Managers;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using YueFM.Utils;

namespace YueFM.Controls
{
    public partial class SocialSendControl : UserControl
    {
        private List<AccountContent> list = new List<AccountContent>();

        private Popup SharePopup;

        public Action action;

        public void CreatePopup()
        {

            if (ResolutionUtils.CurrentResolution == Resolutions.HD720p)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    this.LayoutRoot.Height += 53;
                    UpdateLayout();
                });
            }

            try
            {
                this.listBox.SelectionChanged -= listBox_SelectionChanged;
            }
            catch (Exception)
            { 
            }

            list = new List<AccountContent>();

            if (SocialAPI.WeiboAccessToken != null)
            {
                list.Add(new AccountContent("weibo", "新浪微博"));
            }
            if (SocialAPI.DoubanAccessToken != null)
            {
                list.Add(new AccountContent("douban", "豆瓣网"));
            }
            if (SocialAPI.TencentAccessToken != null)
            {
                list.Add(new AccountContent("tencent", "腾讯微博"));
            }

            list.Add(new AccountContent("mail", "电子邮件"));
            list.Add(new AccountContent("sms", "短信"));
            list.Add(new AccountContent("other", "其他"));

            listBox.ItemsSource = list;
            this.listBox.SelectionChanged += listBox_SelectionChanged;

            SharePopup.IsOpen = true;

            LayoutRootInStoryboard.Begin();
        }

        public void HidePopup()
        {
            LayoutRootOutStoryboard.Begin();
        }

        public SocialSendControl()
        {
            InitializeComponent();

            LayoutRoot.RenderTransform = this.LayoutRootTranslateTransform;

            SharePopup = new Popup();
            SharePopup.Child = this;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void LayoutRootInStoryboard_Completed(object sender, EventArgs e)
        {

        }

        private void LayoutRootOutStoryboard_Completed(object sender, EventArgs e)
        {
            SharePopup.IsOpen = false;
        }

        private void AccountItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (sender as AccountItem).SelectedItem(new Action(() =>
            {
                OnSelected();
            }));
        }

        private void OnSelected()
        {
            ArticleContent ca = APIManager.GetInstance().currentArticle;
            String share = ca.title + " - 阅FM http://yue.fm/" + ca.short_id;

            switch (list[this.listBox.SelectedIndex].name)
            {
                case "weibo":
                case "tencent":
                case "douban":
                    String uri = "/SocialStatusPage.xaml?type=" + list[this.listBox.SelectedIndex].name + "&share=" + share;
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
                    break;
                case "mail":
                    EmailComposeTask ect = new EmailComposeTask();
                    ect.Body = share;
                    ect.Show();
                    break;
                case "sms":
                    SmsComposeTask sct = new SmsComposeTask();
                    sct.Body = share;
                    sct.Show();
                    break;
                default:
                    ShareStatusTask sst = new ShareStatusTask();
                    sst.Status = share;
                    sst.Show();
                    break;
            }

            action();

            LayoutRootOutStoryboard.Begin();
        }
    }
}