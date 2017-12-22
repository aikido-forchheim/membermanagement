using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.Views;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class DaySelectionPageViewModel : ViewModelBase
    {
        public DaySelectionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Trainingsteilnahme";

            NavigateToKursSelectionPageCommand = new DelegateCommand(NavigateToKursSelectionPage, CanNavigateToKursSelectionPage);
        }

        #region NavigateToKursSelectionPageCommand

        public ICommand NavigateToKursSelectionPageCommand { get; }

        private void NavigateToKursSelectionPage()
        {
            var navigationParameters = new NavigationParameters {{"Date", new DateTime(2016, 10, 11)}};
            NavigationService.NavigateAsync(nameof(KursSelectionPage), navigationParameters);
        }

        private bool CanNavigateToKursSelectionPage()
        {
            return true;
        }

        #endregion
    }
}
