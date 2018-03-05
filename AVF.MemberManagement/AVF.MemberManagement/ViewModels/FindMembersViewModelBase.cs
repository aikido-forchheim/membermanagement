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

        private ObservableCollection<Mitglied> _foundMembers = new ObservableCollection<Mitglied>();

        public ObservableCollection<Mitglied> FoundMembers
        {
            get => _foundMembers;
            set => SetProperty(ref _foundMembers, value);
        }

        public FindMembersViewModelBase(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
        }
    }
}
