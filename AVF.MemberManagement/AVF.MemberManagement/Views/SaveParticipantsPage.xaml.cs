using System;
using AVF.MemberManagement.ViewModels;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class SaveParticipantsPage : ContentPage
    {
        public SaveParticipantsPage()
        {
            InitializeComponent();
        }

        private void SaveParticipantsPage_OnAppearing(object sender, EventArgs e)
        {
            var x = ((SaveParticipantsPageViewModel)this.BindingContext);
        }
    }
}
