﻿#pragma checksum "C:\Users\Elephas\SkyDrive\YueFM for Windows Phone 8\YueFM for Windows Phone\AccountItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "65EB0172068C1A4916C30CE2E4BEEA45"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18051
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


namespace YueFM.Controls {
    
    
    public partial class AccountItem : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard SelectedStoryBoard;
        
        internal System.Windows.Media.TransformGroup StackPanelTransformGroup;
        
        internal System.Windows.Media.ScaleTransform StackPanelScaleTransform;
        
        internal System.Windows.Media.TranslateTransform StackPanelTranslateTransform;
        
        internal System.Windows.Controls.StackPanel LayoutRoot;
        
        internal System.Windows.Controls.TextBlock textBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/YueFM%20for%20Windows%20Phone%208;component/AccountItem.xaml", System.UriKind.Relative));
            this.SelectedStoryBoard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("SelectedStoryBoard")));
            this.StackPanelTransformGroup = ((System.Windows.Media.TransformGroup)(this.FindName("StackPanelTransformGroup")));
            this.StackPanelScaleTransform = ((System.Windows.Media.ScaleTransform)(this.FindName("StackPanelScaleTransform")));
            this.StackPanelTranslateTransform = ((System.Windows.Media.TranslateTransform)(this.FindName("StackPanelTranslateTransform")));
            this.LayoutRoot = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot")));
            this.textBlock = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock")));
        }
    }
}

