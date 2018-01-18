using Prism.Commands;
using System;
using System.Diagnostics;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.Views;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class EditTrainingPageViewModel : ViewModelBase
    {
        private string _selectedDate;

        public string SelectedDateString
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private TrainingsModel _training;

        public TrainingsModel SelectedTraining
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        private string _annotation;

        public string Annotation
        {
            get => _annotation;
            set => SetProperty(ref _annotation, value);
        }

        public EditTrainingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            EnterParticipantsCommand = new DelegateCommand(EnterParticipants, CanEnterParticipants);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (!parameters.ContainsKey("SelectedTraining")) return;

                SelectedTraining = (TrainingsModel)parameters["SelectedTraining"];
                SelectedDateString = (string)parameters["SelectedDateString"];

                Title = $"{SelectedTraining.Class.Time} ({SelectedTraining.Class.Trainer.Vorname})";

                Annotation = SelectedTraining.Training.Bemerkung; //erst beim weiter zurück an die Bemerkung binden
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #region EnterParticipantsCommand

        public ICommand EnterParticipantsCommand { get; }

        private void EnterParticipants()
        {
            var updateAnnotation = SelectedTraining.Training.Bemerkung != Annotation;

            SelectedTraining.Training.Bemerkung = Annotation;

            if (SelectedTraining.Training.Id == 0)
            {
                //TODO: Training anlegen
                //TODO: Achtung beim zurück von EnterParticipants usw. darf nicht noch mal angelegt werden
            }
            else if (updateAnnotation)
            {
                //TODO: Update Traing on change of Bemerkung
            }

            var enterParticipantsPageName = Globals.Idiom == Idiom.Phone ? nameof(EnterParticipantsPage) : nameof(EnterParticipantsTabletPage);
            NavigationService.NavigateAsync(enterParticipantsPageName, new NavigationParameters { { "SelectedTraining", SelectedTraining }, { "SelectedDateString", SelectedDateString } });
        }

        private bool CanEnterParticipants()
        {
            return true;
        }

        #endregion
    }
}
