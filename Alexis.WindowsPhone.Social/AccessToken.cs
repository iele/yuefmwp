// ===============================================================================
// AccessToken.cs
// Description:OAuth2 AccessToken
// Author:  Alexis
// Date:    2012.05.26
// Blog: http://alexis.cnblogs.com
// ===============================================================================
// Copyright (c) Alexis. 
// All rights reserved.
// ===============================================================================

using System;
namespace Alexis.WindowsPhone.Social
{
    public class AccessToken
    {
        public string Token { get; set; }

        public DateTime ExpiresTime { get; set; }

        public string UserInfo { get; set; }

        public string OpenId { get; set; }

        public bool IsExpired
        {
            get
            {
                return ExpiresTime.AddSeconds(30) <= DateTime.Now;
            }
        }
    }
}
