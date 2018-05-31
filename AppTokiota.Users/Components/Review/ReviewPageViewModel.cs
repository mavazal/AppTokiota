﻿using AppTokiota.Users.Components.Core;
using AppTokiota.Users.Components.Core.Module;
using AppTokiota.Users.Components.ManageImputedDay;
using AppTokiota.Users.Controls;
using AppTokiota.Users.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTokiota.Users.Components.Review
{
    public class ReviewPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //Todo Sacar a settings
        DateTimeFormatInfo dtinfo = new CultureInfo(AppSettings.CultureInfoApp).DateTimeFormat;

        #region Services
        protected readonly IReviewModule _reviewModule;
        #endregion

        #region Datapicker
        private ObservableCollection<PickerItem> _yearPicker;
        public ObservableCollection<PickerItem> YearPicker
        {
            get { return _yearPicker; }
            set { SetProperty(ref _yearPicker, value); }
        }

        private ObservableCollection<PickerItem> _monthPicker;
        public ObservableCollection<PickerItem> MonthPicker
        {
            get { return _monthPicker; }
            set { SetProperty(ref _monthPicker, value); }
        }


        private int _myIndexYearPicker;
        public int MyIndexYearPicker
        {
            get { return _myIndexYearPicker; }
            set { SetProperty(ref _myIndexYearPicker, value); }
        }

        private int _myIndexMonthPicker;
        public int MyIndexMonthPicker
        {
            get { return _myIndexMonthPicker; }
            set { SetProperty(ref _myIndexMonthPicker, value); }
        }

        #endregion datapicker

        #region DataReview
        private ObservableCollection<ReviewTimeLine> _lstReview;
        public ObservableCollection<ReviewTimeLine> LstReview
        {
            get { return _lstReview; }
            set { SetProperty(ref _lstReview, value); }
        }


        private Models.Review _currentReview;

        #endregion DataReview

        /// <summary>
        /// Gets or sets the Total DeviationTotal 
        /// </summary>
        /// <value>The Total DeviationTotal</value>
        private double _deviationTotal;
        public double DeviationTotal
        {
            get { return _deviationTotal; }
            set { SetProperty(ref _deviationTotal, value); }
        }

        /// <summary>
        /// Gets or sets the Total Imputed 
        /// </summary>
        /// <value>The Total Imputed</value>
        private double _imputedTotal;
        public double ImputedTotal
        {
            get { return _imputedTotal; }
            set { SetProperty(ref _imputedTotal, value); }
        }

        public bool _btnSendReviewIsVisible;
        public bool BtnSendReviewIsVisible
        {
            get { return _btnSendReviewIsVisible; }
            set { SetProperty(ref _btnSendReviewIsVisible, value); }
        }

        #region Construct
        public ReviewPageViewModel(IViewModelBaseModule baseModule, IReviewModule reviewModule) : base(baseModule)
        {
            _reviewModule = reviewModule;
            _yearPicker = new ObservableCollection<PickerItem>();
            _monthPicker = new ObservableCollection<PickerItem>();

            Title = "Review";
            ModeLoadingPopUp = true;
            LstReview = new ObservableCollection<ReviewTimeLine>();
            LoadDataAsync();
        }
        #endregion constructor

        #region LoadPickersListViewData

        protected void LoadDataAsync()
        {
            IsBusy = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (this.IsInternetAndCloseModal())
                    {
                        await LoadDataPickerAsync();
                        LoadDataReviewByDate(YearPicker.ElementAt(MyIndexYearPicker).Value, MonthPicker.ElementAt(MyIndexMonthPicker).Value);
                    }
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    BaseModule.DialogErrorCustomService.DialogErrorCommonTryAgain();
                    Debug.WriteLine($"[GetTimesheet] Error: {ex}");
                }
            });

        }


        private async Task LoadDataPickerAsync()
        {
            await Task.Run(() =>
            {
				var yearPickerTemp = new ObservableCollection<PickerItem>();
                for (int iyear = DateTime.Now.Year - 1; iyear <= (DateTime.Now.Year + 1); iyear++)
                {
					yearPickerTemp.Add(new PickerItem { Value = iyear, DisplayName = iyear.ToString() });
                }
				YearPicker = yearPickerTemp;

				var monthPickerTemp = new ObservableCollection<PickerItem>();
                for (int imes = DateTime.MinValue.Month; imes < DateTime.MaxValue.Month + 1; imes++)
                {
					monthPickerTemp.Add(new PickerItem { Value = imes, DisplayName = dtinfo.GetMonthName(imes) });
                }
				MonthPicker = monthPickerTemp;
				
                LoadDefaultValues();
            });
        }

        private void LoadDefaultValues()
        {
            DateTime MyDate = DateTime.Now;
            var InitYearPickerItem = new PickerItem
            {
                Value = MyDate.Year,
                DisplayName = MyDate.Year.ToString(),
            };

            var InitMonthPickerItem = new PickerItem
            {
                Value = MyDate.Month,
                DisplayName = dtinfo.GetMonthName(MyDate.Month),
            };

            MyIndexYearPicker = YearPicker.IndexOf(YearPicker.Where(x => x.Value == InitYearPickerItem.Value).FirstOrDefault());
            MyIndexMonthPicker = MonthPicker.IndexOf(MonthPicker.Where(x => x.Value == InitMonthPickerItem.Value).FirstOrDefault());
        }

        protected void LoadDataReviewByDate(int year, int month)
        {
            IsBusy = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (this.IsInternetAndCloseModal())
                    {
                        _currentReview = await _reviewModule.ReviewService.GetReview(year, month);
                        LoadDataReviewAsync(_currentReview);
                    }
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    BaseModule.DialogErrorCustomService.DialogErrorCommonTryAgain();
                    Debug.WriteLine($"[Review load data] Error: {ex}");
                }
            });
        }

        protected async void LoadDataReviewAsync(Models.Review review)
        {
            try
            {
                BtnSendReviewIsVisible = !(review.IsValidated || review.IsClosed);
                var lstReviewDates = await _reviewModule.TimeLineService.GetListTimesheetForDay(review);
                LoadTotalTime(lstReviewDates);
                var listTemp = new ObservableCollection<ReviewTimeLine>();
                lstReviewDates.ForEach(x => listTemp.Add(map(x)));
                listTemp.Last().IsLast = true;
                LstReview = listTemp;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                BaseModule.DialogErrorCustomService.DialogErrorCommonTryAgain();
                Debug.WriteLine($"[Review load data] Error: {ex}");
            }
        }

        private void LoadTotalTime(IList<TimesheetForDay> lstReviewDates)
        {
            foreach (var tsd in lstReviewDates)
            {
                ImputedTotal = ImputedTotal + tsd.Activities.Sum(x => x.Imputed);
                DeviationTotal = DeviationTotal + tsd.Activities.Sum(x => x.Deviation);
            }
        }

        private ReviewTimeLine map(TimesheetForDay x)
        {
            var currentTimeSheetDay = new ReviewTimeLine();
            currentTimeSheetDay.Activity = x.Activities.FirstOrDefault();
            currentTimeSheetDay.Day = x.Day;
            currentTimeSheetDay.IsLast = x.IsLast;
            return currentTimeSheetDay;
        }

        #endregion LoadPickersListViewData

        #region EventOnInfoActivityItemCommand
        public DelegateCommand<object> OnInfoActivityItemCommand => new DelegateCommand<object>((obj) => { OnInfoActivityItem((ReviewTimeLine)obj); });
        protected void OnInfoActivityItem(ReviewTimeLine from)
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add(ActivityDay.Tag, from.Activity);
            BaseModule.NavigationService.NavigateAsync(PageRoutes.GetKey<InfoActivityPopUpPage>(), navigationParameters, true, true);
        }
        #endregion

        #region sendValidateReview

        public DelegateCommand SendReviewValidateCommand => new DelegateCommand(SendReviewToValidate);
        protected void SendReviewToValidate()
        {
            IsBusy = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (this.IsInternetAndCloseModal())
                    {
                        var response = await _reviewModule.ReviewService.PatchReview(YearPicker.ElementAt(MyIndexYearPicker).Value, MonthPicker.ElementAt(MyIndexMonthPicker).Value);
                        if (response)
                        {
                            LoadDataReviewByDate(YearPicker.ElementAt(MyIndexYearPicker).Value, MonthPicker.ElementAt(MyIndexMonthPicker).Value);
                        }
                        else
                        {
                            BaseModule.DialogService.ShowToast("The sending review is not avaible in this moment. Please try again later.");
                        }
                        IsBusy = false;
                    }
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    BaseModule.DialogErrorCustomService.DialogErrorCommonTryAgain();
                    Debug.WriteLine($"[Review load data] Error: {ex}");
                }

            });
        }
        #endregion
    }
}
