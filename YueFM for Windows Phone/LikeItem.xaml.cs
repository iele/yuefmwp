using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace YueFM.Controls
{public partial class LikeItem : UserControl
    {
        public Action StoryBoardCompleted { get; set; }

        public LikeItem()
        {
            InitializeComponent();

            this.RenderTransform = this.StackPanelTransformGroup;
            this.image.RenderTransform = this.imageTransformGroup;
            this.text.RenderTransform = this.textTransformGroup;
        }

        private void StackPanel_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            StackPanelScaleTransform.ScaleX = 0.95;
            StackPanelScaleTransform.ScaleY = 0.95;
        }

        private void StackPanel_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            StackPanelScaleTransform.ScaleX = 0.95;
            StackPanelScaleTransform.ScaleY = 0.95;
        }

        private void StackPanel_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            StackPanelScaleTransform.ScaleX = 1;
            StackPanelScaleTransform.ScaleY = 1;
        }

        private void SelectedStoryBoard_Completed(object sender, EventArgs e)
        {
            StoryBoardCompleted();
        }
    }
}
