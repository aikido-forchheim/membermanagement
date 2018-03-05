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
        protected List<Mitglied> _mitglieder = new List<Mitglied>();

        protected string _searchText = string.Empty;

        public string FoundMembersCountText => $"Gefundene Mitglieder ({FoundMembers.Count}):";

        private ObservableCollection<Mitglied> _foundMembers = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> FoundMembers
        {
            get => _foundMembers;
            set => SetProperty(ref _foundMembers, value);
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

        protected ObservableCollection<Mitglied> _participants = new ObservableCollection<Mitglied>();
        public ObservableCollection<Mitglied> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        public FindMembersViewModelBase(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
        }

        protected void FindMembers(string searchText)
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
    }
}
