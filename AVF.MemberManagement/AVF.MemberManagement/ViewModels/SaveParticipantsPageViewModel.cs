using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Navigation;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class SaveParticipantsPageViewModel : ViewModelBase
    {
        private readonly IRepository<TrainingsTeilnahme> _trainingsTeilnahmenRepository;

        #region TrainingsModel

        private TrainingsModel _training;

        public TrainingsModel Training
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        #endregion

        #region Lists

        private ObservableCollection<Mitglied> _inserts = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> Inserts
        {
            get => _inserts;
            set => SetProperty(ref _inserts, value);
        }

        private ObservableCollection<Mitglied> _deletes = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> Deletes
        {
            get => _deletes;
            set => SetProperty(ref _deletes, value);
        }

        #endregion

        private bool _passThroughMode = true;

        public bool PassThroughMode
        {
            get => _passThroughMode;
            set => SetProperty(ref _passThroughMode, value);
        }

        public SaveParticipantsPageViewModel(INavigationService navigationService, ILogger logger, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository) : base(navigationService, logger)
        {
            _trainingsTeilnahmenRepository = trainingsTeilnahmenRepository;
            DontSaveCommand = new DelegateCommand(DontSave, CanDontSave);
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            PassThroughMode = parameters.InternalParameters["__NavigationMode"].ToString() != "Back";

            await OnAppearing(parameters);
        }

        public async System.Threading.Tasks.Task OnAppearing(NavigationParameters parameters)
        {
            Inserts.Clear();
            Deletes.Clear();

            if (!parameters.Any()) return;

            if (PassThroughMode)
            {
                var enterParticipantsPageName = Globals.Idiom == Idiom.Phone ? nameof(EnterParticipantsPage) : nameof(EnterParticipantsTabletPage);
                await NavigationService.NavigateAsync(enterParticipantsPageName, new NavigationParameters { { "SelectedTraining", parameters["SelectedTraining"] }, { "SelectedDateString", parameters["SelectedDateString"] } });
            }
            else
            {
                //is this a switch page or the save page directly then?
                //we do not use this page directly, we create a empty switch page, so no UI is displayed temporarily

                Training = parameters["SelectedTraining"] as TrainingsModel;
                var deletedList = parameters["DeletedList"] as List<Mitglied>;
                var insertList = parameters["InsertList"] as List<Mitglied>;

                if ((deletedList == null || deletedList.Count == 0) && (insertList == null || insertList.Count == 0))
                {
                    await NavigationService.GoBackAsync();
                    return;
                }

                //Check if this isnt fireing even if GoBackAsync
                InsertParticipants(insertList);
                DeleteParticipants(deletedList);
            }
        }

        private void InsertParticipants(List<Mitglied> insertList)
        {
            if (insertList == null) return;

            foreach (var mitglied in insertList)
            {
                Inserts.Add(mitglied);
            }
        }

        private void DeleteParticipants(List<Mitglied> deletedList)
        {
            if (deletedList == null) return;

            foreach (var mitglied in deletedList)
            {
                Deletes.Add(mitglied);
            }
        }

        #region DontSaveCommand

        public ICommand DontSaveCommand { get; }

        private void DontSave()
        {
            NavigationService.GoBackAsync();
        }

        private bool CanDontSave()
        {
            return true;
        }

        #endregion

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            var trainingsTeilnahmen = await _trainingsTeilnahmenRepository.GetAsync();

            foreach (var memberToDeleteFromTraining in Deletes)
            {
                try
                {
                    var trainingsTeilnahmenQuery= trainingsTeilnahmen.Where(t => t.MitgliedID == memberToDeleteFromTraining.Id && t.TrainingID == Training.Training.Id);

                    var teilnahmenQuery = trainingsTeilnahmenQuery.ToList();
                    if (!teilnahmenQuery.Any()) continue;

                    var participationToDelete = teilnahmenQuery.Single();

                    await _trainingsTeilnahmenRepository.DeleteAsync(participationToDelete);

                    //var participationsQuery = Training.Participations.Where(t => t.MitgliedID == memberToDeleteFromTraining.Id && t.TrainingID == Training.Training.Id);

                    //if (participationsQuery.Any())
                    //{
                    //    var participationFromTrainingToDelete = participationsQuery.Single();
                    //    Training.Participations.
                    //}
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                }
            }

            foreach (var memberToInsertToTraining in Inserts)
            {
                try
                {
                    var trainingsTeilnahme = new TrainingsTeilnahme
                    {
                        MitgliedID = memberToInsertToTraining.Id,
                        TrainingID = Training.Training.Id,
                        DatensatzAngelegtAm = DateTime.Now,
                        DatensatzAngelegtVon = Globals.User.Id,
                        DatensatzGeaendertAm = DateTime.Now,
                        DatensatzGeaendertVon = Globals.User.Id
                    };

                    await _trainingsTeilnahmenRepository.CreateAsync(trainingsTeilnahme);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                }
            }

            Training.Participations =
                KursSelectionPageViewModel.GetParticipantListForTraining(trainingsTeilnahmen, Training.Training);

            await NavigationService.GoBackAsync();
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
