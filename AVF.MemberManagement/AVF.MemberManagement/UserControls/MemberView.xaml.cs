using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AVF.MemberManagement.UserControls
{
    public partial class MemberView : ContentView
    {
        public MemberView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty OriginalBindingContextProperty =
            BindableProperty.Create(nameof(OriginalBindingContext), typeof(object), typeof(MemberView));

        public object OriginalBindingContext
        {
            get => (object)GetValue(OriginalBindingContextProperty);
            set => SetValue(OriginalBindingContextProperty, value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MemberView));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}
