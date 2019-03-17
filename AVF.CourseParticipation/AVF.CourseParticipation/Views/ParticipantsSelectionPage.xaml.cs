using AVF.CourseParticipation.ViewModels;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class ParticipantsSelectionPage
    {
        public ParticipantsSelectionPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            ((ParticipantsSelectionPageViewModel)BindingContext).Cancel();
            return true; // Disable back button
        }
    }
}
