using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Navigation;
using System.Windows.Input;
using Prism.Commands;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class FindMembersViewModelBase : ViewModelBase
    {
        protected List<Mitglied> Mitglieder = new List<Mitglied>();

        public int MaxParticipantsCount = int.MaxValue;


        public string ParticipantsCountText => $"Bereits eingetragen ({Participants.Count}):";
        public string PreviousParticipantsCountText => $"Zuletzt anwesend ({PreviousParticipants?.Count}):";
        public string FoundMembersCountText => $"Gefundene Mitglieder ({FoundMembers.Count}):";


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
                ((DelegateCommand)RemoveParticipantCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region PreviousParticipants

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
        public virtual ObservableCollection<Mitglied> PreviousParticipants { get; set; }

        #endregion
        
        #region FindMembers

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

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FindMembers(_searchText);
                RaisePropertyChanged(nameof(FoundMembersCountText));
                ((DelegateCommand)ClearSearchTextCommand)?.RaiseCanExecuteChanged();
                ((DelegateCommand)AddAndClearSearchTextCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Mitglied _selectedMember;

        public Mitglied SelectedMember
        {
            get => _selectedMember;
            set
            {
                SetProperty(ref _selectedMember, value);
                ((DelegateCommand)AddFoundMemberCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion


        public ICommand AddFoundMemberCommand { get; set; }
        public ICommand ClearSearchTextCommand { get; set; }
        public ICommand AddAndClearSearchTextCommand { get; set; }
        public ICommand RemoveParticipantCommand { get; set; }
        public ICommand AddPreviousParticipantCommand { get; set; }


        #region ctor
        public FindMembersViewModelBase(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
            AddFoundMemberCommand = new DelegateCommand(AddFoundMember, CanAddFoundMember);
            ClearSearchTextCommand = new DelegateCommand(ClearSearchText, CanClearSearchText);
            AddAndClearSearchTextCommand = new DelegateCommand(AddAndClearSearchText, CanAddAndClearSearchText);
            AddPreviousParticipantCommand = new DelegateCommand(AddPreviousParticipant, CanAddPreviousParticipant);
            RemoveParticipantCommand = new DelegateCommand(RemoveParticipant, CanRemoveParticipant);
        }
        #endregion

        protected void FindMembers(string searchText)
        {
            FoundMembers.Clear();

            searchText = searchText ?? "";

            var searchStrings = searchText.Split(' ');

            var foundMembers = Mitglieder.Where(m =>
            {
                var argVorname = m.Vorname ?? string.Empty;
                var argNachname = m.Nachname ?? string.Empty;
				var argRufname = m.Rufname ?? string.Empty;

                if (m.Vorname == null && m.Nachname == null) return false;

                var allSearchStringsMatch = true;
                foreach (var searchString in searchStrings)
                {
                    var containsNamePart = DoesNamePartsContain(searchString, argVorname, argNachname, argRufname);
                    if (!containsNamePart) allSearchStringsMatch = false;
                }

                return allSearchStringsMatch
                && !Participants.Contains(m)
                && !HasResigned(m)
                && (
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

        public virtual Task FindPreviousParticipants()
        {
            return Task.Run(() => { });
        }

        #region AddFoundMemberCommand

        protected bool CanAddFoundMember()
        {
            return FoundMembers != null && FoundMembers.Count > 0 && SelectedMember != null && FoundMembers.Contains(SelectedMember) && Participants.Count < MaxParticipantsCount;
        }

        protected void AddFoundMember()
        {
            if (!CanAddFoundMember()) return;

            Participants.Add(SelectedMember);

            PreviousParticipants?.Remove(SelectedMember);
            FoundMembers.Remove(SelectedMember);

            ((DelegateCommand) AddFoundMemberCommand).RaiseCanExecuteChanged();

            RaiseCounterPropertiesChanged();

            if (FoundMembers.Count == 0) ClearSearchText();
        }

        #endregion

        #region ClearSearchTextCommand

        protected void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        protected bool CanClearSearchText()
        {
            return !string.IsNullOrEmpty(SearchText);
        }

        #endregion

        #region AddAndClearSearchTextCommand

        protected void AddAndClearSearchText()
        {
            if (FoundMembers != null && FoundMembers.Count == 1)
            {
                SelectedMember = FoundMembers[0];

                AddFoundMember();

                SearchText = string.Empty;
            }
        }

        protected bool CanAddAndClearSearchText()
        {
            return FoundMembers != null && FoundMembers.Count == 1;
        }

        #endregion

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
            return PreviousParticipants != null && PreviousParticipants.Count > 0 && SelectedPreviousParticipant != null && PreviousParticipants.Contains(SelectedPreviousParticipant) && Participants.Count < MaxParticipantsCount;
        }

        private void AddPreviousParticipant()
        {
            if (!CanAddPreviousParticipant()) return;

            Participants.Add(SelectedPreviousParticipant);

            FoundMembers.Remove(SelectedPreviousParticipant);
            PreviousParticipants.Remove(SelectedPreviousParticipant);

            ((DelegateCommand) AddPreviousParticipantCommand).RaiseCanExecuteChanged();

            RaiseCounterPropertiesChanged();
        }

        #endregion


        #region Helpers

        protected void RaiseCounterPropertiesChanged()
        {
            RaisePropertyChanged(nameof(ParticipantsCountText));
            RaisePropertyChanged(nameof(PreviousParticipantsCountText));
            RaisePropertyChanged(nameof(FoundMembersCountText));
        }

        private static bool DoesNamePartsContain(string searchText, string argVorname, string argNachname, string argRufname)
        {
            return argVorname.ToLower().Contains(searchText.ToLower()) ||
                                   argNachname.ToLower().Contains(searchText.ToLower()) ||
								   argRufname.ToLower().Contains(searchText.ToLower());
        }

        private static bool HasResigned(Mitglied mitglied)
        {
            if (mitglied.Austritt == null)
                return false;

            var resignDate = (DateTime)mitglied.Austritt;

            return resignDate < DateTime.Now;
        }

        protected static int CompareMemberNames(Mitglied x, Mitglied y)
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

        #endregion
    }
}
