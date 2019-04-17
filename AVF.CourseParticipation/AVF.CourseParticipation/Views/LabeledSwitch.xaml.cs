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
	public partial class LabeledSwitch : ContentView
	{
	    public static readonly BindableProperty TextProperty =
	        BindableProperty.Create(nameof(Text), typeof(string), typeof(LabeledSwitch), defaultValue: string.Empty);
	    public string Text
	    {
	        get => (string)GetValue(TextProperty);
	        set => SetValue(TextProperty, value);
	    }
        
	    public static readonly BindableProperty IsToggledProperty =
	        BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(LabeledSwitch), defaultValue: false);
	    public bool IsToggled
        {
	        get => (bool)GetValue(IsToggledProperty);
	        set => SetValue(IsToggledProperty, value);
	    }

        public LabeledSwitch()
        {
            BindingContext = this;

			InitializeComponent ();
		}
	}
}