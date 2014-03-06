using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YueFM.Controls
{
    public partial class AccountItem : UserControl
    {
        public Action StoryBoardCompleted;

        public AccountItem()
        {
            InitializeComponent();

            this.RenderTransform = this.StackPanelTransformGroup;
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

        public void SelectedItem(Action storyBoardCompleted)
        {
            this.StoryBoardCompleted = storyBoardCompleted;
            this.SelectedStoryBoard.Begin();
        }

        private void SelectedStoryBoard_Completed(object sender, EventArgs e)
        {
            StoryBoardCompleted();
        }
    }
}
