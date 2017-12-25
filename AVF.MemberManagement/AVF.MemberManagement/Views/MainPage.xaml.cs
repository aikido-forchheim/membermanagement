using Xamarin.Forms;

namespace AVF.MemberManagement.Views
{
    public partial class MainPage : ContentPage
    {
        private double width;
        private double height;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.Idiom != TargetIdiom.Phone) return;
                
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    innerGrid.Children.Remove(controlsGrid);
                    innerGrid.RowDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.Children.Add(controlsGrid, 1, 0);
                }
                else
                {
                    innerGrid.Children.Remove(controlsGrid);
                    innerGrid.RowDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(9, GridUnitType.Star) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(16, GridUnitType.Star) });
                    innerGrid.Children.Add(controlsGrid, 0, 1);
                }
            }
        }
    }
}
