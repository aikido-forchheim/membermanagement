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
            var result = await DisplayActionSheet("Möchten Sie das aktuelle Training speichern?", null, null, "Ja",
                "Nein", "Abbrechen");

            if (result == "Ja" || result == "Nein")
            {
                //TODO: Implement Save or Discard on Yes or No
                await ((EnterParticipantsTabletPageViewModel) BindingContext).GoBackAsync();
            }
        }
    }
}