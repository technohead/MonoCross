using System;
using Xamarin.Forms;

namespace MCForms
{
	public class SplashPage : ContentPage
	{
		protected ImageSource imageSource;


		public SplashPage (string splashResourceName)
		{
			var imageSplash = new Image();
			imageSplash.Source = FileImageSource.FromFile(splashResourceName);;

			this.Content = imageSplash;
		}

	
	}
}

