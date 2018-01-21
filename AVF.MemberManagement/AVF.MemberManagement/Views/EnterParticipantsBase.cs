using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.ViewModels;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public class EnterParticipantsBase : TabbedPage
    {
        protected override bool OnBackButtonPressed()
        {
            OnClosePageRequested();
            return true;
        }

        private async void OnClosePageRequested()
        {
            var bindingContext = ((EnterParticipantsTabletPageViewModel)BindingContext);

            if (!bindingContext.IsDirty())
            {
                await bindingContext.GoBackAsync();
                return;
            }

            var result = await DisplayActionSheet("Möchten Sie das aktuelle Training speichern?", null, null, "Ja",
                "Nein", "Abbrechen");

            if (result == "Ja" || result == "Nein")
            {
                //TODO: Implement Save or Discard on Yes or No

                await bindingContext.GoBackAsync();
            }
        }
    }
}
