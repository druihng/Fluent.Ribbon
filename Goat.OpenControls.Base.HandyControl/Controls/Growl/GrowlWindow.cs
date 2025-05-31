using System;
using System.Windows;
using System.Windows.Controls;
using Goat.OpenControls.Base.HandyControl.Tools;
using Goat.OpenControls.Base.HandyControl.Tools.Interop;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    public class GrowlWindow : Window
    {
        internal Panel GrowlPanel { get; set; }

        internal GrowlWindow()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;

            GrowlPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top
            };

            Content = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                IsInertiaEnabled = true,
                Content = GrowlPanel
            };
        }

        internal void Init()
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Height = desktopWorkingArea.Height;
            Left = desktopWorkingArea.Right - Width;
            Top = 0;
        }

        protected override void OnSourceInitialized(EventArgs e)
            => InteropMethods.IntDestroyMenu(this.GetHwndSource().CreateHandleRef());
    }
}
