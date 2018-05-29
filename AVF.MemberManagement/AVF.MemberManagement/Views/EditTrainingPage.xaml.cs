using System;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class EditTrainingPage : ContentPage
    {
        public EditTrainingPage()
        {
            InitializeComponent();
        }

        private void EditTrainingPage_OnAppearing(object sender, EventArgs e)
        {
            ButtonEnterParticpants.Focus();
        }
    }
}
