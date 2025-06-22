using Android.Graphics;
using AndroidX.AppCompat.Widget;
using CommunityToolkit.Mvvm.Messaging;
using HKAstro.UX;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using ColorMatrixColorFilter = Android.Graphics.ColorMatrixColorFilter;
using Paint = Android.Graphics.Paint;

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
    public partial class RedLightableImageHandler : ViewHandler<RedLightableImage, AppCompatImageView>
    {
        protected override AppCompatImageView CreatePlatformView()
        {
            return new AppCompatImageView(Context);
        }

        protected override void ConnectHandler(AppCompatImageView platformView)
        {
            base.ConnectHandler(platformView);
            WeakReferenceMessenger.Default.Register<RedLightableImageHandler, RedLightModeChangedMessage>(this,
                (recipient, message) => { RefreshPlatformView(this, this.VirtualView, message.Value); });
        }

        protected override void DisconnectHandler(AppCompatImageView platformView)
        {
            WeakReferenceMessenger.Default.Unregister<RedLightModeChangedMessage>(this);
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }

        public static void MapImageSource(RedLightableImageHandler handler, RedLightableImage view)
        {
            RefreshPlatformView(handler, view, UserPreferenceHelper.Get<bool>(PreferenceKey.EnableRedLightMode));
        }

        private static async Task RefreshPlatformView(RedLightableImageHandler handler, RedLightableImage view,
            bool isRedLightMode)
        {
            if (view.Source != null)
            {
                var bitmap = await GetBitmapAsync(view.Source, handler.Context);
                var bmp = (isRedLightMode) ? GetRedOnlyBitmap(bitmap) : bitmap;
                handler.PlatformView.SetImageBitmap(bmp);
            }
        }

        private static async Task<Bitmap> GetBitmapAsync(ImageSource source, Android.Content.Context context)
        {
            IImageSourceHandler handler = null;
            Bitmap returnValue = null;
            try
            {
                if (source is UriImageSource)
                {
                    handler = new ImageLoaderSourceHandler();
                }
                else if (source is FileImageSource)
                {
                    handler = new FileImageSourceHandler();
                }
                else if (source is StreamImageSource)
                {
                    handler = new StreamImagesourceHandler();
                }

                if (handler != null)
                    returnValue = await handler.LoadImageAsync(source, context);
            }
            catch (TaskCanceledException)
            {
            }

            return returnValue;
        }

        private static Bitmap GetRedOnlyBitmap(Bitmap src)
        {
#if !DEBUG
            if (src == null) return Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888);

            try
            {
#endif
            var newBmp = src.Copy(Bitmap.Config.Argb8888, true);

            var canvasResult = new Canvas(newBmp);
            var paint = new Paint();
            var colorMatrix = new ColorMatrix([
                1, 1, 1, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 1, 0
            ]);

            var filter = new ColorMatrixColorFilter(colorMatrix);
            paint.SetColorFilter(filter);
            canvasResult.DrawBitmap(newBmp, 0, 0, paint);

            return newBmp;
#if !DEBUG
            } 
            catch (ObjectDisposedException ex)
            {
                return Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888);
            }
#endif
        }
    }
}