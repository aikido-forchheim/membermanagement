using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Services;

namespace AVF.CourseParticipation.ViewModels
{
	public class NewPasswordPageViewModel : ViewModelBase
	{
	    private readonly IPageDialogService _dialogService;
	    private string _password1;

	    public string Password1
	    {
	        get => _password1;
	        set => SetProperty(ref _password1, value);
	    }

	    private string _password2;

	    public string Password2
	    {
	        get => _password2;
	        set => SetProperty(ref _password2, value);
	    }

        public ICommand SaveAndContinueCommand { get; }

        public NewPasswordPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;

            SaveAndContinueCommand = new DelegateCommand(SaveAndContinue, CanSaveAndContinue);
        }

	    private bool CanSaveAndContinue()
	    {
	        if (Password1 != Password2)
	        {
	            _dialogService.DisplayAlertAsync("Fehler", "Passwörter sind nicht identisch!", "OK");
	            return false;
	        }

	        var password = Password1 ?? string.Empty;

	        if (password.Length < 8)
	        {
	            _dialogService.DisplayAlertAsync("Fehler", "Passwort muss mindestens 8 Zeichen besitzen!", "OK");
	            return false;
            }

            return true;
	    }

	    private async void SaveAndContinue()
	    {
	        await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }
	}
}
