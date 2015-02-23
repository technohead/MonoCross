using System;
using UIKit;
using MonoCross.Navigation;

namespace MonoCross.Touch
{
	abstract public class BaseMXTouchContainer : MXContainer
	{
		protected UIWindow window;
		protected UIApplicationDelegate appDelegate;
		protected SplashViewController splashViewController = null;

		protected BaseMXTouchContainer (MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window): base(theApp)
		{
			this.appDelegate = appDelegate;
			this.window = window;
		}


		protected void StartApplication()
		{
			if (window.Subviews.Length == 0)
			{
				// toss in a temporary view until async initialization is complete
				string bitmapFile = string.Empty;
				MXTouchContainerOptions options = Attribute.GetCustomAttribute(appDelegate.GetType(), typeof(MXTouchContainerOptions)) as MXTouchContainerOptions;
				if (options != null) {
					bitmapFile = options.SplashBitmap;
				}

				if (!String.IsNullOrEmpty(bitmapFile))
				{
					splashViewController = new SplashViewController(bitmapFile);
					window.AddSubview(splashViewController.View);
					window.MakeKeyAndVisible();
				}
			}
		}

		public void ShowSplashView(bool show = true)
		{
			if (show)
			{
				this.window.BringSubviewToFront(this.splashViewController.View);
				this.splashViewController.View.Hidden = false;
			}
			else
			{
				if (this.splashViewController != null)
				{
					this.splashViewController.View.Hidden = true;
					this.splashViewController.View.RemoveFromSuperview();
					this.splashViewController.Dispose();
					this.splashViewController = null;
				}
			}

		}


	}
}

