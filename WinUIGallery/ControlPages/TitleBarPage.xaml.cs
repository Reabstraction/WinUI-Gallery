//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using AppUIBasics.Helper;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUIGallery.DesktopWap.Helper;
using Microsoft.UI.Xaml.Shapes;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppUIBasics.ControlPages
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TitleBarPage : Page
    {
        private Windows.UI.Color currentBgColor = Colors.Transparent;
        private Windows.UI.Color currentFgColor = Colors.Black;

        public TitleBarPage()
        {
            this.InitializeComponent();
            Loaded += (object sender, RoutedEventArgs e) =>
            {
                (sender as TitleBarPage).UpdateTitleBarColor();
                UpdateButtonText();
            };
        }


        private void SetTitleBar(UIElement titlebar)
        {
            var window = WindowHelper.GetWindowForElement(this as UIElement);
            if (!window.AppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                var appTitleBar = window.AppWindow.TitleBar;
                appTitleBar.ExtendsContentIntoTitleBar = true;
                appTitleBar.BackgroundColor = Microsoft.UI.Colors.Transparent;
                appTitleBar.ButtonBackgroundColor = Microsoft.UI.Colors.Transparent;
                appTitleBar.ButtonInactiveBackgroundColor = Microsoft.UI.Colors.Transparent;
                window.SetTitleBar(titlebar);
            }
            else
            {
                var appTitleBar = window.AppWindow.TitleBar;
                appTitleBar.ExtendsContentIntoTitleBar = false;
                appTitleBar.BackgroundColor = null;
                appTitleBar.ButtonBackgroundColor = null;
                appTitleBar.ButtonInactiveBackgroundColor = null;
                window.SetTitleBar(null);

            }
            UpdateButtonText();
            UpdateTitleBarColor();
        }

        public void UpdateButtonText()
        {
            var window = WindowHelper.GetWindowForElement(this as UIElement);
            if (window.AppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                customTitleBar.Content = "Reset to system TitleBar";
                defaultTitleBar.Content = "Reset to system TitleBar";
            }
            else
            {
                customTitleBar.Content = "Set Custom TitleBar";
                defaultTitleBar.Content = "Set Fallback Custom TitleBar";
            }
            if (window.AppWindow.TitleBar.PreferredHeightOption == Microsoft.UI.Windowing.TitleBarHeightOption.Tall)
            {
                sizeTitleBar.Content = "Set TitleBar to Standard Height";
            }
            else
            {
                sizeTitleBar.Content = "Set TitleBar to Tall Height";
            }
        }

        private void BgGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rect = (Rectangle)e.ClickedItem;
            var color = ((SolidColorBrush)rect.Fill).Color;
            BackgroundColorElement.Background = new SolidColorBrush(color);

            currentBgColor = color;
            UpdateTitleBarColor();

            // Delay required to circumvent GridView bug: https://github.com/microsoft/microsoft-ui-xaml/issues/6350
            Task.Delay(10).ContinueWith(_ => myBgColorButton.Flyout.Hide(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FgGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rect = (Rectangle)e.ClickedItem;
            var color = ((SolidColorBrush)rect.Fill).Color;

            ForegroundColorElement.Background = new SolidColorBrush(color);

            currentFgColor = color;
            UpdateTitleBarColor();

            // Delay required to circumvent GridView bug: https://github.com/microsoft/microsoft-ui-xaml/issues/6350
            Task.Delay(10).ContinueWith(_ => myFgColorButton.Flyout.Hide(), TaskScheduler.FromCurrentSynchronizationContext());
        }


        public void UpdateTitleBarColor()
        {
            var window = WindowHelper.GetWindowForElement(this);
            var titleBarElement = UIHelper.FindElementByName(this, "AppTitleBar");

            (titleBarElement as Border).Background = new SolidColorBrush(currentBgColor); // changing titlebar uielement's color
            TitleBarHelper.SetCaptionButtonColors(window, currentFgColor);
        }

        private void customTitleBar_Click(object sender, RoutedEventArgs e)
        {
            UIElement titleBarElement = UIHelper.FindElementByName(sender as UIElement, "AppTitleBar");
            SetTitleBar(titleBarElement);

            // announce visual change to automation
            UIHelper.AnnounceActionForAccessibility(sender as UIElement, "TitleBar size and width changed", "TitleBarChangedNotificationActivityId");
        }

        private void sizeTitleBar_Click(object sender, RoutedEventArgs e)
        {
            var window = WindowHelper.GetWindowForElement(this as UIElement);
            var appTitleBar = window.AppWindow.TitleBar;

            if(appTitleBar.PreferredHeightOption == Microsoft.UI.Windowing.TitleBarHeightOption.Tall)
            {
                appTitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Standard;
            }
            else
            {
                appTitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
            }

            UpdateButtonText();

            UIHelper.AnnounceActionForAccessibility(sender as UIElement, "TitleBar height option changed", "TitleBarChangedNotificationActivityId");
        }

        private void defaultTitleBar_Click(object sender, RoutedEventArgs e)
        {
            SetTitleBar(null);

            // announce visual change to automation
            UIHelper.AnnounceActionForAccessibility(sender as UIElement, "TitleBar size and width changed", "TitleBarChangedNotificationActivityId");
        }
    }
}
