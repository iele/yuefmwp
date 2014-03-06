// ===============================================================================
// SocialAPI.cs
// Description:Socail API for Sharing Images
// Author:  Alexis
// Date:    2012.05.27
// Blog: http://alexis.cnblogs.com
// ===============================================================================
// Copyright (c) Alexis. 
// All rights reserved.
// ===============================================================================

using System;
using System.Net;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.IO;
using Alexis.WindowsPhone.Social.Helper;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexis.WindowsPhone.Social.Renren;

namespace Alexis.WindowsPhone.Social
{
    public enum SocialType
    {
        /// <summary>
        /// sina weibo
        /// </summary>
        Weibo,
        /// <summary>
        /// tencent weibo
        /// </summary>
        Tencent,
        /// <summary>
        /// renren 
        /// </summary>
        Renren,
        /// <summary>
        /// douban
        /// </summary>
        Douban,
        /// <summary>
        /// 163
        /// </summary>
        Fanfou,
    }

    public class SocialAPI
    {
        public const string SHARE_IMAGE_NAME = "share.jpg";
        public const string ACCESS_TOKEN_PREFIX = "accesstoken_{0}.json";
        static object _obj = new object();

        #region Property
        private static AccessToken _weiboAccessToken;
        public static AccessToken WeiboAccessToken
        {
            get
            {
                if (_weiboAccessToken == null)
                {
                    string path = string.Format(ACCESS_TOKEN_PREFIX, SocialType.Weibo.ToString());
                    _weiboAccessToken = JsonHelper.LoadJson<AccessToken>(path) as AccessToken;
                }
                return _weiboAccessToken;
            }
            set
            {
                _weiboAccessToken = value;
            }
        }


        private static AccessToken _tencentAccessToken;
        public static AccessToken TencentAccessToken
        {
            get
            {
                if (_tencentAccessToken == null)
                {
                    string path = string.Format(ACCESS_TOKEN_PREFIX, SocialType.Tencent.ToString());
                    _tencentAccessToken = JsonHelper.LoadJson<AccessToken>(path) as AccessToken;
                }
                return _tencentAccessToken;
            }
            set
            {
                _tencentAccessToken = value;
            }
        }

        private static AccessToken _renrenAccessToken;
        public static AccessToken RenrenAccessToken
        {
            get
            {
                if (_renrenAccessToken == null)
                {
                    string path = string.Format(ACCESS_TOKEN_PREFIX, SocialType.Renren.ToString());
                    _renrenAccessToken = JsonHelper.LoadJson<AccessToken>(path) as AccessToken;
                }
                return _renrenAccessToken;
            }
            set
            {
                _renrenAccessToken = value;
            }
        }

        private static AccessToken _doubanAccessToken;
        public static AccessToken DoubanAccessToken
        {
            get
            {
                if (_doubanAccessToken == null)
                {
                    string path = string.Format(ACCESS_TOKEN_PREFIX, SocialType.Douban.ToString());
                    _doubanAccessToken = JsonHelper.LoadJson<AccessToken>(path) as AccessToken;
                }
                return _doubanAccessToken;
            }
            set
            {
                _doubanAccessToken = value;
            }
        }

        private static AccessToken _fanfouAccessToken;
        public static AccessToken FanfouAccessToken
        {
            get
            {
                if (_fanfouAccessToken == null)
                {
                    string path = string.Format(ACCESS_TOKEN_PREFIX, SocialType.Fanfou.ToString());
                    _fanfouAccessToken = JsonHelper.LoadJson<AccessToken>(path) as AccessToken;
                }
                return _fanfouAccessToken;
            }
            set
            {
                _fanfouAccessToken = value;
            }
        }

        public static ClientInfo Client { get; set; }
        #endregion
        
