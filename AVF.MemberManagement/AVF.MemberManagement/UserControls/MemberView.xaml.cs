﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AVF.MemberManagement.UserControls
{
    public partial class MemberView : ContentView
    {
        public MemberView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ParentBindingContextProperty =
            BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(MemberView));

        public object ParentBindingContext
        {
            get => (object)GetValue(ParentBindingContextProperty);
            set => SetValue(ParentBindingContextProperty, value);
        }
    }
}
