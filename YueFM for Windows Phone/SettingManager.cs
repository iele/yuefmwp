using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.Live;

namespace YueFM.Managers
{
    public sealed class SettingManager
    {
        private Boolean _article_is_image;
        public Boolean article_is_image
        {
            get
            {
                return _article_is_image;
            }
            set
            {
                _article_is_image = value;
                settings["article_is_image"] = value;

                is_setting_changed = true;
            }
        }

        private String _article_size;
        public String article_size
        {
            get
            {
                return _article_size;
            }
            set
            {
                _article_size = value;
                settings["article_size"] = value;

                is_setting_changed = true;
            }
        }

        private String _article_font;
        public String article_font
        {
            get
            {
                return _article_font;
            }
            set
            {
                _article_font = value;
                settings["article_font"] = value;

                is_setting_changed = true;
            }
        }

        private Boolean _crash_report;
        public Boolean crash_report
        {
            get
            {
                return _crash_report;
            }
            set
            {
                _crash_report = value;
                settings["crash_report_2"] = value;
            }
        }

        private Boolean _night_mode;
        public Boolean night_mode
        {
            get
            {
                return _night_mode;
            }
            set
            {
                _night_mode = value;
                settings["night_mode"] = value;

                is_setting_changed = true;
            }
        }

        private Boolean _touch_enable;
        public Boolean touch_enable
        {
            get
            {
                return _touch_enable;
            }
            set
            {
                _touch_enable = value;
                settings["touch_enable"] = value;

                is_setting_changed = true;
            }
        }

        private Boolean _quit_confirm;
        public Boolean quit_confirm
        {
            get
            {
                return _quit_confirm;
            }
            set
            {
                _quit_confirm = value;
                settings["quit_confirm"] = value;

                is_setting_changed = true;
            }
        }

        private UInt32 _read_count;
        public UInt32 read_count
        {
            get
            {
                return _read_count;
            }
            set
            {
                _read_count = value;
                settings["read_count"] = value;
            }
        }

        private String _last_read;
        public String last_read
        {
            get
            {
                return _last_read;
            }
            set
            {
                _last_read = value;
                settings["last_read"] = value;
            }
        }

        private UInt32 _like_count;
        public UInt32 like_count
        {
            get
            {
                return _like_count;
            }
            set
            {
                _like_count = value;
                settings["like_count"] = value;
            }
        }

        private String _night_light;
        public String night_light
        {
            get
            {
                return _night_light;
            }
            set
            {
                _night_light = value;
                settings["night_light"] = value;
            }
        }

        private Boolean _skydrive_login;
        public Boolean skydrive_login
        {
            get
            {
                return _skydrive_login;
            }
            set
            {
                _skydrive_login = value;
                settings["skydrive_login"] = value;

                is_setting_changed = true;
            }
        }

        public Boolean is_setting_changed { get; set; }

        private static readonly SettingManager instance = new SettingManager();

        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        private SettingManager()
        {
        }

        public static SettingManager GetInstance()
        {
            return instance;
        }

        public void SaveSettings()
        {
            settings["article_is_image"] = this.article_is_image;
            settings["article_size"] = this.article_size;
            settings["article_font"] = this.article_font;
            settings["crash_report_2"] = this.crash_report;
            settings["read_count"] = this.read_count;
            settings["like_count"] = this.like_count;
            settings["night_mode"] = this.night_mode;
            settings["touch_enable"] = this.touch_enable;
            settings["quit_confirm"] = this.quit_confirm;
            settings["last_read"] = this.last_read;
            settings["night_light"] = this.night_light;
            settings["skydrive_login"] = this.skydrive_login
        ;
            settings.Save();
        }

        public void RestoreSettings()
        {
            Object article_is_image, article_size, article_font, crash_report, read_count, like_count, night_mode, touch_enable, quit_confirm, last_read, night_light, skydrive_login;

            if (settings.TryGetValue("article_is_image", out article_is_image) && article_is_image != null)
                this.article_is_image = (Boolean)article_is_image;
            else
                this.article_is_image = true;

            if (settings.TryGetValue("article_size", out article_size) && article_size != null)
                this.article_size = article_size as String;
            else
                this.article_size = "22";

            if (settings.TryGetValue("article_font", out article_font) && article_font != null)
                this.article_font = article_font as String;
            else
                this.article_font = "Microsoft YaHei";

            if (settings.TryGetValue("crash_report_2", out crash_report) && crash_report != null)
                this.crash_report = (Boolean)crash_report;
            else
                this.crash_report = true;

            if (settings.TryGetValue("read_count", out read_count) && read_count != null)
                this.read_count = (UInt32)read_count;
            else
                this.read_count = 0;

            if (settings.TryGetValue("like_count", out like_count) && like_count != null)
                this.like_count = (UInt32)like_count;
            else
                this.like_count = 0;

            if (settings.TryGetValue("night_mode", out night_mode) && night_mode != null)
                this.night_mode = (Boolean)night_mode;
            else
                this.night_mode = false;

            if (settings.TryGetValue("touch_enable", out touch_enable) && touch_enable != null)
                this.touch_enable = (Boolean)touch_enable;
            else
                this.touch_enable = false;

            if (settings.TryGetValue("quit_confirm", out quit_confirm) && quit_confirm != null)
                this.quit_confirm = (Boolean)quit_confirm;
            else
                this.quit_confirm = true;

            if (settings.TryGetValue("last_read", out last_read) && last_read != null)
                this.last_read = (String)last_read;
            else
                this.last_read = "暂无";

            if (settings.TryGetValue("night_light", out night_light) && night_light != null)
                this.night_light = (String)night_light;
            else
                this.night_light = "50";

            if (settings.TryGetValue("skydrive_login", out skydrive_login) && skydrive_login != null)
                this.skydrive_login = (Boolean)skydrive_login;
            else
                this.skydrive_login = false;
        }
    }
}
