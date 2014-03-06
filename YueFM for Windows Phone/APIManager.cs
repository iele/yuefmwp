using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using YueFM.Contents;
using System.Windows.Threading;
using System.Linq;

namespace YueFM.Managers
{
    public sealed class APIManager
    {
        public String username;
        public String password;
        public Boolean isLogin;

        private static List<String> _readedArticle;
        public static List<String> readedArticle
        {
            get { return APIManager._readedArticle; }
            set
            {
                settings["readed_article"] = value;
                _readedArticle = value;
            }
        }

        private static List<ArticleContent> _cacheArticle;
        public static List<ArticleContent> cacheArticle
        {
            get { return APIManager._cacheArticle; }
            set
            {
                settings["cached_article"] = value;
                _cacheArticle = value;
            }
        }

        public ArticleContent currentArticle { get; set; }

        public List<LikeContent> likesArticle { get; set; }

        private const String API_BASE = "http://yue.fm/api";
        private const String ARTICLES = "/articles/";
        private const String ARTICLES_RANDOM = "/articles/random";
        private const String ARTICLES_NEXT = "/articles/next";
        private const String SESSION = "/session";
        private const String USERS = "/users/";
        private const String LIKES = "/likes/";

        private String cookieHeader;

        public String errorMessage { get; set; }

        public event RandomArticleEvent RandomArticleHandler;
        public delegate void RandomArticleEvent(ArticleContent ac);
        public event NextArticleEvent NextArticleHandler;
        public delegate void NextArticleEvent(ArticleContent ac);
        public event GetArticleEvent GetArticleHandler;
        public delegate void GetArticleEvent(ArticleContent ac);
  
        public event CacheArticleEvent CacheArticleHandler;
        public delegate void CacheArticleEvent(Boolean b);


        public event PostSessionEvent PostSessionHandler;
        public delegate void PostSessionEvent(Boolean success);
        public event PostUsersEvent PostUsersHandler;
        public delegate void PostUsersEvent(Boolean success);

        public event GetLikesEvent GetLikesHandler;
        public delegate void GetLikesEvent(List<LikeContent> llc);
        public event PostLikesEvent PostLikesHandler;
        public delegate void PostLikesEvent(Boolean success);
        public event DeleteLikesEvent DeleteLikesHandler;
        public delegate void DeleteLikesEvent(Boolean success);

        private static readonly APIManager instance = new APIManager();

        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        private APIManager()
        {

        }

        public static APIManager GetInstance()
        {
            try
            {
                if (APIManager.settings.Contains("readed_article"))
                {
                    APIManager.readedArticle = (List<String>)APIManager.settings["readed_article"];
                }
                else
                {
                    APIManager.readedArticle = new List<String>();
                }
                if (APIManager.settings.Contains("cached_article"))
                {
                    APIManager.cacheArticle = (List<ArticleContent>)APIManager.settings["cached_article"];
                }
                else
                {
                    APIManager.cacheArticle = new List<ArticleContent>();
                }
            }
            catch (Exception)
            {
                APIManager.readedArticle = new List<String>();
                APIManager.cacheArticle = new List<ArticleContent>();
            }

            return instance;
        }

        #region Login State
        public void RemoveLoginState()
        {
            settings["username"] = null;
            settings["password"] = null;
        }

        private Action<Boolean> _restoreLoginStateAction;
        public Boolean RestoreLoginState(Action<Boolean> a)
        {
            Object username, password;

            this._restoreLoginStateAction = a;

            if (settings.TryGetValue("username", out username) &&
                settings.TryGetValue("password", out password))
            {
                this.username = username as String;
                this.password = password as String;

                PostSessionHandler += new PostSessionEvent(APIManager_PostSessionHandler);
                PostSession();

                return true;
            }

            return false;
        }

        void APIManager_PostSessionHandler(bool success)
        {
            PostSessionHandler -= APIManager_PostSessionHandler;

            _restoreLoginStateAction(success);
        }
        #endregion

