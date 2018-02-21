using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Navigation;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class SaveParticipantsPageViewModel : ViewModelBase
    {
        private ObservableCollection<Mitglied> _inserts = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> Inserts
        {
            get => _inserts;
            set => SetProperty(ref _inserts, value);
        }

        private ObservableCollection<Mitglied> _deletes = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> Deletes
        {
            get => _deletes;
            set => SetProperty(ref _deletes, value);
        }

        public SaveParticipantsPageViewModel(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            Inserts.Clear();
            Deletes.Clear();

            if (parameters == null || !parameters.Any()) return;
           
            if (parameters["__NavigationMode"].ToString() != "Back")
            {
                var enterParticipantsPageName = Globals.Idiom == Idiom.Phone ? nameof(EnterParticipantsPage) : nameof(EnterParticipantsTabletPage);
                await NavigationService.NavigateAsync(enterParticipantsPageName, new NavigationParameters { { "SelectedTraining", parameters["SelectedTraining"] }, { "SelectedDateString", parameters["SelectedDateString"] } });
            }
            else
            {
                //is this a switch page or the save page directly then?
                //we do not use this page directly, we create a empty switch page, so no UI is displayed temporarily

                var deletedList = parameters["DeletedList"] as List<Mitglied>;
                var insertList = parameters["InsertList"] as List<Mitglied>;

                if (deletedList != null && insertList != null && insertList.Count == 0 && deletedList.Count == 0)
                {
                    await NavigationService.GoBackAsync();
                    return;
                }

                //Check if this isnt fireing even if GoBackAsync
                InsertParticipants(insertList);
                DeleteParticipants(deletedList);
            }
        }

        private void InsertParticipants(List<Mitglied> insertList)
        {
            foreach (var mitglied in insertList)
            {
                Inserts.Add(mitglied);
            }
        }

        private void DeleteParticipants(List<Mitglied> deletedList)
        {
            foreach (var mitglied in deletedList)
            {
                Deletes.Add(mitglied);
            }
        }
    }
}
