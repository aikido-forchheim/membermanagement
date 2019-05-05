using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Services;

namespace AVF.CourseParticipation.ViewModels
{
	public class NewPasswordPageViewModel : ViewModelBase
	{
	    private readonly IPageDialogService _dialogService;
	    private readonly IPasswordService _passwordService;
	    private readonly IRepository<User> _usersRepository;

	    private int _userId;

	    private string _password1;

	    public string Password1
	    {
	        get => _password1;
	        set
	        {
	            SetProperty(ref _password1, value);
                ((DelegateCommand)SaveAndContinueCommand).RaiseCanExecuteChanged();
	        }
	    }

	    private string _password2;

	    public string Password2
	    {
	        get => _password2;
	        set
	        {
	            SetProperty(ref _password2, value);
	            ((DelegateCommand)SaveAndContinueCommand).RaiseCanExecuteChanged();
            }
	    }

	    public ICommand SaveAndContinueCommand { get; }

        public NewPasswordPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IPasswordService passwordService, IRepository<User> usersRepository) : base(navigationService)
        {
            _dialogService = dialogService;
            _passwordService = passwordService;
            _usersRepository = usersRepository;

            SaveAndContinueCommand = new DelegateCommand(SaveAndContinue, CanSaveAndContinue);
        }

	    private bool CanSaveAndContinue()
	    {
            return true;
	    }

	    private async void SaveAndContinue()
	    {
            LoginPageViewModel.ClearLoggedInMemberId();

	        if (Password1 != Password2)
	        {
	            await _dialogService.DisplayAlertAsync("Fehler", "Passwörter sind nicht identisch!", "OK");
	            return;
	        }

	        var password = Password1 ?? string.Empty;

	        if (password.Length < 8)
	        {
	            await _dialogService.DisplayAlertAsync("Fehler", "Passwort muss mindestens 8 Zeichen besitzen!", "OK");
	            return;
	        }

	        var passwordHash = await _passwordService.HashPasswordAsync(password, null);

	        var user = await _usersRepository.GetAsync(_userId);

	        user.Password = passwordHash;

	        await _usersRepository.UpdateAsync(user);

	        LoginPageViewModel.SetLoggedInMemberId(user);
	        await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }

	    public override void OnNavigatingTo(INavigationParameters parameters)
	    {
	        if (!parameters.ContainsKey("UserId"))
	            throw new ArgumentOutOfRangeException(nameof(parameters));

            var userId = parameters["UserId"].ToString();
	        var isUserIdInt = int.TryParse(userId, out _userId);

	        if (!isUserIdInt) throw new ArgumentOutOfRangeException(nameof(parameters));
	    }
    }
}
