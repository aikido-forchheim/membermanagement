using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AVF.CourseParticipation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MemberViewCell : ViewCell
	{
	    public static readonly BindableProperty FontAttributesProperty =
	        BindablePropertyHelper.Create<MemberViewCell, FontAttributes>(nameof(FontAttributes));

	    public FontAttributes FontAttributes
	    {
	        get => (FontAttributes) GetValue(FontAttributesProperty);
	        set => SetValue(FontAttributesProperty, value);
	    }   

		public MemberViewCell ()
		{
			InitializeComponent ();
		}
	}
}