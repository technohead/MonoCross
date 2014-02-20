using System;
using System.Linq;
using MonoCross.Navigation;
using MonoTouch.SlideoutNavigation;
using MonoTouch.UIKit;
using MonoCross.Touch;

namespace MonoCross.Touch
{
	public class MXTouchSlideoutContainer : MXContainer
	{
		protected UIWindow window;
		protected UIApplicationDelegate appDelegate;
		protected SplashViewController splashViewController = null;


		public delegate UIViewController RenderLayerDelegate(IMXView view);

		public static RenderLayerDelegate RenderLayer { get; set; }
		public static SlideoutNavigationController Menu { get; protected set; }

		protected MXTouchSlideoutContainer (MXApplication theApp): base(theApp)
		{
		}

		private MXTouchSlideoutContainer (MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window): base(theApp)
		{
			this.appDelegate = appDelegate;
			Menu = new SlideoutNavigationController();
			Menu.BackgroundColor = UIColor.White;

			this.window = window;
			this.window.RootViewController = Menu;



		}

		public static void Initialize(MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window)
		{
			// initialize the application and hold a reference for a bit
			MXTouchSlideoutContainer thisContainer = new MXTouchSlideoutContainer(theApp, appDelegate, window);
			MXContainer.InitializeContainer(thisContainer);

			thisContainer.StartApplication();
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

			if (Menu.NavigationController != null && fromView != null)
				Menu.NavigationController.PushViewController(viewController, true);
			else
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



		}


		public event Action<IMXController> ControllerLoadComplete;

		#region implemented abstract members of MXContainer

		protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
		{
			Console.WriteLine("Controller Load End");

			appDelegate.InvokeOnMainThread( delegate {
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

