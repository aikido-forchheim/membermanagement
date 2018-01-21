using AVF.MemberManagement.ViewModels;
using Xamarin.Forms.Xaml;

namespace AVF.MemberManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterParticipantsTabletPage
    {
        public EnterParticipantsTabletPage()
        {
            InitializeComponent();
        }

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
                //TODO: Inherit EnterParticipantsTabletPage and EnterParticipantsPage from base
                //TODO: Implement Save or Discard on Yes or No


                await bindingContext.GoBackAsync();
            }
        }
    }
}