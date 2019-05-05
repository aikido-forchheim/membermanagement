using System;
using AVF.CourseParticipation.Models;
using Prism.Mvvm;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
    public class ViewModelBaseLoggedIn : ViewModelBase
    {
        private static int? _loggedInMemberId;

        /// <summary>
        /// The LoggedInMemberId is set to null on each beginning of a login process, so if you cast from int? to int, be sure to raise an error
        /// </summary>
        public static int? LoggedInMemberId
        {
            get
            {
                if (_loggedInMemberId == null)
                {
                    throw new ArgumentNullException("The LoggedInMemberId was null. Please verify login process!");
                }
                return _loggedInMemberId;
            }

            set => _loggedInMemberId = value;
        }

        public ViewModelBaseLoggedIn(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
