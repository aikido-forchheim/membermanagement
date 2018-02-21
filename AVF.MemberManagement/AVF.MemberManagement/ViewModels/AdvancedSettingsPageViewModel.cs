using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using Prism.Navigation;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class AdvancedSettingsPageViewModel : ViewModelBase
    {
        private readonly IJsonFileFactory _jsonFileFactory;
        private readonly IRepositoryBootstrapper _repositoryBootstrapper;

        public ICommand RefreshCacheCommand { get; }


        private Idiom _selectedIdiom;

        public Idiom SelectedIdiom
        {
            get => _selectedIdiom;
            set
            {
                SetProperty(ref _selectedIdiom, value);
                if (SelectedIdiom != Idiom.Unsupported) //whyever this is set on back button
                Globals.Idiom = SelectedIdiom;
            }
        }

        public ObservableCollection<Idiom> AvailableIdioms { get; set; } = new ObservableCollection<Idiom>() { Idiom.Unsupported, Idiom.Phone, Idiom.Tablet, Idiom.Desktop, Idiom.TV };


        public bool UseFileProxies
        {
            get => Globals.UseFileProxies;
            set
            {
                Globals.UseFileProxies = value;
                _repositoryBootstrapper.RegisterRepositories(value);
                RaisePropertyChanged();
            }
        }

        private string _cacheMessage;

        public string CacheMessage
        {
            get => _cacheMessage;
            set => SetProperty(ref _cacheMessage, value);
        }

        public AdvancedSettingsPageViewModel(IJsonFileFactory jsonFileFactory, IRepositoryBootstrapper repositoryBootstrapper, INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
            Title = "Erweiterte Einstellungen";

            _jsonFileFactory = jsonFileFactory;
            _repositoryBootstrapper = repositoryBootstrapper;

            RefreshCacheCommand = new DelegateCommand(OnRefreshCache, CanRefreshCache);

            SelectedIdiom = Globals.Idiom;
        }

        private bool CanRefreshCache()
        {
            return true;
        }

        private async void OnRefreshCache()
        {
            try
            {
                var factory = _jsonFileFactory;
                var fileList = await factory.RefreshFileCache();

                //Debug.WriteLine("Cached files count:" + fileList.Count);

                CacheMessage = "Cached files count:" + fileList.Count;
            }
            catch (Exception ex)
            {
                CacheMessage = ex.Message;
            }
        }
    }
}
