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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using YueFM.Managers;
using YueFM.Utils;
using YueFM.Contents;
using System.Windows.Controls.Primitives;
using YueFM.Controls;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Imaging;
using Microsoft.Live;
using System.Runtime.Serialization.Json;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Live.Controls;
using System.Windows.Resources;
using Windows.Phone.Speech.Synthesis;

namespace YueFM.Pages
{
    public partial class MainPage : PhoneApplicationPage
    {
        private APIManager apiManager = APIManager.GetInstance();
        private SettingManager settingManager = SettingManager.GetInstance();

        private SpeechSynthesizer synth = new SpeechSynthesizer();

        private SocialSendControl SocialSendControl;

        private List<UIElement> articleContent;

        public MainPage()
        {
            InitializeComponent();

            if (ResolutionUtils.CurrentResolution == Resolutions.HD720p)
            {
                this.scroll.Height += 53;
            }

            AppUtils.FlurryLog("Main");

            this.progressBar.Visibility = Visibility.Visible;

            this.SocialSendControl = new SocialSendControl();

            this.scroll.RenderTransform = this.ArticleTransforms;

            this.floatPopup.FloatInStoryboardHandler += floatPopup_FloatInStoryboardHandler;
            this.floatPopup.FloatOutStoryboardHandler += floatPopup_FloatOutStoryboardHandler;
            this.floatPopup.NextImageTapEventHandler += floatPopup_NextImageTapEventHandler;
            this.floatPopup.FavImageTapEventHandler += floatPopup_FavImageTapEventHandler;

            apiManager.RandomArticleHandler += this.RandomArticleHandler;
            apiManager.NextArticleHandler += this.NextArticleHandler;
            apiManager.GetArticleHandler += this.GetArticleHandler;
            apiManager.DeleteLikesHandler += this.DeleteLikesHandler;
            apiManager.PostLikesHandler += this.PostLikesHandler;

            //this.titleControl.image.Visibility = Visibility.Collapsed;
            //this.titleControl.OnImageTap = new Action(() =>
            //{
            //    Dispatcher.BeginInvoke(() =>
            //    {
            //        LikeArticle();
            //    });
            //});
            this.popup.IsOpen = false;

            GetArticleHandler(apiManager.currentArticle);

        }

        #region ApplicationBar
        private ApplicationBarIconButton RefreshButton, LikeButton, ShareButton, OriginalButton;
        private ApplicationBarMenuItem SkyDriveItem, LikesItem, SettingItem;
        private void InitAppBar(Boolean is_liked, Boolean is_sharing)
        {
            if (!is_freshing)
            {
                ApplicationBar appBar = ThemeManager.CreateApplicationBar();
                appBar.Mode = ApplicationBarMode.Minimized;

                RefreshButton = new ApplicationBarIconButton(new Uri("/Images/appbar.next.rest.png", UriKind.Relative));
                RefreshButton.Click += new EventHandler(this.RefreshButton_Click);
                RefreshButton.Text = "下一文章";
                appBar.Buttons.Add(RefreshButton);
                if (is_liked)
                {
                    LikeButton = new ApplicationBarIconButton(new Uri("/Images/appbar.cancel.rest.png", UriKind.Relative));
                    LikeButton.Click += new EventHandler(this.LikeButton_Click);
                    LikeButton.Text = "取消推荐";

                    if (is_sharing)
                    {
                        LikeButton.IsEnabled = false;
                        LikeButton.Text = "正在取消..";
                    }

                    appBar.Buttons.Add(LikeButton);
                }
                else
                {
                    LikeButton = new ApplicationBarIconButton(new Uri("/Images/appbar.favs.rest.png", UriKind.Relative));
                    LikeButton.Click += new EventHandler(this.LikeButton_Click);
                    LikeButton.Text = "推荐文章";

                    if (is_sharing)
                    {
                        LikeButton.IsEnabled = false;
                        LikeButton.Text = "正在推荐..";
                    }

                    appBar.Buttons.Add(LikeButton);
                }

                ShareButton = new ApplicationBarIconButton(new Uri("/Images/appbar.share.rest.png", UriKind.Relative));
                ShareButton.Click += new EventHandler(this.ShareButton_Click);
                ShareButton.Text = "分享文章";
                appBar.Buttons.Add(ShareButton);

                OriginalButton = new ApplicationBarIconButton(new Uri("/Images/appbar.feature.search.rest.png", UriKind.Relative));
                OriginalButton.Click += new EventHandler(this.OriginalButton_Click);
                OriginalButton.Text = "查看原网页";
                appBar.Buttons.Add(OriginalButton);

                if (settingManager.skydrive_login)
                {
                    SkyDriveItem = new ApplicationBarMenuItem("上传到SkyDrive");
                    SkyDriveItem.Click += new EventHandler(this.SkyDriveItem_Click);
                    appBar.MenuItems.Add(SkyDriveItem);
                }

                LikesItem = new ApplicationBarMenuItem("我的推荐");
                LikesItem.Click += new EventHandler(this.LikesItem_Click);
                appBar.MenuItems.Add(LikesItem);

                SettingItem = new ApplicationBarMenuItem("设置");
                SettingItem.Click += new EventHandler(this.SettingItem_Click);
                appBar.MenuItems.Add(SettingItem);

                ApplicationBar = appBar;

            }
        }



