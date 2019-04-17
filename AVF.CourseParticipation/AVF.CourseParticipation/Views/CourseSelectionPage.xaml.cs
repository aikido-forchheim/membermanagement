using System;
using AVF.CourseParticipation.Models;
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
            ListViewCourses.Focus();
        }

        public void KeyPressed(Page element, KeyEventArgs keyEventArgs)
        {
            
        }
    }
}
