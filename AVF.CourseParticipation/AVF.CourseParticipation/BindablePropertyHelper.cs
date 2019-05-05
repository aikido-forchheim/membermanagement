using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AVF.CourseParticipation
{
    public static class BindablePropertyHelper
    {
        public static BindableProperty Create<TDeclaring, T>(string propertyName)
        {
            return BindableProperty.Create(propertyName, typeof(T), typeof(TDeclaring), default(T));
        }

        public static BindableProperty Create<TDeclaring, T>(string propertyName, T defaultValue)
        {
            return BindableProperty.Create(propertyName, typeof(T), typeof(TDeclaring), defaultValue);
        }
    }
}
