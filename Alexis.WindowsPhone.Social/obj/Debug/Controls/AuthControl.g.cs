﻿#pragma checksum "C:\Users\Elephas\SkyDrive\YueFM for Windows Phone 8\Alexis.WindowsPhone.Social\Controls\AuthControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CDF92020EF8C6C2CF4599F2946846135"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18046
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Alexis.WindowsPhone.Social {
    
    
    public partial class AuthControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.WebBrowser webbrowser;
        
        internal System.Windows.Controls.Grid grdLoading;
        
        internal System.Windows.Controls.Image imgLogo;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Alexis.WindowsPhone.Social;component/Controls/AuthControl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.webbrowser = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("webbrowser")));
            this.grdLoading = ((System.Windows.Controls.Grid)(this.FindName("grdLoading")));
            this.imgLogo = ((System.Windows.Controls.Image)(this.FindName("imgLogo")));
        }
    }
}

