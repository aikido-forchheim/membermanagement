using System;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class CourseSelectionPage : ContentPage
    {
        public CourseSelectionPage()
        {
            InitializeComponent();
        }

        private void CourseSelectionPage_OnAppearing(object sender, EventArgs e)
        {
            ButtonEnterParticipants.Focus();
        }
    }
}
