using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AVF.MemberManagement.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BigLabel
    {
        public NamedSize FontSize
        {
            get
            {
                switch (Device.Idiom)
                {
                    case TargetIdiom.Phone:
                        return NamedSize.Default;
                    default:
                        return NamedSize.Medium;
                        
                }
            }
            set
            {
                
            }
        }

        public BigLabel()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(BigLabel), defaultValue:string.Empty);

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(BigLabel), defaultValue: Xamarin.Forms.FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(TextProperty, value);
        }
    }
}