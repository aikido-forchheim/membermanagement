using System;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class CalenderPage : ContentPage
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
    }
}