        private void DisableAppBar()
        {
            ApplicationBar = null;
        }
        #endregion

        #region ApplicationBar Buttons & Items
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            AppUtils.FlurryLog("RefreshButton");

            GetNewArticle();
            //this.titleControl.TitleOutAnimation();
            DisableAppBar();
        }

        private Boolean is_speech;
        private async void ReadItem_Click(object sender, EventArgs e)
        {
            if (this.apiManager.currentArticle != null)
            {

                if (is_speech)
                {
                    is_speech = false;
                    synth.CancelAll();
                    InitAppBar(this.apiManager.currentArticle.is_liked, false);
                    return;
                }
                if (!is_speech)
                {
                    HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(this.apiManager.currentArticle.body);

                    is_speech = true;
                    InitAppBar(this.apiManager.currentArticle.is_liked, false);

                    foreach (var node in html.DocumentNode.ChildNodes)
                    {
                        await synth.SpeakTextAsync(node.InnerText);
                    }
                }
                is_speech = false;
                InitAppBar(this.apiManager.currentArticle.is_liked, false);
            }
        }

        private void OriginalButton_Click(object sender, EventArgs e)
        {
            AppUtils.FlurryLog("OriginalButton");

            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri(apiManager.currentArticle.source);
            wbt.Show();
        }

        private void ShareButton_Click(object sender, EventArgs e)
        {
            AppUtils.FlurryLog("ShareButton");

            SocialSendControl.CreatePopup();

            OnShareSelecting();
            this.SocialSendControl.action = (() =>
            {
                this.OnShareSelected();
            });
        }

        private void LikeButton_Click(object sender, EventArgs e)
        {
            AppUtils.FlurryLog("LikeButton");

            LikeArticle();
        }
        private void LikeArticle()
        {
            if (Check_Login() && apiManager.currentArticle != null)
            {
                InitAppBar(apiManager.currentArticle.is_liked, true);

                if (apiManager.currentArticle.is_liked)
                {
                    apiManager.DeleteLikes(apiManager.currentArticle.id);
                }
                else
                {
                    apiManager.PostLikes(apiManager.currentArticle.id);
                }
            }
        }

        private void LikesItem_Click(object sender, EventArgs e)
        {
            if (Check_Login())
                NavigationService.Navigate(new Uri("/LikesPage.xaml", UriKind.Relative));
        }

