using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AVF.MemberManagement.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParticipantsView : ContentView
    {
        public ParticipantsView()
        {
            InitializeComponent();
        }

        private void Parent_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                (BindingContext as FindMembersViewModelBase)?.RemoveParticipantCommand.Execute(null);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}