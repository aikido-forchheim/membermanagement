using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;

namespace AVF.MemberManagement.ViewModels
{
    public class AdvancedSettingsPageViewModel : BindableBase
    {
        private readonly IJsonFileFactory _jsonFileFactory;
        private readonly IRepositoryBootstrapper _repositoryBootstrapper;

        public ICommand RefreshCacheCommand { get; }



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

        public AdvancedSettingsPageViewModel(IJsonFileFactory jsonFileFactory, IRepositoryBootstrapper repositoryBootstrapper)
        {
            _jsonFileFactory = jsonFileFactory;
            _repositoryBootstrapper = repositoryBootstrapper;

            RefreshCacheCommand = new DelegateCommand(OnRefreshCache, CanRefreshCache);
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
