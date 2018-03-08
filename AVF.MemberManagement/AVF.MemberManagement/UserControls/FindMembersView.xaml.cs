using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AVF.MemberManagement.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindMembersView : ContentView
    {
        public FindMembersView()
        {
            InitializeComponent();
        }

        private void Parent_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (BindingContext as EnterParticipantsPageViewModel)?.AddFoundMemberCommand?.Execute(null);
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            (BindingContext as EnterParticipantsPageViewModel)?.AddAndClearSearchTextCommand?.Execute(null);
            EntrySearchText.Focus();
        }
    }
}