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
    internal class SelectableLabelHandler: LabelHandler
    {
        protected override void ConnectHandler(MauiLabel platformView)
        {
            base.ConnectHandler(platformView);
            platformView.UserInteractionEnabled = true;
            var longPressGesture = new UILongPressGestureRecognizer(gesture =>
            {
                if (gesture.State == UIGestureRecognizerState.Began)
                {
                    var menu = UIMenuController.SharedMenuController;
                    if (!menu.MenuVisible)
                    {
                        platformView.BecomeFirstResponder();
                        menu.SetTargetRect(platformView.Bounds, platformView);
                        menu.SetMenuVisible(true, true);
                    }
                }
            });
            platformView.AddGestureRecognizer(longPressGesture);
        }

        protected override MauiLabel CreatePlatformView()
        {
            return new SelectableUILabel();
        }

        private class SelectableUILabel : MauiLabel
        {
            public override bool CanBecomeFirstResponder => true;

            public override bool CanPerform(ObjCRuntime.Selector action, Foundation.NSObject withSender)
            {
                return action.Name is "copy:";
            }

            public override void Copy(Foundation.NSObject sender)
            {
                UIPasteboard.General.String = Text;
            }
        }
    }
}