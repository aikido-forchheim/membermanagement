﻿using System;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginPage_OnAppearing(object sender, EventArgs e)
        {
            if (UsernameEntry.Text.Length > 0)
            {
                PasswordEntry.Focus();
            }
            else
            {
                UsernameEntry.Focus();
            }
        }
    }
}
