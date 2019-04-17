using System.Collections.Generic;
using System.Linq;
using AVF.CourseParticipation.Models;
using Xamarin.Forms;

namespace AVF.CourseParticipation.Views
{
    public partial class TrainerSelectionPage
    {
        private List<object> _tappedListViewMembersObjects = new List<object>();
        private List<object> _tappedListViewSelectedMembersObjects = new List<object>();

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

        private void ListViewMembers_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (_tappedListViewMembersObjects.Contains(e.Item) && _tappedListViewMembersObjects.Last() == e.Item)
            {
                _tappedListViewMembersObjects.Clear();
                ButtonAddSelectedMember.Command.Execute(null);
            }
            else
            {
                _tappedListViewMembersObjects.Clear();
                _tappedListViewMembersObjects.Add(e.Item);
            }
        }

        private void ListViewSelectedMembers_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (_tappedListViewSelectedMembersObjects.Contains(e.Item) && _tappedListViewSelectedMembersObjects.Last() == e.Item)
            {
                _tappedListViewSelectedMembersObjects.Clear();
                ButtonRemoveSelectedMember.Command.Execute(null);
            }
            else
            {
                _tappedListViewSelectedMembersObjects.Clear();
                _tappedListViewSelectedMembersObjects.Add(e.Item);
            }
        }
    }
}