        private void SettingItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingPage.xaml", UriKind.Relative));
        }
        #endregion

        #region API Manager Handler
        private Boolean Check_Login()
        {
            if (apiManager.isLogin == false)
            {
                MessageBoxResult mbr = MessageBox.Show("尚未登录，现在登录吗？", "提示", MessageBoxButton.OKCancel);
                if (mbr == MessageBoxResult.OK)
                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                return false;
            }
            return true;
        }

        private void GetArticleHandler(ArticleContent ac)
        {
            if (ac == null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    this.progressBar.Visibility = Visibility.Collapsed;

                    MessageBox.Show("未能获取到文章，是否是网络出现了问题？");

                    if (apiManager.currentArticle != null)
                        RefreshCurrentArticle();
                    else
                    {
                        ApplicationBar appBar = ThemeManager.CreateApplicationBar();
                        appBar.Mode = ApplicationBarMode.Default;

                        RefreshButton = new ApplicationBarIconButton(new Uri("/Images/appbar.next.rest.png", UriKind.Relative));
                        RefreshButton.Click += new EventHandler(this.RefreshButton_Click);
                        RefreshButton.Text = "下一文章";
                        appBar.Buttons.Add(RefreshButton);

                        SettingItem = new ApplicationBarMenuItem("设置");
                        SettingItem.Click += new EventHandler(this.SettingItem_Click);
                        appBar.MenuItems.Add(SettingItem);

                        ApplicationBar = appBar;
                    }
                });
                return;
            }
            if (ac != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    //this.titleControl.LayoutRoot.Opacity = 0;
                    apiManager.currentArticle = ac;
                    RefreshCurrentArticle();
                });
            }
        }

        private void RandomArticleHandler(ArticleContent ac)
        {
            GetArticleHandler(ac);
        }

        private void NextArticleHandler(ArticleContent ac)
        {
            GetArticleHandler(ac);
        }

        private void PostLikesHandler(Boolean success)
        {
            if (is_freshing)
                return;

            if (success)
            {
                apiManager.currentArticle.is_liked = true;
                Dispatcher.BeginInvoke(() =>
                {
                    InitAppBar(apiManager.currentArticle.is_liked, false);
                    AppUtils.ToastPromptShow("阅FM", "推荐成功~");
                    //this.titleControl.ChangeBookmark(true);
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    InitAppBar(apiManager.currentArticle.is_liked, false);
                    AppUtils.ToastPromptShow("阅FM", "推荐失败");
                });
            }
        }

        private void DeleteLikesHandler(Boolean success)
        {
            if (is_freshing)
                return;

            if (success)
            {
                apiManager.currentArticle.is_liked = false;
                Dispatcher.BeginInvoke(() =>
                {
                    InitAppBar(apiManager.currentArticle.is_liked, false);
                    AppUtils.ToastPromptShow("阅FM", "删除成功~");
                    //this.titleControl.ChangeBookmark(false);
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    InitAppBar(apiManager.currentArticle.is_liked, false);
                    AppUtils.ToastPromptShow("阅FM", "删除失败");
                });
            }
        }

        #endregion

        #region SkyDrive Upload
        private LiveConnectSessionChangedEventArgs liveConnectSessionChangedEventArgs;
        private LiveConnectClient client;
        private Boolean is_upload_started;
        private async void ButtonSignin_SessionChanged(object sender, LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == LiveConnectSessionStatus.Connected && settingManager.skydrive_login)
            {
                client = new LiveConnectClient(e.Session);
                liveConnectSessionChangedEventArgs = e;
                var result = await client.GetAsync("me");
            }
            else
            {
                client = null;
            }
        }

        private async void SkyDriveItem_Click(object sender, EventArgs e)
        {
            AppUtils.FlurryLog("SkyDrive");

            if (client == null)
                return;
            if (is_upload_started)
            {
                Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "请等待上传操作结束后开始新上传"));
                return;
            }
            is_upload_started = true;

            Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "正在处理上传请求.."));

            if (this.apiManager.currentArticle == null)
                return;
            ArticleContent content = this.apiManager.currentArticle;

            var folderData = new Dictionary<string, object>();

            client = new LiveConnectClient(client.Session);

            try
            {
                var result = await client.GetAsync("me/skydrive/files");
                SkyDriveContent sdc = new SkyDriveContent();
                DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(SkyDriveContent));
                byte[] bs = System.Text.Encoding.Unicode.GetBytes(result.RawResult);
                MemoryStream ms = new MemoryStream(bs);
                sdc = ds.ReadObject(ms) as SkyDriveContent;
                string folder = null;
                foreach (var item in sdc.data)
                {
                    if (item.name == "阅FM")
                    {
                        folder = item.id;
                    }
                }
                if (folder == null)
                {
                    Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "正在创建上传目标文件夹.."));

                    folderData.Add("name", "阅FM");
                    result = await client.PostAsync("me/skydrive", folderData);
                    folder = result.Result.ContainsKey("id") ? result.Result["id"].ToString() : null;
                    SendFileToSkyDrive(client, folder, content);
                    return;
                }
                SendFileToSkyDrive(client, folder, content);
            }
            catch (Exception e5)
            {
            }
        }

        private async void SendFileToSkyDrive(LiveConnectClient client, String folder, ArticleContent content)
        {
            if (folder != null)
            {
                Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "正在准备文件"));

                var store = IsolatedStorageFile.GetUserStoreForApplication();

                string filePath = "tmp.html";

                if (store.FileExists(filePath))
                {
                    store.DeleteFile(filePath);
                }

                var tmpfile = store.OpenFile(filePath,
                                FileMode.OpenOrCreate, FileAccess.Write);
                if (tmpfile != null)
                {
                    try
                    {
                        using (StreamWriter sw =
                            new StreamWriter(tmpfile))
                        {

                            string css;
                            string fileName = "css.txt";
                            StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri(fileName, UriKind.Relative));
                            using (StreamReader reader = new StreamReader(resourceInfo.Stream))
                            {
                                css = reader.ReadToEnd();
                            }

                            String html = "";
                            html += "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"><style>" + css + "</style></head>";
                            html += "<body><div class=\"page\"><div class=\"article\">";
                            html += "<a target=\"_blank\" href=" + content.source + " class=\"source\">来源</a>";
                            html += "<h1 class=\"title\">" + content.title + "</h1><div class=\"body\">";
                            html += content.body;
                            html += "</div></div></div></body></html>";
                            sw.Write(html);
                            sw.Flush();
                        }
                    }
                    catch (IsolatedStorageException ex)
                    {
                        Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "上传失败,准备文件时发生错误"));
                        is_upload_started = false;
                        return;
                    }
                }
                tmpfile.Close();

                Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "开始上传.."));

                await client.UploadAsync(folder, content.title + ".html", store.OpenFile(filePath,
                                FileMode.Open, FileAccess.Read), OverwriteOption.Overwrite);
                Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "上传成功!")); is_upload_started = false;
            }
            else
            {
                Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "上传失败,找不到目标文件夹"));
                is_upload_started = false;
            }
        }
        #endregion

        #region Article Loading Handle
        public Boolean is_freshing = false;
        public Boolean is_first = true;

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (apiManager.currentArticle != null)
            {
                if (LikesPage.is_selected == true)
                {
                    LikesPage.is_selected = false;
                    GetArticleStart();
                    return;
                }
                if (settingManager.is_setting_changed && !is_first)
                {
                    settingManager.is_setting_changed = false;
                    RefreshCurrentArticle();
                }
                if (this.is_sharing)
                {
                    this.SocialSendControl.HidePopup();
                    OnShareSelected();
                }
                if (this.is_first)
                {
                    is_first = false;
                }
            }
            else
            {
                this.GetNewArticle();
                //this.titleControl.TitleOutAnimation();
            }

            base.OnNavigatedTo(e);
        }

        private void GetArticleStart()
        {
            Dispatcher.BeginInvoke(() =>
            {
                DisableAppBar();
                //this.titleControl.TitleOutAnimation();
            });
        }

        private void RefreshCurrentArticle()
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.progressBar.Visibility = Visibility.Collapsed;
                this.articleContent = new List<UIElement>();

                var header = new HeaderControl();
                if (settingManager.night_mode)
                {
                    Color f = Color.FromArgb(0xff,
                                   (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                                   (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                                   (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11));
                    header.title.Foreground = new SolidColorBrush(f);
                    header.line.Visibility = Visibility.Collapsed;
                    header.source.Foreground = new SolidColorBrush(f);
                }
                if (this.settingManager.night_mode)
                    header.SetTitleSize(false);
                else
                    header.SetTitleSize(true);

                this.articleContent.Add(header);

                header.SetTitle(this.apiManager.currentArticle.title, this.apiManager.currentArticle.source, int.Parse(this.settingManager.article_size));

                var rich = new HtmlToRichText(articleContent);
                rich.size = int.Parse(this.settingManager.article_size);
                rich.html = this.apiManager.currentArticle.body;
                rich.Parse();

                var footer = new FooterControl(this.apiManager.currentArticle.likes);
                this.articleContent.Add(footer);

                this.ArticleOutStoryboard.Begin();
            });
        }

        private void GetNewArticle()
        {
            AppUtils.FlurryLog("NewArticle");

            //this.titleControl.TitleOutAnimation();

            if (this.popup.IsOpen == true)
                this.floatPopup.FloatOutStoryboard.Begin();       

            DisableAppBar();

            this.is_freshing = true;

            Dispatcher.BeginInvoke(() =>
            {
                GC.Collect();

                this.scroll.IsHitTestVisible = false;

                this.progressBar.Visibility = Visibility.Visible;

                foreach (var item in this.content.Children)
                {
                    item.Opacity = 0.25;
                }

            });
            if (apiManager.isLogin)
            {
                apiManager.GetNextArticle();
            }
            else
            {
                apiManager.GetRandomArticle();
            }
        }

        private void ContentLoadCompleted()
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.scroll.IsHitTestVisible = true;

                this.is_freshing = false;

                InitAppBar(apiManager.currentArticle.is_liked, false);

                if (!settingManager.night_mode)
                {
                    //this.titleControl.image.Visibility = Visibility.Visible;

                    //if (apiManager.currentArticle.is_liked)
                        //this.titleControl.image.SetValue(Image.SourceProperty, new BitmapImage(new Uri("/Images/BookmarkLiked.png", UriKind.Relative)));
                    //else
                        //this.titleControl.image.SetValue(Image.SourceProperty, new BitmapImage(new Uri("/Images/Bookmark.png", UriKind.Relative)));
                }
                else
                {
                    //this.titleControl.image.Visibility = Visibility.Collapsed;
                }

                this.ArticleInStoryboard.Begin();
                //this.titleControl.TitleInAnimation();

                settingManager.read_count = settingManager.read_count + 1;
                settingManager.last_read = apiManager.currentArticle.title;
            });
        }

        private void ArticleInStoryboard_Completed(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.content.Opacity = 1;
            });
        }

        private void ArticleRestoreStoryboard_Completed(object sender, EventArgs e)
        {
        }

        private void ArticleOutStoryboard_Completed(object sender, EventArgs e)
        {
            if (this.apiManager.currentArticle == null)
                return;

            Dispatcher.BeginInvoke(() =>
            {
                this.content.Children.Clear();
                foreach (var i in this.articleContent)
                {
                    this.content.Children.Add(i);
                }

                ContentLoadCompleted();

                this.scroll.UpdateLayout();
                this.scroll.ScrollToVerticalOffset(0);
            });
        }
        #endregion

        #region Sharing Control Handle
        public Boolean is_sharing = false;

        private void OnShareSelecting()
        {
            this.DisableAppBar();
            this.is_sharing = true;
        }

        private void OnShareSelected()
        {
            this.InitAppBar(apiManager.currentArticle.is_liked, false);
            this.is_sharing = false;
        }
        #endregion

        #region Tile & Exit


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (is_sharing)
            {
                this.SocialSendControl.HidePopup();
                OnShareSelected();
                e.Cancel = true;
            }
            else
            {
                RefreshTile();

                if (settingManager.quit_confirm)
                {
                    MessageBoxResult result = MessageBox.Show("确认退出 阅FM for Windows Phone？", "确认退出", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        FlurryWP8SDK.Api.EndSession();
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    FlurryWP8SDK.Api.EndSession();
                    e.Cancel = false;
                }
            }

            base.OnBackKeyPress(e);
        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }

        private void RefreshTile()
        {
            ShellTile tileToUpdate = ShellTile.ActiveTiles.FirstOrDefault();
            IconicTileData TileData = new IconicTileData()
            {
                Title = "阅FM",
                WideContent1 = "阅读统计",
                WideContent2 = "您已阅读: " + settingManager.read_count + "篇 " + "已推荐: " + settingManager.like_count + "篇\n",
                WideContent3 = "上次阅读: " + settingManager.last_read,
                SmallIconImage = new Uri("/Images/IconSmall.png", UriKind.Relative),
                IconImage = new Uri("/Images/IconMedium.png", UriKind.Relative)
            };
            tileToUpdate.Update(TileData);
        }
        #endregion

        #region Control Panel
        private void floatPopup_FloatInStoryboardHandler(object sender, EventArgs b)
        {
        }

        private void floatPopup_FloatOutStoryboardHandler(object sender, EventArgs b)
        {
            Dispatcher.BeginInvoke(() => this.popup.IsOpen = false);
        }


        private void GestureListener_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.popup.IsOpen == false)
            {
                this.popup.IsOpen = true;
                this.floatPopup.FloatInStoryboard.Begin();
            }
        }

        private void GestureListener_DragStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (this.popup.IsOpen == true)
                this.floatPopup.FloatOutStoryboard.Begin();       
        }

        void floatPopup_FavImageTapEventHandler(object sender, EventArgs b)
        {

        }

        void floatPopup_NextImageTapEventHandler(object sender, EventArgs b)
        {
            this.GetNewArticle();
        }
        #endregion

    }
}