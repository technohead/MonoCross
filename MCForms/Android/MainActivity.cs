using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using MonoCross.Forms;
using MonoCross.Navigation;
using System.Threading.Tasks;


namespace MCForms.Android
{
	[Activity (Label = "MCForms.Android.Android", MainLauncher = true)]
	public class MainActivity : AndroidActivity
	{
		async protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);

			SplashPage splashPage = new SplashPage("@drawable/splash");

			FormsApp formsApp = new FormsApp();

			MXFormsContainer.Initialize(formsApp, formsApp.NavigationPage, (t)=>{
				Xamarin.Forms.Device.BeginInvokeOnMainThread(()=>{
					//SetPage(formsApp.NavigationPage);
				});
			});

			MXFormsContainer.AddView<Home>(typeof(HomePage), ViewPerspective.Default);
			MXFormsContainer.AddView<About>(typeof(AboutPage), ViewPerspective.Default);


			await MXFormsContainer.Navigate("Home");


			SetPage(formsApp.NavigationPage);

		}
	}
}