        #region Article Manage
        public void GetRandomArticle()
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + ARTICLES_RANDOM));
            if (hwr.Headers == null)
            {
                hwr.Headers = new WebHeaderCollection();
            }
            hwr.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            hwr.BeginGetResponse(new AsyncCallback(GetRandomArticleCallBack), hwr);
        }

        private void GetRandomArticleCallBack(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                WebResponse response = hwr.EndGetResponse(asynchronousResult);
                ArticleContent article = GetArticleFromResponse(response);

                if (readedArticle != null && readedArticle.Contains(article.short_id))
                {
                    GetRandomArticle();
                    return;
                }

                this.currentArticle = article;
                RandomArticleHandler(this.currentArticle);

                CheckReadedList(article);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                errorMessage = e.Message;

                if (cacheArticle != null && cacheArticle.Count > 0)
                {
                    Random rd = new Random();
                    this.currentArticle = cacheArticle[rd.Next(cacheArticle.Count)];
                    RandomArticleHandler(this.currentArticle);
                }
                else
                {
                    RandomArticleHandler(null);
                }
            }
        }

        public void GetNextArticle()
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + ARTICLES_NEXT));
            if (hwr.Headers == null)
            {
                hwr.Headers = new WebHeaderCollection();
            }
            hwr.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            hwr.Headers["Cookie"] = cookieHeader;
            hwr.BeginGetResponse(new AsyncCallback(GetNextCallBack), hwr);
        }

        private void GetNextCallBack(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                WebResponse response = hwr.EndGetResponse(asynchronousResult);

                ArticleContent article = GetArticleFromResponse(response);

                if (readedArticle != null && readedArticle.Contains(article.short_id))
                {
                    GetNextArticle();
                    return;
                }
                this.currentArticle = article;
                NextArticleHandler(this.currentArticle);

                CheckReadedList(article);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                errorMessage = e.Message;

                if (cacheArticle != null && cacheArticle.Count > 0)
                {
                    Random rd = new Random();
                    this.currentArticle = cacheArticle[rd.Next(cacheArticle.Count)];
                    NextArticleHandler(this.currentArticle);
                }
                else
                {
                    NextArticleHandler(null);
                }
            }
        }

        private void CheckReadedList(ArticleContent article)
        {
            if (readedArticle != null)
            {
                readedArticle.Add(article.short_id);
                if (readedArticle.Count >= 50)
                {
                    readedArticle.RemoveAt(0);
                }

                APIManager.readedArticle = APIManager.readedArticle;
            }
        }

        public void GetArticle(String id)
        {
            Boolean flag = false;
            cacheArticle.ForEach(new Action<ArticleContent>(item =>
            {
                if (item.id.CompareTo(id) == 0)
                {
                    flag = true;
                    this.currentArticle = item;
                    GetArticleHandler(this.currentArticle);
                }
            }));
            if (flag)
                return;


            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + ARTICLES + "/" + id));
            if (hwr.Headers == null)
            {
                hwr.Headers = new WebHeaderCollection();
            }
            hwr.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            if (cookieHeader != null)
                hwr.Headers["Cookie"] = cookieHeader;
            hwr.BeginGetResponse(new AsyncCallback(GetArticleCallBack), hwr);
        }

        private void GetArticleCallBack(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                WebResponse response = hwr.EndGetResponse(asynchronousResult);
                this.currentArticle = GetArticleFromResponse(response);
                GetArticleHandler(this.currentArticle);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                errorMessage = e.Message;
                GetArticleHandler(null);
            }
        }


        public void CacheArticle(String id)
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + ARTICLES + "/" + id));
            if (hwr.Headers == null)
            {
                hwr.Headers = new WebHeaderCollection();
            }
            hwr.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            if (cookieHeader != null)
                hwr.Headers["Cookie"] = cookieHeader;
            hwr.BeginGetResponse(new AsyncCallback(GetCacheArticleCallBack), hwr);
        }

        private void GetCacheArticleCallBack(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                WebResponse response = hwr.EndGetResponse(asynchronousResult);
                var content = GetArticleFromResponse(response);

                cacheArticle.Add(content);

                CacheArticleHandler(true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                errorMessage = e.Message;
                CacheArticleHandler(false);
            }
        }

        private ArticleContent GetArticleFromResponse(WebResponse response)
        {
            ArticleContent ac = new ArticleContent();
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(ArticleContent));
            Stream stream = response.GetResponseStream();
            ac = ds.ReadObject(stream) as ArticleContent;
            stream.Close();
            response.Close();

            return ac;
        }
        #endregion

        #region Session Manage
        public void PostSession()
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + SESSION));
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            hwr.BeginGetRequestStream(new AsyncCallback(PostSessionCallBack1), hwr);
        }

        private void PostSessionCallBack1(IAsyncResult asynchronousResult)
        {
            HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream stream = hwr.EndGetRequestStream(asynchronousResult);
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("username=" + this.username + "&password=" + this.password);
            sw.Flush();
            sw.Close();

            hwr.BeginGetResponse(new AsyncCallback(PostSessionCallBack2), hwr);
        }

        private void PostSessionCallBack2(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = hwr.EndGetResponse(asynchronousResult) as HttpWebResponse;
                cookieHeader = response.Headers["Set-Cookie"];

                HttpStatusCode code = response.StatusCode;

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                String result = sr.ReadToEnd();

                this.isLogin = true;

                settings["username"] = this.username;
                settings["password"] = this.password;

                PostSessionHandler(true);
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                if (e.Response == null || e.Response.ContentLength == 0)
                    errorMessage = e.StackTrace;
                else
                    errorMessage = "InfomationError";
                PostSessionHandler(false);
            }
        }

        public void PostUsers(String username, String password)
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + USERS));
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            Object[] o = { hwr, username, password };
            hwr.BeginGetRequestStream(new AsyncCallback(PostUsersCallBack1), o);
        }

        private void PostUsersCallBack1(IAsyncResult asynchronousResult)
        {
            HttpWebRequest hwr = (HttpWebRequest)(asynchronousResult.AsyncState as Object[])[0];
            String username = (String)(asynchronousResult.AsyncState as Object[])[1];
            String password = (String)(asynchronousResult.AsyncState as Object[])[2];
            Stream stream = hwr.EndGetRequestStream(asynchronousResult);
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("username=" + username + "&password=" + password);
            sw.Flush();
            sw.Close();

            hwr.BeginGetResponse(new AsyncCallback(PostUsersCallBack2), hwr);
        }

        private void PostUsersCallBack2(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = hwr.EndGetResponse(asynchronousResult) as HttpWebResponse;
                HttpStatusCode code = response.StatusCode;

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                String result = sr.ReadToEnd();

                PostUsersHandler(true);
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                if (e.Response.ContentLength != 0)
                    errorMessage = "InfomationError";
                else
                    errorMessage = e.StackTrace;
                PostUsersHandler(false);
            }
        }
        #endregion

        #region Likes Manage
        public void GetLikes()
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + LIKES));
            if (hwr.Headers == null)
            {
                hwr.Headers = new WebHeaderCollection();
            }
            hwr.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            hwr.Headers["Cookie"] = cookieHeader;
            hwr.BeginGetResponse(new AsyncCallback(GetLikeCallBack), hwr);
        }

        private void GetLikeCallBack(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                WebResponse response = hwr.EndGetResponse(asynchronousResult);
                likesArticle = new List<LikeContent>();
                DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(List<LikeContent>));
                Stream stream = response.GetResponseStream();
                likesArticle = ds.ReadObject(stream) as List<LikeContent>;
                stream.Close();
                response.Close();

                GetLikesHandler(likesArticle);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                errorMessage = e.Message;
                GetLikesHandler(null);
            }
        }

        private String postId;
        private String postTitle;
        public void PostLikes(String id)
        {
            postId = id;
            postTitle = currentArticle.title;
            HttpWebRequest hwr = WebRequest.CreateHttp(new Uri(API_BASE + LIKES));
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            hwr.Headers["Cookie"] = cookieHeader;
            Object[] o = { hwr, id };
            hwr.BeginGetRequestStream(new AsyncCallback(PostLikesCallBack1), o);
        }

        private void PostLikesCallBack1(IAsyncResult asynchronousResult)
        {
            HttpWebRequest hwr = (HttpWebRequest)(asynchronousResult.AsyncState as Object[])[0];
            String id = (String)(asynchronousResult.AsyncState as Object[])[1];
            Stream stream = hwr.EndGetRequestStream(asynchronousResult);
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("any_id=" + id);
            sw.Flush();
            sw.Close();

            hwr.BeginGetResponse(new AsyncCallback(PostLikesCallBack2), hwr);
        }

        private void PostLikesCallBack2(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = hwr.EndGetResponse(asynchronousResult) as HttpWebResponse;
                HttpStatusCode code = response.StatusCode;
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                String result = sr.ReadToEnd();

                if (likesArticle != null)
                {
                    LikeContent lc = new LikeContent();
                    lc.article_title = postTitle;
                    lc.article_id = postId;
                    likesArticle.Add(lc);
                }
                if (this.currentArticle != null && !cacheArticle.Contains(this.currentArticle) && this.currentArticle.title == postTitle)
                {
                    cacheArticle.Add(this.currentArticle);
                }
                PostLikesHandler(true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                errorMessage = e.Message;
                PostLikesHandler(false);
            }

        }

        private String deleteId;
        public void DeleteLikes(String id)
        {
            deleteId = id;
            WebClient wc = new WebClient();
            wc.Headers["Cookie"] = cookieHeader;
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
            wc.UploadStringAsync(new Uri(API_BASE + LIKES + id), "DELETE", "");
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                String result = e.Result;

                if (likesArticle != null)
                {
                    likesArticle.ForEach(new Action<LikeContent>(item =>
                    {
                        if (item.article_id.CompareTo(deleteId) == 0)
                        {
                            likesArticle.Remove(item);
                        }
                    }));
                }

                cacheArticle.ForEach(new Action<ArticleContent>(item =>
                {
                    if (item.id.CompareTo(deleteId) == 0)
                    {
                        cacheArticle.Remove(item);
                    }
                }));

                DeleteLikesHandler(true);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                errorMessage = err.Message;
                DeleteLikesHandler(false);
            }
        }
        #endregion
    }
}
