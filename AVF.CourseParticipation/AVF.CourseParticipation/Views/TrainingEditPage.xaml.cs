using AVF.CourseParticipation.Models;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class TrainingEditPage
    {
        public TrainingEditPage()
        {
            InitializeComponent();
        }

        public override void KeyPressed(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == "Enter")
            {
                if (ButtonSelectParticipants.IsEnabled)
                {
                    ButtonSelectParticipants.Focus();
                }
                else
                {
                    ListViewTrainers.Focus();
                }
            }

            if (keyEventArgs.Key == "Back")
            {
                Navigation.PopAsync();
            }

            if (keyEventArgs.Key == "T")
            {
                ButtonEditTrainer.Command.Execute(null);
            }
        }
    }
}
