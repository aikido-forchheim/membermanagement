using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Navigation;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class EnterParticipantsPageViewModel : FindMembersViewModelBase
    {
        private readonly IRepository<Mitglied> _mitgliederRepository;
        private readonly IRepository<Training> _trainingsRepository;
        private readonly IRepository<TrainingsTeilnahme> _trainingsTeilnahmenRepository;

        private readonly ObservableCollection<Mitglied> _originalParticipantsList = new ObservableCollection<Mitglied>();
        private readonly List<Mitglied> _insertList = new List<Mitglied>();
        private readonly List<Mitglied> _deletedList = new List<Mitglied>();

        #region TrainingsModel

        private TrainingsModel _training;

        public TrainingsModel Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        #endregion
        
        #region PreviousParicipants

        private ObservableCollection<Mitglied> _previousParticipants = new ObservableCollection<Mitglied>();

        public override ObservableCollection<Mitglied> PreviousParticipants
        {
            get => _previousParticipants;
            set => SetProperty(ref _previousParticipants, value);
        }

        #endregion

        #region ctor
        public EnterParticipantsPageViewModel(IRepository<Mitglied> mitgliederRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository, INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
            _mitgliederRepository = mitgliederRepository;
            _trainingsRepository = trainingsRepository;
            _trainingsTeilnahmenRepository = trainingsTeilnahmenRepository;
        }
        #endregion

        #region INavigatedAware

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (!parameters.ContainsKey("SelectedTraining")) return;

            Participants.Clear();

            Training = (TrainingsModel)parameters["SelectedTraining"];
            
            ChildrenOnly = Training.Training.Kindertraining;

            Title = Globals.Idiom != Idiom.Phone ? $"{Training.Date}, {Training.Description}" : Training.Description;

            Mitglieder = await _mitgliederRepository.GetAsync();
            Mitglieder.Sort(CompareMemberNames);

            #region GetExistingParticipants()
            foreach (var existingParticipant in GetExistingParticipants())
            {
                _originalParticipantsList.Add(existingParticipant);
            }
            #endregion
            await FindPreviousParticipants();
            FindMembers(SearchText);

            RaiseCounterPropertiesChanged();
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            if (ShouldCancel) return;

            FillLists();

            parameters.Add(NavigationParameter.SelectedTraining, Training);
            parameters.Add("DeletedList", _deletedList);
            parameters.Add("InsertList", _insertList);
        }

        #endregion

        #region Helper

        private ObservableCollection<Mitglied> GetExistingParticipants()
        {
            foreach (var trainingParticipation in Training.Participations)
            {
                var member = Mitglieder.Single(m => m.Id == trainingParticipation.MitgliedID);

                Participants.Add(member);
            }

            return Participants;
        }

        public override async Task FindPreviousParticipants()
        {
            try
            {
                PreviousParticipants.Clear();

                var trainings = await _trainingsRepository.GetAsync();

                var trainingsOnWeekDay = trainings.Where(training =>
                {
                    var daysToCheck = Training.Training.Termin - TimeSpan.FromDays(120);

                    return training.KursID == Training.Training.KursID && training.Id != Training.Training.Id &&
                               training.Termin < Training.Training.Termin && training.Termin > daysToCheck;
                }
                ).Select(t => t.Id).ToList();

                var trainingsTeilnahmen = await _trainingsTeilnahmenRepository.GetAsync();

                var mitgliederCount = new Dictionary<int, int>();
                var trainingsTeilnahmes = trainingsTeilnahmen.Where(teilnahme => trainingsOnWeekDay.Contains(teilnahme.TrainingID)).ToList();
                foreach (var trainingsTeilnahme in trainingsTeilnahmes)
                {
                    if (!mitgliederCount.ContainsKey(trainingsTeilnahme.MitgliedID)) mitgliederCount.Add(trainingsTeilnahme.MitgliedID, 0);

                    mitgliederCount[trainingsTeilnahme.MitgliedID]++;
                }

                //now order dict by counter (second int)
                var myList = mitgliederCount.ToList();

                myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value)); //order descending

                foreach (var memberMapping in myList)
                {
                    if (Participants.Any(p => p.Id == memberMapping.Key))
                        continue;

                    PreviousParticipants.Add(Mitglieder.Single(m => m.Id == memberMapping.Key));
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public bool FillLists()
        {
            _insertList.Clear();
            _deletedList.Clear();

            foreach (var participant in Participants)
            {
                if (!_originalParticipantsList.Contains(participant))
                {
                    _insertList.Add(participant);
                }
            }

            foreach (var participant in _originalParticipantsList)
            {
                if (!Participants.Contains(participant))
                {
                    _deletedList.Add(participant);
                }
            }

            return _insertList.Count != 0 || _deletedList.Count != 0;
        }

        #endregion
    }
}