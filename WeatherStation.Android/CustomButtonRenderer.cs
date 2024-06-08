using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using WeatherStation;
using WeatherStation.Droid;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace WeatherStation.Droid
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // Disable the automatic uppercasing of text
                Control.SetAllCaps(false);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Button.TextProperty.PropertyName)
            {
                // Ensure the text remains in the desired case when changed
                Control.SetAllCaps(false);
            }
        }
    }
}