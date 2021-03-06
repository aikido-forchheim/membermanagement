﻿using System;
using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class DaySelectionPage : ContentPage
    {
        public DaySelectionPage()
        {
            InitializeComponent();
        }

        private void DaySelectionPage_OnAppearing(object sender, EventArgs e)
        {
            BtnToday.Focus();
        }

        private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            BtnToday.Focus();
        }
    }
}
