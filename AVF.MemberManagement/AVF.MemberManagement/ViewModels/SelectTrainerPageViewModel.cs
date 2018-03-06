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
using System.Collections.ObjectModel;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using System.Threading.Tasks;

namespace AVF.MemberManagement.ViewModels
{
    public class SelectTrainerPageViewModel : FindMembersViewModelBase
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

        private Mitglied _selectedParticipant;

        public Mitglied SelectedParticipant
        {
            get => _selectedParticipant;
            set => SetProperty(ref _selectedParticipant, value);
        }

        private readonly IRepository<Training> _trainingsRepository;
        private readonly IRepository<Mitglied> _mitgliederRepository;

        public SelectTrainerPageViewModel(INavigationService navigationService, ILogger logger, IRepository<Training> trainingsRepository, IRepository<Mitglied> mitgliederRepository) : base(navigationService, logger)
        {
            _trainingsRepository = trainingsRepository;
            _mitgliederRepository = mitgliederRepository;
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                _mitglieder = await _mitgliederRepository.GetAsync();
                _mitglieder.Sort(CompareMemberNames);

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

                //ParticipantsCountText = $"Trainer ({Participants.Count}):";

                await GetPreviousTrainers();

                FindMembers(_searchText);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task GetPreviousTrainers()
        {
            //Bei beiden ausgetretene Mitglieder nicht mehr berücksichtigen: HasResigned()-Methode
            //Bei beiden egal ob Trainer, Cotrainer1 oder 2
            //Find alle Mitglieder, die jemals bei diesem Kurs Übungsleitung gemacht haben, sortiere diese nach Häufigkeit
            //Mit Schalter "optional": Finde alle Trainer über alle Kurse, die noch nicht in der ersten Liste sind, und füge diese auch geordnet nach Häufigkeit ein.

            //Wenn Partipants leer springe in der Mobile-View automatisch auf den Tab PreviousParticipants.

            PreviousParticipants.Clear();

            var trainings = await _trainingsRepository.GetAsync();

            var previousTrainersOfThisClass = trainings.Where(t => t.KursID == SelectedTraining.Training.KursID)
                .GroupBy(l => l.Trainer)
                  .Select(g => new
                  {
                      Trainer = g.Key,
                      Count = g.Select(l => l.Trainer).Distinct().Count()
            }).OrderByDescending(t => trainings.Count()).Select(t => t.Trainer);

            foreach (var previousTrainerId in previousTrainersOfThisClass)
            {
                var previousTrainer = await _mitgliederRepository.GetAsync(previousTrainerId);
                PreviousParticipants.Add(previousTrainer);
            }
        }
    }
}
