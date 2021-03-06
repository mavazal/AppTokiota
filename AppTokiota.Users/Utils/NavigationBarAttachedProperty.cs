﻿using System;
using AppTokiota.Users.Components.BaseNavigation;
using Xamarin.Forms;

namespace AppTokiota.Users.Utils
{
    public static class NavigationBarAttachedProperty
    {
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.CreateAttached(
                "TextColor",
                typeof(Color),
                typeof(NavigationBarAttachedProperty),
                Color.Default);

        public static Color GetTextColor(BindableObject view)
        {
            return (Color)view.GetValue(TextColorProperty);
        }

        public static void SetTextColor(BindableObject view, Color value)
        {
            view.SetValue(TextColorProperty, value);

            var page = view as Page;
            var parent = page?.Parent as BaseNavigationPage;
            parent?.ApplyNavigationTextColor(page);
        }

        public static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.CreateAttached(
                "BackgroundColor",
                typeof(Color),
                typeof(NavigationBarAttachedProperty),
                Color.Default);

        public static Color GetBackgroundColor(BindableObject view)
        {
            return (Color)view.GetValue(BackgroundColorProperty);
        }

        public static void SetBackgroundColor(BindableObject view, Color value)
        {
            view.SetValue(BackgroundColorProperty, value);

            var page = view as Page;
            var parent = page?.Parent as BaseNavigationPage;
            parent?.ApplyNavigationBackgroundColor(page);
        }
    }
}
