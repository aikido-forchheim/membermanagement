using System;
using AVF.CourseParticipation.Models;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class CourseSelectionPage
    {
        public CourseSelectionPage()
        {
            InitializeComponent();
        }

        public override void KeyPressed(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == "Enter")
            {
                ButtonEnterParticipants.Focus();
            }

            if (keyEventArgs.Key == "Up" || keyEventArgs.Key == "Down")
            {
                ListViewCourses.Focus();
            }
        }
    }
}
