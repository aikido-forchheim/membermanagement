using Prism.Commands;
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

namespace AVF.MemberManagement.ViewModels
{
    public class EnterParticipantsPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IRepository<Mitglied> _mitgliederRepository;

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
            set => SetProperty(ref _selectedParticipant, value);
        }

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
            set => SetProperty(ref _selectedMember, value);
        }

        public EnterParticipantsPageViewModel(IRepository<Mitglied> mitgliederRepository)
        {
            _mitgliederRepository = mitgliederRepository;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        private void FindMembers(string searchText)
        {
            FoundMembers.Clear();

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

                return allSearchStringsMatch && !Participants.Contains(m);
            });

            foreach (var foundMember in foundMembers)
            {
                FoundMembers.Add(foundMember);
            }
        }

        private int Tester(Mitglied x, Mitglied y)
        {
            var argVornameX = x.Vorname ?? string.Empty;
            var argNachnameX = x.Nachname ?? string.Empty;

            var argVornameY = y.Vorname ?? string.Empty;
            var argNachnameY = y.Nachname ?? string.Empty;

            string namex = argVornameX + argNachnameX;
            string namey = argVornameY + argNachnameY;

            var compare = string.Compare(namex, namey, StringComparison.Ordinal);
            return compare;
        }

        private static bool DoesNamePartsContain(string searchText, string argVorname, string argNachname)
        {
            return argVorname.ToLower().Contains(searchText.ToLower()) ||
                                   argNachname.ToLower().Contains(searchText.ToLower());
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            _participants.Clear();

            SelectedDateString = (string)parameters["SelectedDateString"];
            Training = (TrainingsModel)parameters["SelectedTraining"];

            _mitglieder = await _mitgliederRepository.GetAsync();

            _mitglieder.Sort(Tester);

            foreach (var trainingParticipation in Training.Participations)
            {
                var member = _mitglieder.Single(m => m.Id == trainingParticipation.MitgliedID);

                Participants.Add(member);
            }
        }
    }
}