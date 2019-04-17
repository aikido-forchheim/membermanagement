using AVF.CourseParticipation.Models;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class TrainerSelectionPage
    {
        public TrainerSelectionPage()
        {
            InitializeComponent();
        }
        public override void KeyPressed(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == "Enter")
            {
                
            }

            if (keyEventArgs.Key == "Back")
            {
                Navigation.PopAsync();
            }

            if (keyEventArgs.Key == "C")
            {
                
            }
        }
    }
}
