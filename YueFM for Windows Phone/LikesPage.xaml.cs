using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using YueFM.Contents;
using YueFM.Managers;
using YueFM.Controls;
using YueFM.Utils;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading;
using Microsoft.Phone.Shell;

namespace YueFM.Pages
{
    public partial class LikesPage : PhoneApplicationPage
    {
        private List<LikeContent> llc;

        public static Boolean is_selected = false;

        private APIManager apiManager = APIManager.GetInstance();

        public LikesPage()
        {
            InitializeComponent();

            AppUtils.FlurryLog("Likes");
         
            if (apiManager.likesArticle == null)
            {
                apiManager.GetLikesHandler += this.GetLikesHandler;
                apiManager.GetLikes();
            }
            else
            {
                llc = this.apiManager.likesArticle;
                ListBoxInit();
            }


        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.apiManager.likesArticle != null)
            {
                llc = this.apiManager.likesArticle;
                ListBoxInit();
            }

            base.OnNavigatedTo(e);
        }

        private void ListBoxInit()
        {
            this.listBox.DataContext = llc;

            Boolean flag = true;
            llc.ForEach((item) =>
            {
                var query = from article in APIManager.cacheArticle
                            where article.id == item.article_id
                            select article;
                if (query.FirstOrDefault() != null)
                    flag &= true;
                else
                    flag &= false;
            });
            ApplicationBar appBar = ThemeManager.CreateApplicationBar();

            appBar.Mode = ApplicationBarMode.Minimized;
            var button = new ApplicationBarIconButton(new Uri("/Images/appbar.sync.rest.png", UriKind.Relative));
            if (flag == false)
            {
                button.Click += new EventHandler(this.ApplicationBarIconButton_Click);
                button.Text = "同步到本地";
            }
            else
            {
                button.Click += new EventHandler(this.ApplicationBarIconButton_Click);
                button.Text = "文章已同步";
                button.IsEnabled = false;
            }
            appBar.Buttons.Add(button);
            this.ApplicationBar = appBar;

            SettingManager.GetInstance().like_count = (UInt32)llc.Count;

            this.progressBar.Visibility = Visibility.Collapsed;

        }

        private void GetLikesHandler(List<LikeContent> llc)
        {
            if (llc != null)
            {
                this.llc = llc;
                Dispatcher.BeginInvoke(() =>
                {
                    ListBoxInit();
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    this.progressBar.Visibility = Visibility.Collapsed;
                    MessageBox.Show("列表获取失败，是否是网络出现了问题？");
                });
            }
            apiManager.GetLikesHandler -= this.GetLikesHandler;
        }

        private void LikeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            is_selected = true;

            (sender as LikeItem).StoryBoardCompleted = () =>
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }

                this.apiManager.GetArticle(this.llc[this.listBox.SelectedIndex].article_id);
            };

            (sender as LikeItem).SelectedStoryBoard.Begin();
        }

        private int count;
        private Boolean is_syncing;
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            AppUtils.FlurryLog("Sync");
            
            if (is_syncing)
            {
                Dispatcher.BeginInvoke(() => { AppUtils.ToastPromptShow("阅FM", "当前已有同步任务"); });
                return;
            }
            is_syncing = true;

            Dispatcher.BeginInvoke(() => { AppUtils.ToastPromptShow("阅FM", "开始同步推荐文章到本地.."); });

            count = 0;

            apiManager.CacheArticleHandler += apiManager_CacheArticleHandler;
            if (llc != null)
            {
                llc.ForEach((item) =>
                {
                    var query = from article in APIManager.cacheArticle
                                where article.id == item.article_id
                                select article;

                    if (query.FirstOrDefault() == null)
                    {
                        apiManager.CacheArticle(item.article_id);
                    }
                    else
                    {
                        count++;
                    }
                });
            }
        }

        private void apiManager_CacheArticleHandler(bool b)
        {
            count++;
            if (count >= llc.Count)
            {
                Dispatcher.BeginInvoke(() => { AppUtils.ToastPromptShow("阅FM", "同步完毕!"); });
                is_syncing = false;

                Dispatcher.BeginInvoke(() =>
                {
                    llc = apiManager.likesArticle;
                    ListBoxInit();
                });
            }
        }
    }
}