﻿#pragma checksum "C:\Users\Elephas\Documents\GitHub\yuefmwp\YueFM for Windows Phone\LikesPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5B90BAEBD0686EC9C9C69A1841F37D89"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34011
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


namespace YueFM.Pages {
    
    
    public partial class LikesPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar progressBar;
        
        internal Microsoft.Phone.Controls.Pivot Pivot;
        
        internal System.Windows.Controls.ListBox listBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/YueFM%20for%20Windows%20Phone%208;component/LikesPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.progressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("progressBar")));
            this.Pivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("Pivot")));
            this.listBox = ((System.Windows.Controls.ListBox)(this.FindName("listBox")));
        }
    }
}

