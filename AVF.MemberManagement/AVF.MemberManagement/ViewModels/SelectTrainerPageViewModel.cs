using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class SelectTrainerPageViewModel : ViewModelBase
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

        private Mitglied _trainer;

        public Mitglied Trainer
        {
            get => _trainer;
            set => SetProperty(ref _trainer, value);
        }

        private Mitglied _cotrainer1;

        public Mitglied Cotrainer1
        {
            get => _cotrainer1;
            set => SetProperty(ref _cotrainer1, value);
        }

        private Mitglied _cotrainer2;

        public Mitglied Cotrainer2
        {
            get => _cotrainer2;
            set => SetProperty(ref _cotrainer2, value);
        }

        private List<Mitglied> _trainers = new List<Mitglied>();

        public List<Mitglied> Participants //TODO: Rename back to Trainer, and allow override of Binding in ParticipantsView, or rename to something more gerenic like MemberSelection
        {
            get => _trainers;
            set => SetProperty(ref _trainers, value);
        }

        public SelectTrainerPageViewModel(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                SelectedTraining = (TrainingsModel)parameters["SelectedTraining"];
                SelectedDateString = (string)parameters["SelectedDateString"];

                Trainer = (Mitglied)parameters["Trainer"];
                Cotrainer1 = (Mitglied)parameters["Cotrainer1"];
                Cotrainer2 = (Mitglied)parameters["Cotrainer2"];

                Participants.Clear();
                Participants.Add(Trainer);

                if (Cotrainer1 != null)
                {
                    Participants.Add(Cotrainer1);
                }

                if (Cotrainer2 != null)
                {
                    Participants.Add(Cotrainer2);
                }

                Title = $"{SelectedTraining.Class.Time} ({SelectedTraining.Class.Trainer.FirstName})";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
