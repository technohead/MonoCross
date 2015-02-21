using System;
using MonoCross.Navigation;
using Xamarin.Forms;

namespace MonoCross.Forms
{
	public class MXFormsNavigation 
	{
		public NavigationPage NavigationPage { protected set; get; }

		public Page Page { get; set; }



		public MXFormsNavigation (NavigationPage navigationPage)
		{
			this.NavigationPage = navigationPage;
		}

		public static ViewNavigationContext GetViewNavigationContext(object view)
		{
			// Show MyCustomAttribute information for the testClass


			//var attr = Attribute.GetCustomAttribute(view.GetType(), typeof(MXTouchViewAttributes)) as MXTouchViewAttributes;
			//if (attr != null)
			//	return attr.NavigationContext;
			//else
				return ViewNavigationContext.Detail;
		}

		public void PushToModel(Page page)
		{

			NavigationPage.PushAsync(page);
		}

	}
}

