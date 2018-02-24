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
    public partial class PreviousParticipantsView : ContentView
    {
        public PreviousParticipantsView()
        {
            InitializeComponent();
        }

        private void Parent_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((EnterParticipantsPageViewModel)BindingContext).AddPreviousParticipantCommand.Execute(null);
        }

        private void Parent_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            //parent.Focus();
        }
    }
}