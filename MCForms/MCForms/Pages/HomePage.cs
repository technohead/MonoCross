using System;
using Xamarin.Forms;
using MonoCross.Forms;

namespace MCForms
{
	public class HomePage : MXFormsPage<Home>
	{
		public HomePage ()
		{
			BackgroundColor = Color.Blue;

			var button = new Button();
			button.Text = "Press me";
			button.Clicked += (object sender, EventArgs e) => {
				MXFormsContainer.Navigate(this, "About");
			};

			this.Content = button;

		}

		#region implemented abstract members of MXFormsPage

		public override void Render ()
		{


		}

		#endregion
	}
}

