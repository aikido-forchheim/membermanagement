using Windows.UI.Xaml;
using AVF.CourseParticipation.Models;
using AVF.CourseParticipation.UWP.Renderer;
using AVF.CourseParticipation.Views;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CourseSelectionPage), typeof(CourseSelectionPageRenderer))]
namespace AVF.CourseParticipation.UWP.Renderer
{
    public class CourseSelectionPageRenderer : PageRenderer
    {
        /// <summary>
        /// Monitor windows core ui keypress when ExplorePage is showing
        /// </summary>
        public CourseSelectionPageRenderer() : base()
        {
            // When ExplorePage appears then attach to the ui core keydown event
            Loaded += (object sender, RoutedEventArgs e) =>
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown += HandleKeyDown;
            };
            // When ExplorePage disappears then detach from the ui core keydown event
            Unloaded += (object sender, RoutedEventArgs e) =>
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown -= HandleKeyDown;
            };
        }
        /// <summary>
        /// Forward a keypress to an ExplorePage method
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e"></param>
        public void HandleKeyDown(Windows.UI.Core.CoreWindow window, Windows.UI.Core.KeyEventArgs e)
        {
            (Element as CourseSelectionPage).KeyPressed(Element, new KeyEventArgs { Key = e.VirtualKey.ToString() });
        }
    }
}