using System;
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

        private ObservableCollection<Mitglied> _previousParticipants = new ObservableCollection<Mitglied>();

        public override ObservableCollection<Mitglied> PreviousParticipants
        {
            get => _previousParticipants;
            set => SetProperty(ref _previousParticipants, value);
        }

        private readonly IRepository<Training> _trainingsRepository;
        private readonly IRepository<Mitglied> _mitgliederRepository;

        public SelectTrainerPageViewModel(INavigationService navigationService, ILogger logger, IRepository<Training> trainingsRepository, IRepository<Mitglied> mitgliederRepository) : base(navigationService, logger)
        {
            MaxParticipantsCount = 3;

            _trainingsRepository = trainingsRepository;
            _mitgliederRepository = mitgliederRepository;
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Mitglieder = await _mitgliederRepository.GetAsync();
                Mitglieder.Sort(CompareMemberNames);

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

                await FindPreviousParticipants();

                FindMembers(SearchText);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            if (Participants.Count == 0) return; //do not change standard training/trainer
            
            SelectedTraining.Training.Trainer = Participants[0].Id;

            switch (Participants.Count)
            {
                case 1:
                    SelectedTraining.Training.Kotrainer1 = -1;
                    SelectedTraining.Training.Kotrainer2 = -1;
                    break;
                case 2:
                    SelectedTraining.Training.Kotrainer1 = Participants[1].Id;
                    SelectedTraining.Training.Kotrainer2 = -1;
                    break;
                case 3:
                    SelectedTraining.Training.Kotrainer1 = Participants[1].Id;
                    SelectedTraining.Training.Kotrainer2 = Participants[2].Id;
                    break;
            }

            parameters.Add(NavigationParameter.SelectedTraining, SelectedTraining);
        }

        public override async Task FindPreviousParticipants()
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
                if (!Participants.Contains(previousTrainer))
                {
                    PreviousParticipants.Add(previousTrainer);
                }
            }
        }
    }
}
