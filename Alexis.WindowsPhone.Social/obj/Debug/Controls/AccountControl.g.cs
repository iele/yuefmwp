﻿#pragma checksum "C:\Users\Elephas\SkyDrive\YueFM for Windows Phone 8\Alexis.WindowsPhone.Social\Controls\AccountControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8C7C71E513E6E5F2C6A3C6951C03F719"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18046
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class AccountControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock tbkWeibo;
        
        internal System.Windows.Controls.Button btnWeibo;
        
        internal System.Windows.Controls.TextBlock tbkTencent;
        
        internal System.Windows.Controls.Button btnTencent;
        
        internal System.Windows.Controls.TextBlock tbkDouban;
        
        internal System.Windows.Controls.Button btnDouban;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Alexis.WindowsPhone.Social;component/Controls/AccountControl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tbkWeibo = ((System.Windows.Controls.TextBlock)(this.FindName("tbkWeibo")));
            this.btnWeibo = ((System.Windows.Controls.Button)(this.FindName("btnWeibo")));
            this.tbkTencent = ((System.Windows.Controls.TextBlock)(this.FindName("tbkTencent")));
            this.btnTencent = ((System.Windows.Controls.Button)(this.FindName("btnTencent")));
            this.tbkDouban = ((System.Windows.Controls.TextBlock)(this.FindName("tbkDouban")));
            this.btnDouban = ((System.Windows.Controls.Button)(this.FindName("btnDouban")));
        }
    }
}
