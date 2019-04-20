using System;
using AVF.CourseParticipation.Models;
using AVF.CourseParticipation.ViewModels;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class CalenderPage
    {
        private int _monthsNavigated = 0;

        public CalenderPage()
        {
            InitializeComponent();
        }

        private void PreviousMonthButton_OnClicked(object sender, EventArgs e)
        {
            Calendar.PreviousMonth();
            _monthsNavigated--;
        }

        private void NextMonthButton_OnClicked(object sender, EventArgs e)
        {
            Calendar.NextMonth();
            _monthsNavigated++;
        }

        private void TodayButton_OnClicked(object sender, EventArgs e)
        {
            SetToday();
        }

        private void SetToday()
        {
            Calendar.SelectedDate = DateTime.Now;

            if (_monthsNavigated < 0)
            {
                for (int i = 0; i < _monthsNavigated * -1; i++)
                {
                    Calendar.NextMonth();
                }

                _monthsNavigated = 0;
            }

            if (_monthsNavigated > 0)
            {
                for (int i = 0; i < _monthsNavigated; i++)
                {
                    Calendar.PreviousMonth();
                }

                _monthsNavigated = 0;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ((CalenderPageViewModel) BindingContext).Logout();
            return true; // Disable back button
        }

        private void CalenderPage_OnAppearing(object sender, EventArgs e)
        {
            ButtonSelectCourse.Focus();
        }

        public override void KeyPressed(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == "Enter")
            {
                ButtonSelectCourse.Command.Execute(null);
            }

            var previousDate = Calendar.SelectedDate ?? DateTime.Now;

            if (keyEventArgs.Key == "Down")
            {
                Calendar.SelectedDate = Calendar.SelectedDate + new TimeSpan(1, 0, 0, 0);
            }

            if (keyEventArgs.Key == "Up")
            {
                Calendar.SelectedDate = Calendar.SelectedDate - new TimeSpan(1, 0, 0, 0);
            }

            if (keyEventArgs.Key == "Right")
            {
                var currentDate = Calendar.SelectedDate ?? DateTime.Now;

                var newYear = currentDate.Year;
                var newMonth = currentDate.Month+1;

                if (newMonth == 13)
                {
                    newMonth = 1;
                    newYear++;
                }

                Calendar.SelectedDate = new DateTime(newYear, newMonth, 1);
            }

            if (keyEventArgs.Key == "Left")
            {
                var currentDate = Calendar.SelectedDate ?? DateTime.Now;

                var newYear = currentDate.Year;
                var newMonth = currentDate.Month + -1;

                if (newMonth == 0)
                {
                    newMonth = 12;
                    newYear--;
                }

                Calendar.SelectedDate = new DateTime(newYear, newMonth, 1);
            }

            if (keyEventArgs.Key == "Home")
            {
                SetToday();
            }

            var selectedDate = Calendar.SelectedDate ?? DateTime.Now;

            var previousYearMonth = previousDate.Year.ToString() + previousDate.Month.ToString().PadLeft(2, '0');
            var selectedYearMonth = selectedDate.Year.ToString() + selectedDate.Month.ToString().PadLeft(2, '0');

            if (string.Compare(selectedYearMonth, previousYearMonth, StringComparison.Ordinal) > 0)
            {
                Calendar.NextMonth();
                _monthsNavigated++;
            }

            if (string.Compare(selectedYearMonth, previousYearMonth, StringComparison.Ordinal) < 0)
            {
                Calendar.PreviousMonth();
                _monthsNavigated--;
            }
        }
    }
}
