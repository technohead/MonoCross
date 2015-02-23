using System;
using System.Linq;
using MonoCross.Navigation;
using MonoTouch.SlideoutNavigation;
using UIKit;
using MonoCross.Touch;

namespace MonoCross.Touch
{
	public class MXTouchSlideoutContainer : BaseMXTouchContainer
	{
		public delegate UIViewController RenderLayerDelegate(IMXView view);

		public static RenderLayerDelegate RenderLayer { get; set; }
		public static SlideoutNavigationController Menu { get; protected set; }

		protected MXTouchSlideoutContainer (MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window): base(theApp, appDelegate, window)
		{
		}



		public static void Initialize(MXApplication theApp, UIApplicationDelegate appDelegate, UIWindow window)
		{
			// initialize the application and hold a reference for a bit
			MXTouchSlideoutContainer thisContainer = new MXTouchSlideoutContainer(theApp, appDelegate, window);
			MXContainer.InitializeContainer(thisContainer);

			thisContainer.StartApplication();
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
			{

				if (Menu.NavigationController.ViewControllers.Contains(viewController))
					Menu.NavigationController.PopToViewController(viewController, true);
				else
					Menu.NavigationController.PushViewController(viewController, true);
			}
			else
			{
				if (Menu.NavigationController != null)
					Menu.NavigationController.PopToRootViewController(true);

				Menu.TopView = viewController;
			}

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

