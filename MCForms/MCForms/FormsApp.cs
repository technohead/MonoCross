using System;
using Xamarin.Forms;
using MonoCross.Forms;

namespace MCForms
{
	public class FormsApp : MXFormsApplication
	{
		public NavigationPage NavigationPage { protected set; get; }

		public FormsApp()
		{
			this.NavigationPage = new NavigationPage();
		}

		public override void OnAppLoad ()
		{

			// add controllers to navigation map
			NavigationMap.Add ("Home", new Controllers.HomeController ());
			NavigationMap.Add ("About", new Controllers.AboutController ());

			NavigateOnLoad = "Home";
		}
	}

}

