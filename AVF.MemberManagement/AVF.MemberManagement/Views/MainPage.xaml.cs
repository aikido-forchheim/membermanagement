using System;
using AVF.MemberManagement.ViewModels;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            EntryUsername.Focus();
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            ButtonStart.Command.Execute(null);
        }

        private void EntryUsername_OnCompleted(object sender, EventArgs e)
        {
            EntryPassword.Focus();
        }
    }
}
