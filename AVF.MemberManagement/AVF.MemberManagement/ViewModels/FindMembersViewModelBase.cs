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
using AVF.MemberManagement.Views;
using Prism.Commands;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class FindMembersViewModelBase : ViewModelBase
    {
        protected List<Mitglied> Mitglieder = new List<Mitglied>();

        public string ParticipantsCountText => $"Bereits eingetragen ({Participants.Count}):";
        public string PreviousParticipantsCountText => $"Zuletzt anwesend ({PreviousParticipants?.Count}):";
        public string FoundMembersCountText => $"Gefundene Mitglieder ({FoundMembers.Count}):";


        #region Participants

        protected ObservableCollection<Mitglied> _participants = new ObservableCollection<Mitglied>();
        public ObservableCollection<Mitglied> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        #endregion
        
        #region PreviousParticipants

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

        protected string _searchText = string.Empty;

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

        #region ctor
        public FindMembersViewModelBase(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
            AddFoundMemberCommand = new DelegateCommand(AddFoundMember, CanAddFoundMember);
            ClearSearchTextCommand = new DelegateCommand(ClearSearchText, CanClearSearchText);
            AddAndClearSearchTextCommand = new DelegateCommand(AddAndClearSearchText, CanAddAndClearSearchText);
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

                if (m.Vorname == null && m.Nachname == null) return false;

                var allSearchStringsMatch = true;
                foreach (var searchString in searchStrings)
                {
                    var containsNamePart = DoesNamePartsContain(searchString, argVorname, argNachname);
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

        #region AddFoundMemberCommand

        protected bool CanAddFoundMember()
        {
            return FoundMembers != null && FoundMembers.Count > 0 && SelectedMember != null && FoundMembers.Contains(SelectedMember);
        }

        protected void AddFoundMember()
        {
            Participants.Add(SelectedMember);

            PreviousParticipants?.Remove(SelectedMember);
            FoundMembers.Remove(SelectedMember);

            ((DelegateCommand)AddFoundMemberCommand).RaiseCanExecuteChanged();

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

        #region Helpers

        protected void RaiseCounterPropertiesChanged()
        {
            RaisePropertyChanged(nameof(ParticipantsCountText));
            RaisePropertyChanged(nameof(PreviousParticipantsCountText));
            RaisePropertyChanged(nameof(FoundMembersCountText));
        }

        private static bool DoesNamePartsContain(string searchText, string argVorname, string argNachname)
        {
            return argVorname.ToLower().Contains(searchText.ToLower()) ||
                                   argNachname.ToLower().Contains(searchText.ToLower());
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
