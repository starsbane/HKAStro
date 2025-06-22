using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Application = Microsoft.Maui.Controls.Application;

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
        private Android.Views.View _nativeView;

        private Dialog _dialog;

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

                var renderer =
                    Microsoft.Maui.Controls.Compatibility.Platform.Android.Platform.CreateRendererWithContext(
                        loadingIndicatorPage, Platform.CurrentActivity);
                _nativeView = renderer.View;

                this._dialog = new Dialog(Platform.CurrentActivity);
                _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                _dialog.SetCancelable(false);
                _dialog.SetContentView(_nativeView);
                var window = _dialog.Window;
                window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                window.ClearFlags(WindowManagerFlags.DimBehind);
                window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));

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
                _dialog?.Show();
            });
        }

        public void HideLoadingPage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Hide the page
                _dialog?.Hide();
            });
        }
    }
}