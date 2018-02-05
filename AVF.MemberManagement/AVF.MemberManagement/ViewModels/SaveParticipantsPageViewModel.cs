using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;

namespace AVF.MemberManagement.ViewModels
{
    public class SaveParticipantsPageViewModel : ViewModelBase
    {
        public SaveParticipantsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
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
            
        }

        private void DeleteParticipants(List<Mitglied> deletedList)
        {
            
        }
    }
}
