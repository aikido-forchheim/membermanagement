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
    public class PasswordPageViewModel : BindableBase, INavigatedAware
    {
        public const int MinPasswordLength = 8;

        #region Properties

        #region Password1

        private string _password1;

        public string Password1
        {
            get => _password1;
            set
            {
                SetProperty(ref _password1, value);
                IsValidPassword1 = (_password1 ?? string.Empty).Length >= MinPasswordLength;

                (SaveAndContinueCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Password1Length

        private int _password1Length;

        public int Password1Length
        {
            get => _password1Length;
            set => SetProperty(ref _password1Length, value);
        }

        #endregion

        #region IsValidPassword1

        private bool _isValidPassword1;

        public bool IsValidPassword1
        {
            get => _isValidPassword1;
            set => SetProperty(ref _isValidPassword1, value);
        }

        #endregion


        #region Password2

        private string _password2;

        public string Password2
        {
            get => _password2;
            set
            {
                SetProperty(ref _password2, value);
                IsValidPassword2 = (_password2 ?? string.Empty).Length >= MinPasswordLength;

                (SaveAndContinueCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Password2Length

        private int _password2Length;

        public int Password2Length
        {
            get => _password2Length;
            set => SetProperty(ref _password2Length, value);
        }

        #endregion

        #region IsValidPassword2

        private bool _isValidPassword2;

        public bool IsValidPassword2
        {
            get => _isValidPassword2;
            set => SetProperty(ref _isValidPassword2, value);
        }

        #endregion

        #endregion

        #region Commands

        #region SaveAndContinue

        public ICommand SaveAndContinueCommand { get; }

        private void OnSaveAndContinue()
        {
            
        }

        private bool CanSaveAndContinue()
        {
            return _isValidPassword1 && _isValidPassword2 && Password1 == Password2;
        }

        #endregion

        #endregion

        public PasswordPageViewModel()
        {
            SaveAndContinueCommand = new DelegateCommand(OnSaveAndContinue, CanSaveAndContinue);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }
    }
}
