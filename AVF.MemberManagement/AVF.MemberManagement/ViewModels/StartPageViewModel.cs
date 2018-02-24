using System;
using System.Diagnostics;
using System.Windows.Input;
using Prism.Mvvm;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;
using Prism.Commands;
using Prism.Navigation;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly IPasswordService _passwordService;
        private readonly IRepository<TrainingsTeilnahme> _trainingsTeilnahmenRepository;
        private readonly IRepository<Training> _trainingRepository;
        private readonly IRepository<Mitglied> _mitgliederRepository;

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public StartPageViewModel(IAccountService accountService, INavigationService navigationService, IPasswordService passwordService, ILogger logger, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository, IRepository<Training> trainingRepository, IRepository<Mitglied> mitgliederRepository) : base(navigationService, logger)
        {
            Title = "Startseite";

            _accountService = accountService;
            _passwordService = passwordService;
            _trainingsTeilnahmenRepository = trainingsTeilnahmenRepository;
            _trainingRepository = trainingRepository;
            _mitgliederRepository = mitgliederRepository;

            NavigateToDaySelectionPageCommand = new DelegateCommand(NavigateToDaySelectionPage, CanNavigateToDaySelectionPage);
        }

        #region NavigateToDaySelectionPageCommand

        public ICommand NavigateToDaySelectionPageCommand { get; }

        private void NavigateToDaySelectionPage()
        {
            NavigationService.NavigateAsync(nameof(DaySelectionPage));
        }

        private bool CanNavigateToDaySelectionPage()
        {
            return true;
        }

        #endregion

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                var user = (User)parameters["User"];
                if (user == null) return;

                Globals.User = user;
                Username = user.Username;

                //Start caching
                //await _mitgliederRepository.GetAsync();
                await _trainingRepository.GetAsync();
                await _trainingsTeilnahmenRepository.GetAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
