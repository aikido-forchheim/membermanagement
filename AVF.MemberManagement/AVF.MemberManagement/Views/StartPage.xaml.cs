using System;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void StartPage_OnAppearing(object sender, EventArgs e)
        {
            BtnDaySelection.Focus();
        }
    }
}
