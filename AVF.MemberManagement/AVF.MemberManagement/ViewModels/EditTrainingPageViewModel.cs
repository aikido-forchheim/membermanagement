using Prism.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.Views;
using Prism.Navigation;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class EditTrainingPageViewModel : ViewModelBase
    {
        private readonly IRepository<Mitglied> _mitglieder;
        private readonly IRepository<Training> _trainingsRepository;

        private TrainingsModel _training;

        public TrainingsModel SelectedTraining
        {
            get => _training;
            set => SetProperty(ref _training, value);
        }

        private string _annotation;

        public string Annotation
        {
            get => _annotation;
            set => SetProperty(ref _annotation, value);
        }

        private Mitglied _trainer;

        public Mitglied Trainer
        {
            get => _trainer;
            set => SetProperty(ref _trainer, value);
        }

        private Mitglied _cotrainer1;

        public Mitglied Cotrainer1
        {
            get => _cotrainer1;
            set => SetProperty(ref _cotrainer1, value);
        }

        private Mitglied _cotrainer2;

        public Mitglied Cotrainer2
        {
            get => _cotrainer2;
            set => SetProperty(ref _cotrainer2, value);
        }

        private int _participations;

        public int Participations
        {
            get => _participations;
            set => SetProperty(ref _participations, value);
        }

        private string _additionalTrainerText;

        public string AdditionalTrainerText
        {
            get => _additionalTrainerText;
            set => SetProperty(ref _additionalTrainerText, value);
        }

        private bool _isNavigationModeBack;

        public bool IsNavigationModeBack
        {
            get => _isNavigationModeBack;
            set => SetProperty(ref _isNavigationModeBack, value);
        }

        public EditTrainingPageViewModel(INavigationService navigationService, IRepository<Mitglied> mitglieder, IRepository<Training> trainingsRepository, ILogger logger) : base(navigationService, logger)
        {
            _mitglieder = mitglieder;
            _trainingsRepository = trainingsRepository;

            EnterParticipantsCommand = new DelegateCommand(EnterParticipants, CanEnterParticipants);
            ChangeTrainerCommand = new DelegateCommand(ChangeTrainer, CanChangeTrainer);
            LogoutCommand = new DelegateCommand(Logout, CanLogout);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters.InternalParameters.ContainsKey("__NavigationMode") &&
                    parameters.InternalParameters["__NavigationMode"].ToString() == "Back")
                {
                    IsNavigationModeBack = true;
                }
                else
                {
                    IsNavigationModeBack = false;
                }

                if (parameters.ContainsKey("SelectedTraining"))
                {
                    SelectedTraining = (TrainingsModel)parameters["SelectedTraining"];

                    Title = SelectedTraining.Description;

                    Annotation = SelectedTraining.Training.Bemerkung; //erst beim weiter zurück an die Bemerkung binden

                    await SetTrainerFromIds();
                }
                else if (parameters.ContainsKey("Trainer"))
                {
                    var trainer = (Mitglied)parameters["Trainer"];
                    var cotrainer1 = (Mitglied)parameters["Cotrainer1"];
                    var cotrainer2 = (Mitglied)parameters["Cotrainer2"];

                    SelectedTraining.Training.Trainer = trainer.Id;
                    SelectedTraining.Training.Kotrainer1 = cotrainer1?.Id;
                    SelectedTraining.Training.Kotrainer2 = cotrainer2?.Id;
                }

                AdditionalTrainerText = "und 0 weitere...";

                if (SelectedTraining.Training.Kotrainer2 != null && SelectedTraining.Training.Kotrainer2 != -1)
                {
                    AdditionalTrainerText = "und 2 weitere...";
                }
                else if (SelectedTraining.Training.Kotrainer1 != null && SelectedTraining.Training.Kotrainer1 != -1)
                {
                    AdditionalTrainerText = "und 1 weiterer...";
                }

                Participations = SelectedTraining.Participations.Count();

                Title = Globals.Idiom != Idiom.Phone ? $"{SelectedTraining.Date}, {SelectedTraining.Description}" : SelectedTraining.Description;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task SetTrainerFromIds()
        {
            Trainer = await _mitglieder.GetAsync(SelectedTraining.Training.Trainer);

            if (SelectedTraining.Training.Kotrainer1 != null && SelectedTraining.Training.Kotrainer1 != -1)
            {
                Cotrainer1 = await _mitglieder.GetAsync((int)SelectedTraining.Training.Kotrainer1);
            }
            else
            {
                Cotrainer1 = null;
            }

            if (SelectedTraining.Training.Kotrainer2 != null && SelectedTraining.Training.Kotrainer2 != -1)
            {
                Cotrainer2 = await _mitglieder.GetAsync((int)SelectedTraining.Training.Kotrainer2);
            }
            else
            {
                Cotrainer2 = null;
            }
        }

        #region EnterParticipantsCommand

        public ICommand EnterParticipantsCommand { get; }

        private async void EnterParticipants()
        {
            try
            {
                SelectedTraining.Training.Bemerkung = Annotation;

#pragma warning disable 4014
                //Really navigate asnyc to avoid button event fired multiple times
                NavigationService.NavigateAsync(nameof(SaveParticipantsPage), new NavigationParameters { { "SelectedTraining", SelectedTraining } });
#pragma warning restore 4014

                if (SelectedTraining.Training.Id == 0)
                {
                    await _trainingsRepository.CreateAsync(SelectedTraining.Training);
                }
                else
                {
                    await _trainingsRepository.UpdateAsync(SelectedTraining.Training);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private bool CanEnterParticipants()
        {
            return true;
        }

        #endregion

        #region ChangeTrainerCommand

        public ICommand ChangeTrainerCommand { get; }

        private void ChangeTrainer()
        {
            try
            {
                var navigationParameters = new NavigationParameters { { "SelectedTraining", SelectedTraining }, { "Trainer", Trainer }, { "Cotrainer1", Cotrainer1 }, { "Cotrainer2", Cotrainer2 } };

                NavigationService.NavigateAsync(nameof(SelectTrainerPage), navigationParameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private bool CanChangeTrainer()
        {
            return true;
        }

        #endregion

        #region LogoutCommand

        public ICommand LogoutCommand { get; }

        private void Logout()
        {
            NavigationService.GoBackToRootAsync();
        }

        private bool CanLogout()
        {
            return true;
        }

        #endregion
    }
}
