using AndroidX.Activity;

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
    internal class BackPressedCallback : OnBackPressedCallback
    {
        readonly MauiAppCompatActivity _activity;

        public BackPressedCallback(MauiAppCompatActivity activity)
            : base(true)
        {
            _activity = activity;
        }

        public override void HandleOnBackPressed()
        {
            var nav = Shell.Current?.Navigation;
            if (nav != null && nav.NavigationStack.Count > 1)
            {
                Shell.Current?.GoToAsync("..");
            }
            else
            {
                // No back pages — exit app
                _activity.Finish();
            }
        }
    }
}