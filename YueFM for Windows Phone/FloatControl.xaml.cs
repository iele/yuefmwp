using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace YueFM.Controls
{
    public partial class FloatControl : UserControl
    {
        public event FloatInStoryboardEvent FloatInStoryboardHandler;
        public delegate void FloatInStoryboardEvent(Object sender, EventArgs b);
        public event FloatOutStoryboardEvent FloatOutStoryboardHandler;
        public delegate void FloatOutStoryboardEvent(Object sender, EventArgs b);


        public event NextImageTapEvent NextImageTapEventHandler;
        public delegate void NextImageTapEvent(Object sender, EventArgs b);
        public event FavImageTapEvent FavImageTapEventHandler;
        public delegate void FavImageTapEvent(Object sender, EventArgs b);

        public FloatControl()
        {
            InitializeComponent();

            this.LayoutRoot.RenderTransform = this.TranslateTransform;
        }

        private void FloatInStoryboard_Completed(object sender, EventArgs e)
        {
            this.FloatInStoryboardHandler(sender, e);
        }

        private void FloatOutStoryboard_Completed(object sender, EventArgs e)
        {
            this.FloatOutStoryboardHandler(sender, e);
        }

        private void nextImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
             NextImageTapEventHandler( sender,  e);
        }

        private void favImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FavImageTapEventHandler(sender, e);
        }
    }
}
