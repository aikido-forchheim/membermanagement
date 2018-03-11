using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
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

        public KursSelectionPageViewModel(IRepository<Wochentag> wochentageRepository, IRepository<Kurs> kurseRepository, IKursModelService classModelService, IRepository<Training> trainings, IRepository<TrainingsTeilnahme> trainingsTeilnahmen, INavigationService navigationService, ILogger logger) : base(navigationService, logger)
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
                
                Wochentag = await Globals.GetWochentagFromDayOfWeek(_wochentageRepository, SelectedDate.DayOfWeek);

                var selectedDateString =
                    $"{SelectedDate.Day.ToString().PadLeft(2, '0')}.{SelectedDate.Month.ToString().PadLeft(2, '0')}.{SelectedDate.Year} ({Wochentag.Bezeichnung})";

                Title = selectedDateString;

                var alleKurse = await _kurseRepository.GetAsync();
                var trainings = await _trainings.GetAsync();
                var trainingsTeilnahmen = await _trainingsTeilnahmen.GetAsync();

                var trainigModelsWithoutSort = new List<TrainingsModel>();
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
                            DatensatzAngelegtVon = Globals.User.Id,
                            DatensatzGeaendertVon = Globals.User.Id,
                            WochentagID = kurs.WochentagID,
                            DauerMinuten = kurs.DauerMinuten,
                            Bemerkung = string.Empty
                        };
                    }

                    var trainingsModel = new TrainingsModel
                    {
                        Class = classModel,
                        Training = training,
                        Participations = GetParticipantListForTraining(trainingsTeilnahmen, training),
                        Date = selectedDateString,
                        Description = $"{classModel.Time} ({classModel.Trainer.FirstName})"
                    };

                    trainigModelsWithoutSort.Add(trainingsModel);
                }

                foreach (var trainingsModel in trainigModelsWithoutSort.OrderBy(t => t.Training.Zeit))
                {
                    Trainings.Add(trainingsModel);
                }

                SelectedTraining = Trainings.OrderBy(model =>
                {
                    var trainingEndTime = model.Training.Zeit + new TimeSpan(0, model.Training.DauerMinuten, 0);
                    var trainingEndDateTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, trainingEndTime.Hours, trainingEndTime.Minutes, trainingEndTime.Seconds);
                    var differenceToNow = trainingEndDateTime - DateTime.Now;

                    var secondsToNow = differenceToNow.TotalSeconds;
                    var absSecondsToNow = Math.Abs(secondsToNow);

                    return absSecondsToNow;
                }).First();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        #region EnterParticipantsCommand

        public ICommand EnterParticipantsCommand { get; }

        private void EnterParticipants()
        {
            NavigationService.NavigateAsync(nameof(EditTrainingPage), new NavigationParameters { { "SelectedTraining", SelectedTraining } });
        }

        private bool CanEnterParticipants()
        {
            return SelectedTraining != null;
        }

        #endregion

        public static IEnumerable<TrainingsTeilnahme> GetParticipantListForTraining(IEnumerable<TrainingsTeilnahme> appearances, Training training)
        {
            IEnumerable<TrainingsTeilnahme> participations;

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
