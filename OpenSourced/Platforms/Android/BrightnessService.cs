using Android.Views;

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
    public class BrightnessService : IBrightnessService
    {
        private WindowManagerLayoutParams GetAttributesWindow()
        {
            var window = Platform.CurrentActivity.Window;
            var attributesWindow = new WindowManagerLayoutParams();
            attributesWindow.CopyFrom(window.Attributes);
            return attributesWindow;
        }

        public float GetBrightness()
        {
            try
            {
                var attributesWindow = GetAttributesWindow();
                return attributesWindow.ScreenBrightness;
            }
            catch
            {
                return 0;
            }
        }

        public float SetBrightness(float brightness)
        {
            try
            {
                var attributesWindow = GetAttributesWindow();
                var oldBrightness = attributesWindow.ScreenBrightness;
                attributesWindow.ScreenBrightness = brightness;
                Platform.CurrentActivity.Window.Attributes = attributesWindow;

                return oldBrightness;
            }
            catch
            {
                return 0;
            }
        }
    }
}