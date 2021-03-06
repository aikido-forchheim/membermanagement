﻿using AVF.CourseParticipation.ViewModels;

namespace AVF.CourseParticipation.Views
{
    public class ParticipantsSelectionPage : MemberSelectionPage
    {
        public ParticipantsSelectionPage()
        {
            Title = "Trainingsteilnehmer auswählen";
        }

        protected override bool OnBackButtonPressed()
        {
            if (!(BindingContext is ParticipantsSelectionPageViewModel model)) return false;

            model.Cancel();

            return true; // Disable back button
        }
    }
}
