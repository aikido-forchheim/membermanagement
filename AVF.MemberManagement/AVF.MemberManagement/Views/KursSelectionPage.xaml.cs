using System;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class KursSelectionPage : ContentPage
    {
        public KursSelectionPage()
        {
            InitializeComponent();
        }

        private void KursSelectionPage_OnAppearing(object sender, EventArgs e)
        {
            ButtonEnterParticipants.Focus();
        }

        private void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            ButtonEnterParticipants.Focus();
        }
    }
}
