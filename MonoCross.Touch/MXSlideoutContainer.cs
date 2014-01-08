using System;
using System.Linq;
using MonoCross.Navigation;
using MonoTouch.SlideoutNavigation;
using MonoTouch.UIKit;
using MonoCross.Touch;

namespace MonoCross.Touch
{
	public class MXSlideoutContainer : MXContainer
	{
		UIWindow _window;
		UIApplicationDelegate _appDelegate;
		SplashViewController _splashViewController = null;

		public delegate UIViewController RenderLayerDelegate(IMXView view);

		public static RenderLayerDelegate RenderLayer { get; set; }
		public static SlideoutNavigationController Menu { get; private set; }




		private MXSlideoutContainer (MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window): base(theApp)
		{
			_appDelegate = appDelegate;
			Menu = new SlideoutNavigationController();

			_window = window;
			_window.RootViewController = Menu;



		}

		public static void Initialize(MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window)
		{
			// initialize the application and hold a reference for a bit
			MXSlideoutContainer thisContainer = new MXSlideoutContainer(theApp, appDelegate, window);
			MXContainer.InitializeContainer(thisContainer);

			thisContainer.StartApplication();
		}

		private void StartApplication()
		{

			if (_window.Subviews.Length == 0)
			{
				// toss in a temporary view until async initialization is complete
				string bitmapFile = string.Empty;
				MXTouchContainerOptions options = Attribute.GetCustomAttribute(_appDelegate.GetType(), typeof(MXTouchContainerOptions)) as MXTouchContainerOptions;
				if (options != null) {
					bitmapFile = options.SplashBitmap;
				}

				if (!String.IsNullOrEmpty(bitmapFile))
				{
					_splashViewController = new SplashViewController(bitmapFile);
					_window.AddSubview(_splashViewController.View);
					_window.MakeKeyAndVisible();
				}
			}

		}

		public void LoadViewForController(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
		{
			HideLoading();

			if (controller.View == null)
			{
				// get the view, create it if it has yet been created
				controller.View = Views.GetOrCreateView(viewPerspective);
				if (controller.View == null)
				{
					Console.WriteLine("View not found for perspective!" + viewPerspective.ToString());
					throw new ArgumentException("View creation failed for perspective!" + viewPerspective.ToString());
				}
			}


			// asign the view it's model and render the contents of the view
			controller.View.SetModel(controller.GetModel());
			controller.View.Render();

			// pull the type from the view
			ViewNavigationContext navigationContext = MXTouchNavigation.GetViewNavigationContext(controller.View);
			UIViewController viewController = controller.View as UIViewController;

			// iFactr binding options
			if (viewController == null)
				viewController = RenderLayer(controller.View);


			//slideoutNavigation.PushToModel(viewController);
			Menu.TopView = viewController;

			ShowView();
		}


		void HideLoading()
		{
			/*			
			if (_loadingView != null)
				_loadingView.Hide();
			*/
		}

		static bool _firstView = true;

		private void ShowView ()
		{


			/*
			if (_firstView)
			{
				foreach (var view in _window.Subviews)
					view.RemoveFromSuperview();

				_firstView = false;
			
				_window.Add(Menu.TopView.View);
				_window.MakeKeyAndVisible();
			}
			*/
		}


		public event Action<IMXController> ControllerLoadComplete;

		#region implemented abstract members of MXContainer

		protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
		{
			Console.WriteLine("Controller Load End");

			_appDelegate.InvokeOnMainThread( delegate {
				LoadViewForController(fromView, controller, viewPerspective);
				if (ControllerLoadComplete != null)
					ControllerLoadComplete(controller);
			});
		}

		public override void Redirect (string url)
		{

		}

		#endregion
	}
}

