﻿#pragma checksum "C:\Users\Elephas\Documents\GitHub\yuefmwp\YueFM for Windows Phone\SettingPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "96BDA7F0363C398B5CF05691A7A4FCD3"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34014
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Alexis.WindowsPhone.Social;
using Microsoft.Live.Controls;
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
    
    
    public partial class SettingPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot Pivot;
        
        internal System.Windows.Controls.TextBlock AccountTextBlock;
        
        internal System.Windows.Controls.Button LogoutButton;
        
        internal System.Windows.Controls.TextBlock SkyDriveTextBlock;
        
        internal Microsoft.Live.Controls.SignInButton ButtonSignin;
        
        internal Alexis.WindowsPhone.Social.AccountControl accountControl;
        
        internal Microsoft.Phone.Controls.ListPicker FontListPicker;
        
        internal System.Windows.Controls.TextBlock SizeTextBlock;
        
        internal System.Windows.Controls.Slider SliderFontSize;
        
        internal Microsoft.Phone.Controls.ToggleSwitch ImageToggleSwitch;
        
        internal Microsoft.Phone.Controls.ToggleSwitch NightToggleSwitch;
        
        internal System.Windows.Controls.StackPanel NightStackPanel;
        
        internal System.Windows.Media.Animation.Storyboard NightStackPanelFadeInStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard NightStackPanelFadeOutStoryboard;
        
        internal System.Windows.Media.TransformGroup NightStackPanelTransformGroup;
        
        internal System.Windows.Media.ScaleTransform NightStackPanelScaleTransform;
        
        internal System.Windows.Controls.TextBlock NightTextBlock;
        
        internal System.Windows.Controls.Slider SliderNight;
        
        internal Microsoft.Phone.Controls.ToggleSwitch QuitToggleSwitch;
        
        internal Microsoft.Phone.Controls.ToggleSwitch crashToggleSwitch;
        
        internal System.Windows.Controls.Button marketButton;
        
        internal System.Windows.Controls.Button mailButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/YueFM%20for%20Windows%20Phone%208;component/SettingPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Pivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("Pivot")));
            this.AccountTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("AccountTextBlock")));
            this.LogoutButton = ((System.Windows.Controls.Button)(this.FindName("LogoutButton")));
            this.SkyDriveTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("SkyDriveTextBlock")));
            this.ButtonSignin = ((Microsoft.Live.Controls.SignInButton)(this.FindName("ButtonSignin")));
            this.accountControl = ((Alexis.WindowsPhone.Social.AccountControl)(this.FindName("accountControl")));
            this.FontListPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("FontListPicker")));
            this.SizeTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("SizeTextBlock")));
            this.SliderFontSize = ((System.Windows.Controls.Slider)(this.FindName("SliderFontSize")));
            this.ImageToggleSwitch = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("ImageToggleSwitch")));
            this.NightToggleSwitch = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("NightToggleSwitch")));
            this.NightStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("NightStackPanel")));
            this.NightStackPanelFadeInStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("NightStackPanelFadeInStoryboard")));
            this.NightStackPanelFadeOutStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("NightStackPanelFadeOutStoryboard")));
            this.NightStackPanelTransformGroup = ((System.Windows.Media.TransformGroup)(this.FindName("NightStackPanelTransformGroup")));
            this.NightStackPanelScaleTransform = ((System.Windows.Media.ScaleTransform)(this.FindName("NightStackPanelScaleTransform")));
            this.NightTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NightTextBlock")));
            this.SliderNight = ((System.Windows.Controls.Slider)(this.FindName("SliderNight")));
            this.QuitToggleSwitch = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("QuitToggleSwitch")));
            this.crashToggleSwitch = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("crashToggleSwitch")));
            this.marketButton = ((System.Windows.Controls.Button)(this.FindName("marketButton")));
            this.mailButton = ((System.Windows.Controls.Button)(this.FindName("mailButton")));
        }
    }
}

