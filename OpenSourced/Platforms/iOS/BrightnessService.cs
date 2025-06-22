using UIKit;

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
        public float GetBrightness()
        {
            try
            {
                return (float)UIScreen.MainScreen.Brightness;
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
                var prevBrightness = (float)UIScreen.MainScreen.Brightness;
                UIScreen.MainScreen.Brightness = brightness;
                return prevBrightness;
            }
            catch
            {
                return 0;
            }
        }
    }
}