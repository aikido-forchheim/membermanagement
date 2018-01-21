using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Navigation;
using System.Windows.Input;
using Prism.Commands;

namespace AVF.MemberManagement.ViewModels
{
    public class EnterParticipantsPageViewModel : ViewModelBase
    {
        private readonly IRepository<Mitglied> _mitgliederRepository;
        private readonly IRepository<Training> _trainingsRepository;
        private readonly IRepository<TrainingsTeilnahme> _trainingsTeilnahmenRepository;

        public string ParticipantsCountText => $"Bereits eingetragene Teilnehmer ({Participants.Count}):";
        public string PreviousParticipantsCountText => $"Zuletzt anwesend ({PreviousParticipants.Count}):";
        public string FoundMembersCountText => $"Gefundene Mitglieder ({FoundMembers.Count}):";


        #region Mitglieder, aktuelles Datum und Training

        private List<Mitglied> _mitglieder = new List<Mitglied>();

        private string _selectedDate;

        public string SelectedDateString
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private TrainingsModel _training;

        public TrainingsModel Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        #endregion

        #region Participants

        private ObservableCollection<Mitglied> _originalParticipantsList = new ObservableCollection<Mitglied>();

        private ObservableCollection<Mitglied> _participants = new ObservableCollection<Mitglied>();
        public ObservableCollection<Mitglied> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        private Mitglied _selectedParticipant;
        public Mitglied SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                SetProperty(ref _selectedParticipant, value);
                ((DelegateCommand)RemoveParticipantCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region PreviousParicipants

        private ObservableCollection<Mitglied> _previousParticipants = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> PreviousParticipants
        {
            get => _previousParticipants;
            set => SetProperty(ref _previousParticipants, value);
        }

        private Mitglied _selectedPreviousParticipant;

        public Mitglied SelectedPreviousParticipant
        {
            get => _selectedPreviousParticipant;
            set
            {
                SetProperty(ref _selectedPreviousParticipant, value);
                ((DelegateCommand)AddPreviousParticipantCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region FindMembers

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FindMembers(_searchText);
                RaisePropertyChanged(nameof(FoundMembersCountText));
                ((DelegateCommand)ClearSearchTextCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)AddAndClearSearchTextCommand).RaiseCanExecuteChanged();
            }
        }

        private bool _childrenOnly;

        public bool ChildrenOnly
        {
            get => _childrenOnly;
            set
            {
                SetProperty(ref _childrenOnly, value);
                FindMembers(_searchText);
                RaisePropertyChanged(nameof(FoundMembersCountText));
            }
        }

        private ObservableCollection<Mitglied> _foundMembers = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> FoundMembers
        {
            get => _foundMembers;
            set => SetProperty(ref _foundMembers, value);
        }

        private Mitglied _selectedMember;

        public Mitglied SelectedMember
        {
            get => _selectedMember;
            set
            {
                SetProperty(ref _selectedMember, value);
                ((DelegateCommand)AddFoundMemberCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion


        public ICommand RemoveParticipantCommand { get; set; }
        public ICommand AddPreviousParticipantCommand { get; set; }
        public ICommand AddFoundMemberCommand { get; set; }
        public ICommand ClearSearchTextCommand { get; set; }
        public ICommand AddAndClearSearchTextCommand { get; set; }


        public EnterParticipantsPageViewModel(IRepository<Mitglied> mitgliederRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository, INavigationService navigationService) : base(navigationService)
        {
            _mitgliederRepository = mitgliederRepository;
            _trainingsRepository = trainingsRepository;
            _trainingsTeilnahmenRepository = trainingsTeilnahmenRepository;

            AddPreviousParticipantCommand = new DelegateCommand(AddPreviousParticipant, CanAddPreviousParticipant);
            AddFoundMemberCommand = new DelegateCommand(AddFoundMember, CanAddFoundMember);
            RemoveParticipantCommand = new DelegateCommand(RemoveParticipant, CanRemoveParticipant);
            ClearSearchTextCommand = new DelegateCommand(ClearSearchText, CanClearSearchText);
            AddAndClearSearchTextCommand = new DelegateCommand(AddAndClearSearchText, CanAddAndClearSearchText);
        }

        #region INavigatedAware

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            _participants.Clear();

            SelectedDateString = (string)parameters["SelectedDateString"];
            Training = (TrainingsModel)parameters["SelectedTraining"];
            ChildrenOnly = Training.Training.Kindertraining;

            Title = $"{Training.Class.Time} ({Training.Class.Trainer.Vorname})";

            _mitglieder = await _mitgliederRepository.GetAsync();
            _mitglieder.Sort(CompareMemberNames);

            #region GetExistingParticipants()
            foreach (var existingParticipant in GetExistingParticipants())
            {
                _originalParticipantsList.Add(existingParticipant);
            }
            #endregion
            await FindPreviousParticipants();
            FindMembers(_searchText);

            RaiseCounterPropertiesChanged();
        }

        #endregion

        public async Task<bool> GoBackAsync()
        {
            return await NavigationService.GoBackAsync();
        }


        #region RemoveParticipantCommand

        private bool CanRemoveParticipant()
        {
            return Participants != null && Participants.Count > 0 && SelectedParticipant != null && Participants.Contains(SelectedParticipant);
        }

        private async void RemoveParticipant()
        {
            Participants.Remove(SelectedParticipant);

            ((DelegateCommand)RemoveParticipantCommand).RaiseCanExecuteChanged();

            await FindPreviousParticipants();
            FindMembers(_searchText);

            RaiseCounterPropertiesChanged();
        }

        #endregion

        #region AddPreviousParticipantCommand

        private bool CanAddPreviousParticipant()
        {
            return PreviousParticipants != null && PreviousParticipants.Count > 0 && SelectedPreviousParticipant != null && PreviousParticipants.Contains(SelectedPreviousParticipant);
        }

        private void AddPreviousParticipant()
        {
            Participants.Add(SelectedPreviousParticipant);

            FoundMembers.Remove(SelectedPreviousParticipant);
            PreviousParticipants.Remove(SelectedPreviousParticipant);

            ((DelegateCommand)AddPreviousParticipantCommand).RaiseCanExecuteChanged();

            RaiseCounterPropertiesChanged();
        }

        #endregion

        #region AddFoundMemberCommand

        private bool CanAddFoundMember()
        {
            return FoundMembers != null && FoundMembers.Count > 0 && SelectedMember != null && FoundMembers.Contains(SelectedMember);
        }

        private void AddFoundMember()
        {
            Participants.Add(SelectedMember);

            PreviousParticipants.Remove(SelectedMember);
            FoundMembers.Remove(SelectedMember);

            ((DelegateCommand)AddFoundMemberCommand).RaiseCanExecuteChanged();

            RaiseCounterPropertiesChanged();

            if (FoundMembers.Count == 0) ClearSearchText();
        }

        #endregion

        #region ClearSearchTextCommand

        private void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        private bool CanClearSearchText()
        {
            return !string.IsNullOrEmpty(SearchText);
        }

        #endregion

        #region AddAndClearSearchTextCommand

        private void AddAndClearSearchText()
        {
            if (FoundMembers != null && FoundMembers.Count == 1)
            {
                SelectedMember = FoundMembers[0];

                AddFoundMember();

                SearchText = string.Empty;
            }
        }

        private bool CanAddAndClearSearchText()
        {
            return FoundMembers != null && FoundMembers.Count == 1;
        }

        #endregion


        private ObservableCollection<Mitglied> GetExistingParticipants()
        {
            foreach (var trainingParticipation in Training.Participations)
            {
                var member = _mitglieder.Single(m => m.Id == trainingParticipation.MitgliedID);

                Participants.Add(member);
            }

            return Participants;
        }

        private async Task FindPreviousParticipants()
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

                    PreviousParticipants.Add(_mitglieder.Single(m => m.Id == memberMapping.Key));
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void FindMembers(string searchText)
        {
            FoundMembers.Clear();

            searchText = searchText ?? "";

            var searchStrings = searchText.Split(' ');

            var foundMembers = _mitglieder.Where(m =>
            {
                var argVorname = m.Vorname ?? string.Empty;
                var argNachname = m.Nachname ?? string.Empty;

                if (m.Vorname == null && m.Nachname == null) return false;

                var allSearchStringsMatch = true;
                foreach (var searchString in searchStrings)
                {
                    var containsNamePart = DoesNamePartsContain(searchString, argVorname, argNachname);
                    if (!containsNamePart) allSearchStringsMatch = false;
                }

                return allSearchStringsMatch
                &&     !Participants.Contains(m) 
                &&     !HasResigned(m) 
                &&     (
                           !ChildrenOnly && m.Geburtsdatum <= DateTime.Now - TimeSpan.FromDays(365 * 18)
                           ||
                           ChildrenOnly && m.Geburtsdatum > DateTime.Now - TimeSpan.FromDays(365 * 18)
                       )
                ;
            });

            foreach (var foundMember in foundMembers)
            {
                FoundMembers.Add(foundMember);
            }
        }
        

        #region Helper

        private static int CompareMemberNames(Mitglied x, Mitglied y)
        {
            var argVornameX = x.Vorname ?? string.Empty;
            var argNachnameX = x.Nachname ?? string.Empty;

            var argVornameY = y.Vorname ?? string.Empty;
            var argNachnameY = y.Nachname ?? string.Empty;

            var namex = argVornameX + argNachnameX;
            var namey = argVornameY + argNachnameY;

            var compareResult = string.Compare(namex, namey, StringComparison.Ordinal);

            return compareResult;
        }

        private static bool DoesNamePartsContain(string searchText, string argVorname, string argNachname)
        {
            return argVorname.ToLower().Contains(searchText.ToLower()) ||
                                   argNachname.ToLower().Contains(searchText.ToLower());
        }

        private void RaiseCounterPropertiesChanged()
        {
            RaisePropertyChanged(nameof(ParticipantsCountText));
            RaisePropertyChanged(nameof(PreviousParticipantsCountText));
            RaisePropertyChanged(nameof(FoundMembersCountText));
        }

        private static bool HasResigned(Mitglied mitglied)
        {
            if (mitglied.Austritt == null)
                return false;

            var resignDate = (DateTime)mitglied.Austritt;

            return resignDate < DateTime.Now;
        }

        public bool IsDirty()
        {
            //clear insertList
            //clear deletedList

            if (_originalParticipantsList.Count != _participants.Count)
            {
                return true;
            }

            foreach (var participant in _participants)
            {
                if (!_originalParticipantsList.Contains(participant))
                {
                    //add to insertList
                    return true;
                }
            }

            foreach (var participant in _originalParticipantsList)
            {
                if (!_participants.Contains(participant))
                {
                    //add to deletedList
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}