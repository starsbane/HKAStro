using UIKit;
using Platform = Microsoft.Maui.Controls.Compatibility.Platform.iOS.Platform;

// HKAStro
// Copyright (C) 2025  Alex Man (Starsbane)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
namespace HKAstro
{
    public class LoadingPageService : ILoadingPageService
    {
        private UIView _nativeView;

        private bool _isInitialized;

        public void InitLoadingPage(ContentPage loadingIndicatorPage)
        {
            // check if the page parameter is available
            if (loadingIndicatorPage != null)
            {
                // build the loading page with native base
                loadingIndicatorPage.Parent = Application.Current.MainPage;

                loadingIndicatorPage.Layout(new Rect(0, 0,
                    Application.Current.MainPage.Width,
                    Application.Current.MainPage.Height));

                var renderer = Platform.CreateRenderer(loadingIndicatorPage);
                _nativeView = renderer.NativeView;

                _isInitialized = true;
            }
        }

        public void ShowLoadingPage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // check if the user has set the page or not
                if (!_isInitialized)
                    InitLoadingPage(new LoadingIndicatorPage()); // set the default page

                // showing the native loading page
                if (_nativeView != null)
                {
                    UIApplication.SharedApplication.KeyWindow.AddSubview(_nativeView);
                }
            });
        }

        public void HideLoadingPage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Hide the page
                _nativeView?.RemoveFromSuperview();
            });
        }
    }
}