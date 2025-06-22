using System;
using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
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
    public class CustomDatePickerHandler : DatePickerHandler
    {
        protected override MauiDatePicker CreatePlatformView()
        {
            return new CustomDatePicker();
        }

        // Reference: https://github.com/dotnet/maui/blob/5bb5ed2ec93cecfd1cb6094677ea88fa39924214/src/Core/src/Platform/iOS/MauiDatePicker.cs
        private class CustomDatePicker : MauiDatePicker
        {
            public CustomDatePicker()
            {
                var picker = InputView as UIDatePicker;
                if (picker == null)
                {
                    picker = new UIDatePicker
                    {
                        Mode = UIDatePickerMode.Date,
                        TimeZone = new NSTimeZone("UTC")
                    };
                    InputView = picker;
                }

                // Reference: https://developer.apple.com/documentation/uikit/uidatepickerstyle
#pragma warning disable CA1416
                if (OperatingSystem.IsIOSVersionAtLeast(14))
                {
                    picker!.PreferredDatePickerStyle = UIDatePickerStyle.Inline;
                } 
                else if (OperatingSystem.IsIOSVersionAtLeast(13, 4))
                {
                    picker!.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
                }
#pragma warning restore CA1416
            }
        }
    }
}
