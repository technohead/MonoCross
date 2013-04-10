using System;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
    /// <summary>
    /// Class to display the initial view when still warming up
    /// </summary>
	internal class SplashViewController: UIViewController
	{
		UIImageView _imageView;
		
		public SplashViewController (string imageFile)
		{
			UIImage image = null;
			
			if (!String.IsNullOrEmpty(imageFile))
			{
				if (UIScreen.MainScreen.Bounds.Height == 568)
					image = UIImage.FromBundle(String.Format("{0}-568h", imageFile));
				else
					image = UIImage.FromBundle(imageFile);
			}

			if (image != null)
			{
				_imageView = new UIImageView(image);
				_imageView.ContentMode = UIViewContentMode.Center;
				_imageView.BackgroundColor = UIColor.White;
			
				this.View = _imageView;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.View.BackgroundColor = UIColor.Clear;
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);
			
			/*
			UIImageView view = this.View as UIImageView;
			
			if (toInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || 
			    toInterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
				view.Image = UIImage.FromFile("Images/Launch-Landscape.png");
			else
				view.Image = UIImage.FromFile("Images/Launch-Portrait.png");
			*/
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

