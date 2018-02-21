using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;
using Prism.Navigation;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class DaySelectionPageViewModel : ViewModelBase
    {
        private readonly IRepository<Wochentag> _wochentageRepository;
        private DateTime _selectedDate = DateTime.Now;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);

                if (_selectedDate.Day == DateTime.Now.Day && _selectedDate.Month == DateTime.Now.Month &&
                    _selectedDate.Year == DateTime.Now.Year)
                {
                    SetButtonText(true);
                }
                else
                {
                    //ButtonText = $"{_selectedDate.Day}.{_selectedDate.Month}.{_selectedDate.Year}";

                    SetButtonText(false);
                }
            }
        }

        private async void SetButtonText(bool isToday)
        {
            var weekDayName = (await Globals.GetWochentagFromDayOfWeek(_wochentageRepository, SelectedDate.DayOfWeek)).Bezeichnung;

            ButtonText = isToday ? $"Heute ({weekDayName})" : $"{weekDayName}";
        }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }


        private string _buttonText = "Heute";

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        public DaySelectionPageViewModel(INavigationService navigationService, IRepository<Wochentag> wochentageRepository, ILogger logger) : base(navigationService, logger)
        {
            _wochentageRepository = wochentageRepository;

            Title = "Trainingsteilnahme";

            MinDate = DateTime.Now - new TimeSpan(64, 0, 0, 0);
            MaxDate = DateTime.Now;

            NavigateToKursSelectionPageCommand = new DelegateCommand(NavigateToKursSelectionPage, CanNavigateToKursSelectionPage);
        }

        #region NavigateToKursSelectionPageCommand

        public ICommand NavigateToKursSelectionPageCommand { get; }

        private void NavigateToKursSelectionPage()
        {
            var navigationParameters = new NavigationParameters {{"Date", SelectedDate}};
            NavigationService.NavigateAsync(nameof(KursSelectionPage), navigationParameters);
        }

        private bool CanNavigateToKursSelectionPage()
        {
            return true;
        }

        #endregion
    }
}
