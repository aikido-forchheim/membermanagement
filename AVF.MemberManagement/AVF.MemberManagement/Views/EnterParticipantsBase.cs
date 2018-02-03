using AVF.MemberManagement.ViewModels;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public class EnterParticipantsBase : TabbedPage
    {
        public EnterParticipantsBase()
        {
            

        }

        //protected override void OnAppearing()
        //{
        //    if (BindingContext == null) return;

        //    var bindingContext = (EnterParticipantsPageViewModel)BindingContext;
        //    bindingContext.OnNavigatedFromEvent += (sender, e) => OnClosePageRequested();
        //}

        //// when to register and "De"-Register? ctor seems not to wor

        //protected override bool OnBackButtonPressed()
        //{
        //    OnClosePageRequested();
        //    return true;
        //}

        //private async void OnClosePageRequested()
        //{
        //    var bindingContext = (EnterParticipantsPageViewModel)BindingContext;

        //    if (!bindingContext.IsDirty())
        //    {
        //        await bindingContext.GoBackAsync();
        //        return;
        //    }

        //    var shouldSaveTraining = await DisplayActionSheet("Möchten Sie das aktuelle Training speichern?", null, null, "Ja",
        //        "Nein", "Abbrechen");

        //    switch (shouldSaveTraining)
        //    {
        //        case "Ja":
        //            await bindingContext.NavigateToSaveParticipantsPage();
        //            break;
        //        case "Nein":
        //            await bindingContext.GoBackAsync();
        //            break;
        //    }
        //}
    }
}
