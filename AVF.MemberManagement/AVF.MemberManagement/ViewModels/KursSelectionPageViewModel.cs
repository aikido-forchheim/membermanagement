﻿using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class KursSelectionPageViewModel : ViewModelBase
    {
        public DateTime SelectedDate { get; private set; } = DateTime.Today;

        private readonly IRepository<Wochentag> _wochentageRepository;
        private readonly IRepository<Kurs> _kurseRepository;
        private readonly IKursModelService _classModelService;
        private readonly IRepository<Training> _trainings;
        private readonly IRepository<TrainingsTeilnahme> _trainingsTeilnahmen;

        private string _selectedDateString;

        public string SelectedDateString
        {
            get => _selectedDateString;
            private set => SetProperty(ref _selectedDateString, value);
        }

        private Wochentag _wochentag;

        public Wochentag Wochentag
        {
            get => _wochentag;
            set => SetProperty(ref _wochentag, value);
        }

        private ObservableCollection<TrainingsModel> _trainingsCollection = new ObservableCollection<TrainingsModel>();

        public ObservableCollection<TrainingsModel> Trainings
        {
            get => _trainingsCollection;
            set => SetProperty(ref _trainingsCollection, value);
        }

        private TrainingsModel _selectedTraining;

        public TrainingsModel SelectedTraining
        {
            get => _selectedTraining;
            set
            {
                SetProperty(ref _selectedTraining, value);
                ((DelegateCommand)EnterParticipantsCommand).RaiseCanExecuteChanged();
            }
        }

        public KursSelectionPageViewModel(IRepository<Wochentag> wochentageRepository, IRepository<Kurs> kurseRepository, IKursModelService classModelService, IRepository<Training> trainings, IRepository<TrainingsTeilnahme> trainingsTeilnahmen, INavigationService navigationService) : base(navigationService)
        {
            _wochentageRepository = wochentageRepository;
            _kurseRepository = kurseRepository;
            _classModelService = classModelService;
            _trainings = trainings;
            _trainingsTeilnahmen = trainingsTeilnahmen;

            //View all Kurse for this Wochentag and select the nearest one to the actual time (if today?)
            //otherwise do not pre select any

            //Enhancement: show if this Kurs already is a Training and has entered Trainingsteilnahme

            EnterParticipantsCommand = new DelegateCommand(EnterParticipants, CanEnterParticipants);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (!parameters.ContainsKey("Date")) return;

                _trainingsCollection.Clear();

                SelectedDate = (DateTime)parameters["Date"];

                var weekday = (int)SelectedDate.DayOfWeek;
                if (weekday == 0) weekday = 7;

                var wochentage = await _wochentageRepository.GetAsync();

                Wochentag = wochentage.Single(wd => wd.Id == weekday);

                SelectedDateString =
                    $"{SelectedDate.Day.ToString().PadLeft(2, '0')}.{SelectedDate.Month.ToString().PadLeft(2, '0')}.{SelectedDate.Year} ({Wochentag.Bezeichnung})";

                Title = SelectedDateString;

                var alleKurse = await _kurseRepository.GetAsync();
                var trainings = await _trainings.GetAsync();
                var trainingsTeilnahmen = await _trainingsTeilnahmen.GetAsync();

                List<TrainingsModel> trainigModelsWithoutSort = new List<TrainingsModel>();
                foreach (var kurs in alleKurse.Where(k => k.WochentagID == Wochentag.Id))
                {
                    var classModel = await _classModelService.GetAsync(kurs);

                    var existingTrainings =
                        trainings.Where(t => t.KursID == classModel.Id && t.Termin == SelectedDate);

                    var training = existingTrainings.SingleOrDefault();

                    // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                    if (training == null)
                    {
                        training = new Training
                        {
                            Kindertraining = kurs.Kindertraining,
                            Trainer = kurs.Trainer,
                            Kotrainer1 = kurs.Kotrainer1 ?? -1,
                            Kotrainer2 = kurs.Kotrainer2 ?? -1,
                            Zeit = kurs.Zeit,
                            Termin = SelectedDate,
                            KursID = kurs.Id,
                            DatensatzAngelegtAm = DateTime.Now,
                            DatensatzGeaendertAm = DateTime.Now,
                            DatensatzAngelegtVon = 1, //TODO: User logged in MemberId
                            DatensatzGeaendertVon = 1,
                            WochentagID = kurs.WochentagID,
                            DauerMinuten = kurs.DauerMinuten, 
                            Bemerkung = string.Empty
                        };
                    }

                    var trainingsModel = new TrainingsModel
                    {
                        Class = classModel,
                        Training = training,
                        Participations = GetParticipantListForTraining(trainingsTeilnahmen, training)
                    };

                    trainigModelsWithoutSort.Add(trainingsModel);
                }

                foreach (var trainingsModel in trainigModelsWithoutSort.OrderBy(t => t.Training.Zeit))
                {
                    Trainings.Add(trainingsModel);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        #region EnterParticipantsCommand

        public ICommand EnterParticipantsCommand { get; }

        private void EnterParticipants()
        {
            NavigationService.NavigateAsync(nameof(EditTrainingPage), new NavigationParameters { { "SelectedTraining", SelectedTraining }, { "SelectedDateString", SelectedDateString } });
        }

        private bool CanEnterParticipants()
        {
            return SelectedTraining != null;
        }

        #endregion

        private static List<TrainingsTeilnahme> GetParticipantListForTraining(List<TrainingsTeilnahme> appearances, Training training)
        {
            List<TrainingsTeilnahme> participations;

            if (training != null && training.Id != new Training().Id) //new Training().Id == 0
            {
                participations = appearances.Where(participation => participation.TrainingID == training.Id).ToList();
            }
            else
            {
                participations = new List<TrainingsTeilnahme>();
            }

            return participations;
        }
    }
}
