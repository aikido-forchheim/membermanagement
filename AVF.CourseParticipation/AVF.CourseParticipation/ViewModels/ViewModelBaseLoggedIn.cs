using AVF.CourseParticipation.Models;
using Prism.Mvvm;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
    public class ViewModelBaseLoggedIn : ViewModelBase
    {
        public static int LoggedInMemberId { get; set; }

        public ViewModelBaseLoggedIn(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
