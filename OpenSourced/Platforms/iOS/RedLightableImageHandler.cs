using CommunityToolkit.Mvvm.Messaging;
using CoreGraphics;
using CoreImage;
using HKAstro.UX;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Handlers;
using UIKit;
using PImageView = UIKit.UIImageView;

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
    public partial class RedLightableImageHandler : ViewHandler<RedLightableImage, PImageView>
    {
        protected override PImageView CreatePlatformView()
        {
            return new PImageView(CGRect.Empty)
            {
                ContentMode = UIViewContentMode.ScaleAspectFit,
                ClipsToBounds = true
            };
        }

        protected override void ConnectHandler(PImageView platformView)
        {
            base.ConnectHandler(platformView);
            WeakReferenceMessenger.Default.Register<RedLightableImageHandler, RedLightModeChangedMessage>(this,
                (recipient, message) => { RefreshPlatformView(this, this.VirtualView, message.Value); });
        }

        protected override void DisconnectHandler(PImageView platformView)
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
                var bitmap = await GetUIImageAsync(view.Source);
                var bmp = (isRedLightMode) ? GetRedOnlyUIImage(bitmap) : bitmap;
                handler.PlatformView.Image = bmp;
            }
        }

        private static async Task<UIImage> GetUIImageAsync(ImageSource source)
        {
            IImageSourceHandler handler = null;
            UIImage returnValue = null;
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
                    returnValue = await handler.LoadImageAsync(source);
            }
            catch (TaskCanceledException)
            {
            }

            return returnValue;
        }

        private static UIImage GetRedOnlyUIImage(UIImage src)
        {
#if !DEBUG
            if (src == null) return new UIImage();

            try
            {
#endif
            var redColorEffect = new CIColorMatrix()
            {
                InputImage = new CIImage(src),
                RVector = new CIVector(1, 1, 1, 0),
                GVector = new CIVector(0, 0, 0, 0),
                BVector = new CIVector(0, 0, 0, 0),
                AVector = new CIVector(0, 0, 1, 0),
            };

            var holder = redColorEffect.OutputImage;
            var extent = holder.Extent;
            var context = CIContext.FromOptions(null);
            var cgImage = context.CreateCGImage(holder, extent);
            var fixedImage = UIImage.FromImage(cgImage, src.CurrentScale, src.Orientation);
            return fixedImage;
#if !DEBUG
        }
            catch (ObjectDisposedException ex)
            {
                return new UIImage();
            }
#endif
        }
    }
}