using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using MonoCross.Forms;
using MonoCross.Navigation;

namespace MCForms.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Forms.Init ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);

			FormsApp formsApp = new FormsApp();

			var vc = formsApp.NavigationPage.CreateViewController ();
			window.RootViewController = vc;
			window.MakeKeyAndVisible ();

			string splashImage = "Default";

			if (UIScreen.MainScreen.Bounds.Height == 568)
				splashImage = String.Format("{0}-568h", splashImage);

			var splashPage = new SplashPage(splashImage);
			vc.PresentViewController(splashPage.CreateViewController(), false, null);

			MXFormsContainer.Initialize(formsApp, formsApp.NavigationPage, (t)=>{
				this.InvokeOnMainThread(()=>{
					vc.DismissViewController(true, null);
				});
			});


			MXFormsContainer.AddView<Home>(typeof(HomePage), ViewPerspective.Default);
			MXFormsContainer.AddView<About>(typeof(AboutPage), ViewPerspective.Default);

			MXFormsContainer.Navigate("Home");
			return true;
		}
	}
}