        public static void LogOff(SocialType type)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string path = string.Format(ACCESS_TOKEN_PREFIX, type);
                if (store.FileExists(path))
                {
                    store.DeleteFile(path);
                }
                if (type == SocialType.Weibo)
                {
                    WeiboAccessToken = null;
                }
                else if (type == SocialType.Tencent)
                {
                    TencentAccessToken = null;
                }
                else if (type == SocialType.Renren)
                {
                    RenrenAccessToken = null;
                }
                else if (type == SocialType.Douban)
                {
                    DoubanAccessToken = null;
                }
                else if (type == SocialType.Fanfou)
                {
                    FanfouAccessToken = null;
                }
            }
        }        

        /// <summary>
        /// 发送不带图片的微博
        /// 史坤
        /// 2012.7.4
        /// </summary>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="action"></param>
        public static void UpdateStatus(SocialType type, string status, Action<bool, Exception> action)
        {
            HttpUpdate uploader = new HttpUpdate();
            uploader.Type = type;
            uploader.parameters = new Dictionary<string, object>();
            switch (type)
            {
                case SocialType.Weibo:
                    {
                        uploader.url = "https://api.weibo.com/2/statuses/update.json";
                        uploader.parameters.Add("status", status);
                        uploader.parameters.Add("access_token", WeiboAccessToken.Token);
                    }
                    break;
                case SocialType.Tencent:
                    {
                        uploader.url = "https://open.t.qq.com/api/t/add";
                        uploader.parameters.Add("oauth_consumer_key", Client.ClientId);
                        uploader.parameters.Add("access_token", TencentAccessToken.Token);
                        uploader.parameters.Add("openid", TencentAccessToken.OpenId);
                        uploader.parameters.Add("oauth_version", "2.a");
                        uploader.parameters.Add("scope", "all");
                        uploader.parameters.Add("format", "json");
                        uploader.parameters.Add("content", status);
                    }
                    break;
                case SocialType.Renren:
                    {
                        uploader.url = "http://api.renren.com/restserver.do";
                        uploader.parameters.Add("method", "feed.publishFeed");
                        uploader.parameters.Add("v", "1.0");
                        uploader.parameters.Add("access_token", RenrenAccessToken.Token);
                        uploader.parameters.Add("format", "JSON");
                        uploader.parameters.Add("name", " ");
                        uploader.parameters.Add("message", status);
                        uploader.parameters.Add("description", " ");
                        uploader.parameters.Add("url", " ");

                        List<APIParameter> para = new List<APIParameter>();
                        para.Add(new APIParameter("access_token", RenrenAccessToken.Token));
                        para.Add(new APIParameter("method", "feed.publishFeed"));
                        para.Add(new APIParameter("v", "1.0"));
                        para.Add(new APIParameter("name", " "));
                        para.Add(new APIParameter("description", " "));
                        para.Add(new APIParameter("message", status));
                        para.Add(new APIParameter("url", " "));
                        para.Add(new APIParameter("format", "JSON"));
                        
                        uploader.parameters.Add("sig", CalSig(para));
                    }
                    break;
                case SocialType.Douban:
                    {
                        uploader.url = "https://api.douban.com/shuo/v2/statuses/";
                        uploader.parameters.Add("source", Client.ClientId);
                        uploader.parameters.Add("text", status);
                    }
                    break;
                default:
                    break;
            }
            uploader.Submit();

            uploader.UploadCompleted += (e1, e2) =>
            {
                if (e1 is string && e1.ToString() == "ok")
                {
                    action(true, null);
                }
                else
                {
                    action(false, e1 as Exception);
                }
            };
        }

        #region Renren Calculate Sig
        internal static string CalSig(List<APIParameter> parameters)
        {
            parameters.Sort(new ParameterComparer());
            StringBuilder sbList = new StringBuilder();
            foreach (APIParameter para in parameters)
            {
                sbList.AppendFormat("{0}={1}", para.Name, para.Value);
            }
            sbList.Append(Client.ClientSecret);
            return MD5CryptoServiceProvider.GetMd5String((sbList.ToString()));
        }
        #endregion
    }
}
