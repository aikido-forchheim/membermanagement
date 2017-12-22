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
    public class EnterParticipantsPageViewModel : BindableBase, INavigatedAware
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
                //((DelegateCommand)AddPreviousParticipantCommand).RaiseCanExecuteChanged();
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

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FindMembers(_searchText);
            }
        }

        private bool _childrenOnly;

        public bool ChildrenOnly
        {
            get => _childrenOnly;
            set { SetProperty(ref _childrenOnly, value); FindMembers(_searchText); }
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


        public EnterParticipantsPageViewModel(IRepository<Mitglied> mitgliederRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository)
        {
            _mitgliederRepository = mitgliederRepository;
            _trainingsRepository = trainingsRepository;
            _trainingsTeilnahmenRepository = trainingsTeilnahmenRepository;

            AddPreviousParticipantCommand = new DelegateCommand(AddPreviousParticipant, CanAddPreviousParticipant);
            AddFoundMemberCommand = new DelegateCommand(AddFoundMember, CanAddFoundMember);
            RemoveParticipantCommand = new DelegateCommand(RemoveParticipant, CanRemoveParticipant);
        }

        #region INavigatedAware

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            _participants.Clear();

            SelectedDateString = (string)parameters["SelectedDateString"];
            Training = (TrainingsModel)parameters["SelectedTraining"];

            ChildrenOnly = Training.Training.Kindertraining;

            _mitglieder = await _mitgliederRepository.GetAsync();

            _mitglieder.Sort(CompareMemberNames);

            GetExistingParticipants();

            await FindPreviousParticipants();
        }

        #endregion


        #region RemoveParticipantCommand

        private bool CanRemoveParticipant()
        {
            return true;
        }

        private void RemoveParticipant()
        {
            System.Diagnostics.Debug.WriteLine("RemoveParticipant clicked");
        }

        #endregion

        #region AddPreviousParticipantCommand

        private bool CanAddPreviousParticipant()
        {
            return PreviousParticipants != null && PreviousParticipants.Count > 0 && SelectedPreviousParticipant != null
                   && PreviousParticipants.Contains(SelectedPreviousParticipant)
                ;
        }

        private void AddPreviousParticipant()
        {
            Participants.Add(SelectedPreviousParticipant);
            PreviousParticipants.Remove(SelectedPreviousParticipant);

            ((DelegateCommand)AddPreviousParticipantCommand).RaiseCanExecuteChanged();
        }

        #endregion

        #region AddFoundMemberCommand

        private bool CanAddFoundMember()
        {
            return FoundMembers != null && FoundMembers.Count > 0 && SelectedMember != null
                                                       && FoundMembers.Contains(SelectedMember)
                                                                       ;
        }

        private void AddFoundMember()
        {
            Participants.Add(SelectedMember);
            FoundMembers.Remove(SelectedMember);

            ((DelegateCommand)AddFoundMemberCommand).RaiseCanExecuteChanged();
        }

        #endregion

        
        private void GetExistingParticipants()
        {
            foreach (var trainingParticipation in Training.Participations)
            {
                var member = _mitglieder.Single(m => m.Id == trainingParticipation.MitgliedID);

                Participants.Add(member);
            }

            RaisePropertyChanged(nameof(ParticipantsCountText));
        }

        private async Task FindPreviousParticipants()
        {
            try
            {
                PreviousParticipants.Clear()
                                    ;
                var trainings = await _trainingsRepository.GetAsync();

                var trainingsOnWeekDay = trainings.Where(training =>
                {
                    var daysToCheck = Training.Training.Termin - TimeSpan.FromDays(30);

                    return training.KursID == Training.Training.KursID && training.Id != Training.Training.Id &&
                               training.Termin < Training.Training.Termin && training.Termin > daysToCheck;
                }).Select(t => t.Id).ToList();

                var trainingsTeilnahmen = await _trainingsTeilnahmenRepository.GetAsync();

                Dictionary<int, int> mitgliederCount = new Dictionary<int, int>();
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
                    //nur wenn noch nicht in Participants Liste

                    PreviousParticipants.Add(_mitglieder.Single(m => m.Id == memberMapping.Key));
                }

                RaisePropertyChanged(nameof(PreviousParticipantsCountText));
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

                return allSearchStringsMatch && !Participants.Contains(m) && 
                                                             (!ChildrenOnly && m.Geburtsdatum <= DateTime.Now - TimeSpan.FromDays(365 * 18)
                ||
                                                              ChildrenOnly && m.Geburtsdatum > DateTime.Now - TimeSpan.FromDays(365 * 18))

                                                             ;// && m.Austritt == null && m.BeitragsklasseID == 4;
            });

            foreach (var foundMember in foundMembers)
            {
                FoundMembers.Add(foundMember);
            }

            RaisePropertyChanged(nameof(FoundMembersCountText));
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

        #endregion
    }
}