using AVF.CourseParticipation.ViewModels;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class SaveStatusPage : ContentPage
    {
        public SaveStatusPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            ((SaveStatusPageViewModel)BindingContext).GoBack();
            return true; // Disable back button
        }
    }
}
