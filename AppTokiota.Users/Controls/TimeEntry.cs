﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppTokiota.Users.Controls
{
    public class TimeEntry : ContentView
    {
        #region SelectedTimeCommand


        public static readonly BindableProperty SelectedTimeCommandProperty =
            BindableProperty.Create(nameof(SelectedTimeCommand), typeof(ICommand), typeof(TimeEntry), null);

        /// <summary>
        /// Gets or sets the Selected Time command.
        /// </summary>
        /// <value>The date command.</value>
        public ICommand SelectedTimeCommand
        {
            get { return (ICommand)GetValue(SelectedTimeCommandProperty); }
            set
            {
                SetValue(SelectedTimeCommandProperty, value);
            }
        }

        #endregion

        #region Content Visible

        public static readonly BindableProperty ContentViewVisibleProperty =
            BindableProperty.Create(nameof(ContentViewVisible), typeof(bool), typeof(TimeEntry), false, BindingMode.TwoWay,
                                    propertyChanged: (bindable, oldValue, newValue) =>
                                    {
                                        (bindable as TimeEntry).ChangeContentViewVisibleProperty((bool)newValue, (bool)oldValue);
                                    });

        protected void ChangeContentViewVisibleProperty(bool newValue, bool oldValue)
        {
            if (newValue == false)
            {
                Content = null;
                MainView.Children.Clear();
                Content = MainView;
            }
            else
            {
                ChangeOptionImputation(MainHours);
            }
        }


        /// <summary>
        /// Gets or sets the visibility  of the Content views.
        /// </summary>
        /// <value>The width of the selected border.</value>
        public bool ContentViewVisible
        {
            get { return (bool)GetValue(ContentViewVisibleProperty); }
            set { SetValue(ContentViewVisibleProperty, value); }
        }

        #endregion

        private ButtonTimeTask SelectedHour;
        private ButtonTimeTask SelectedMinute;
        private List<ButtonTimeTask> ButtonsHour;
        private List<ButtonTimeTask> ButtonsMinutes;
        private Grid MainHours;
        private Grid MainMinutes;
        private StackLayout MainView;
        private Dictionary<string, string> Response;

        public TimeEntry()
        {
            Response = new Dictionary<string, string>();
            Response.Add("Hour", "");
            Response.Add("Minute", "");

            CreatedHours();
            CreatedMinutes();
            DefaultHourAndMinuteSelected();

            MainView = new StackLayout
            {
                Padding = 0
            };

            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.FillAndExpand;
            this.Content = MainView;
        }

        private void DefaultHourAndMinuteSelected()
        {
            SelectedHour = (ButtonTimeTask)MainHours.Children.FirstOrDefault();
			SelectedHour.BackgroundColor = Color.FromHex("40baaa");
			SelectedHour.TextColor = Color.White;

            SelectedMinute = (ButtonTimeTask)MainMinutes.Children.FirstOrDefault();
			SelectedMinute.BackgroundColor = Color.FromHex("40baaa");
			SelectedMinute.TextColor = Color.White;
        }

        protected void CreatedHours()
        {

            var GridSpace = 1;
            var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) };
            var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) };

            MainHours = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                RowSpacing = GridSpace,
                ColumnSpacing = GridSpace
            };

            MainHours.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef, columDef };
            MainHours.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef };

            ButtonsHour = new List<ButtonTimeTask>();

            for (var fila = 0; fila < 5; fila++)
            {
                for (var i = 0; i <= 4; i++)
                {
                    ButtonsHour.Add(new ButtonTimeTask()
                    {
						CornerRadius = 0,
                        BorderWidth = 1,
                        BorderColor = Color.Gray,
						FontSize = Device.Idiom == TargetIdiom.Tablet ? 20:12,
                        BackgroundColor = Color.Transparent,
                        TextColor = Color.DarkSalmon,
                        Text = $"{i + (fila * 5)} h",
						WidthRequest = Device.Idiom == TargetIdiom.Tablet ? 100:50,
						HeightRequest = Device.Idiom == TargetIdiom.Tablet ? 100:50,
                        Value = (i + (fila * 5)).ToString(),
                    });
                    var b = ButtonsHour.Last();
                    b.Clicked += HourClickedEvent;

                    MainHours.Children.Add(b, i, fila);
                }
            }
        }

        protected void CreatedMinutes()
        {
            var GridSpace = 1;
            var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) };
            var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) };

            MainMinutes = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = GridSpace,
                ColumnSpacing = GridSpace,
                Padding = 1,
            };

            MainMinutes.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef };
            MainMinutes.RowDefinitions = new RowDefinitionCollection { rowDef };

            ButtonsMinutes = new List<ButtonTimeTask>();

            var count = 0;
            for (var i = 0; i <= 45; i = i + 15)
            {
                ButtonsMinutes.Add(new ButtonTimeTask()
                {
					CornerRadius = 0,
                    BorderWidth = 1,
                    BorderColor = Color.Gray,
					FontSize = Device.Idiom == TargetIdiom.Tablet ? 20 : 12,
                    BackgroundColor = Color.Transparent,
                    TextColor = Color.DarkSalmon,
                    Text = $"{i} m",
					WidthRequest = Device.Idiom == TargetIdiom.Tablet ? 100 : 50,
                    HeightRequest = Device.Idiom == TargetIdiom.Tablet ? 100 : 50,
                    Value = i.ToString(),
                });
                var b = ButtonsMinutes.Last();
                b.Clicked += MinuteClickedEvent;

                MainMinutes.Children.Add(b, count, 0);
                count++;
            }
        }

        public void ExecuteSelectedHandle()
        {
            ContentViewVisible = false;

            Response["Hour"] = SelectedHour.Value;
            Response["Minute"] = SelectedMinute.Value;

            SelectedTimeCommand?.Execute(Response);
        }

        protected void MinuteClickedEvent(object s, EventArgs a)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var selected = (s as ButtonTimeTask);
                if (SelectedMinute != null)
                {
					SelectedMinute.BackgroundColor = Color.Transparent;
					SelectedMinute.TextColor = Color.DarkSalmon;
                }
				selected.BackgroundColor = Color.FromHex("40baaa");
				selected.TextColor = Color.White;
                SelectedMinute = selected;

                ExecuteSelectedHandle();
            });
        }

        protected void HourClickedEvent(object s, EventArgs a)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var selectedHour = (s as ButtonTimeTask);
                if (SelectedHour != null)
                {
					SelectedHour.BackgroundColor = Color.Transparent;
					SelectedHour.TextColor = Color.DarkSalmon;
                }
				selectedHour.BackgroundColor = Color.FromHex("40baaa");
				selectedHour.TextColor = Color.White;
                SelectedHour = selectedHour;

                ChangeOptionImputation(MainMinutes);
            });
        }

        protected void ChangeOptionImputation(Grid panel)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                Content = null;

                MainView.Children.Clear();
                MainView.Children.Add(panel);

                Content = MainView;
            });
        }
    }
}
