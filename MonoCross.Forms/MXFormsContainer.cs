using System;
using MonoCross.Navigation;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace MonoCross.Forms
{
	public class MXFormsContainer : MXContainer
	{
		public event Action<IMXController> ControllerLoadBegin;
		public event Action<IMXController, Exception> ControllerLoadFailed;
		public event Action<IMXController> ControllerLoadComplete;

		public delegate Page RenderLayerDelegate(IMXView view);
		public static RenderLayerDelegate RenderLayer { get; set; }


		protected Action<Task> onInitializationFinished;
		protected MXFormsNavigation formsNavigation;
		static private bool firstView = true;



		public MXFormsContainer (MXApplication app, NavigationPage navigationPage, Action<Task> onInitializationFinished = null) : base (app)
		{
			this.formsNavigation = new MXFormsNavigation(navigationPage);
			this.onInitializationFinished = onInitializationFinished;

			MXContainer.InitializeContainer(this);
			startApplication();
		}


		public static void Initialize(MXApplication theApp, NavigationPage navigationPage, Action<Task> onInitializationFinished = null)
		{
			// initialize the application and hold a reference for a bit
			MXFormsContainer thisContainer = new MXFormsContainer(theApp, navigationPage, onInitializationFinished);

		}


		protected virtual void startApplication()
		{
			Task tInitTask = new Task(initApplicationAsync, TaskCreationOptions.LongRunning);
			tInitTask.Start();
			tInitTask.ContinueWith((t)=>{
				if (this.onInitializationFinished != null)
					this.onInitializationFinished(t);
			});

		}

		async protected virtual void initApplicationAsync()
		{
			//await Task.Delay(10000);

		}



		#region implemented abstract members of MXContainer

		protected override void OnControllerLoadBegin(IMXController controller)
		{
			//Console.WriteLine("Controller Load Begin");

			if (ControllerLoadBegin != null) {
				ControllerLoadBegin(controller);
				return;
			}

			showLoading();
		}
		protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
		{
			//Console.WriteLine("Controller Load End");
			Xamarin.Forms.Device.BeginInvokeOnMainThread(()=>{
				LoadViewForController(fromView, controller, viewPerspective);
				if (ControllerLoadComplete != null)
					ControllerLoadComplete(controller);
			});

		}

		protected override void OnControllerLoadFailed (IMXController controller, Exception ex)
		{
			//Console.WriteLine("Controller Load Failed: " + ex.Message);

			if (ControllerLoadFailed != null) {
				ControllerLoadFailed(controller, ex);
				return;
			}

			hideLoading();

			Xamarin.Forms.Device.BeginInvokeOnMainThread(()=>{
				this.formsNavigation.NavigationPage.DisplayAlert("Load Failed", ex.Message, null, "OK");
			});
		}


		public override void Redirect(string url)
		{
			MXFormsContainer.Navigate(null, url);
			CancelLoad = true;
		}

		#endregion

		public void LoadViewForController(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
		{
			hideLoading();

			if (controller.View == null)
			{
				// get the view, create it if it has yet been created
				controller.View = Views.GetOrCreateView(viewPerspective);
				if (controller.View == null)
				{
					//Console.WriteLine("View not found for perspective!" + viewPerspective.ToString());
					throw new ArgumentException("View creation failed for perspective!" + viewPerspective.ToString());
				}
			}


			// asign the view it's model and render the contents of the view
			controller.View.SetModel(controller.GetModel());
			controller.View.Render();

			// pull the type from the view
			//ViewNavigationContext navigationContext = MXFormsNavigation.GetViewNavigationContext(controller.View);
			Page page = controller.View as Page;

			// iFactr binding options
			if (page == null)
				page = RenderLayer(controller.View);

			ViewNavigationContext navigationContext = MXFormsNavigation.GetViewNavigationContext(controller.View);
			formsNavigation.PushToModel(page);



		}



		private void ShowView ()
		{
			if (firstView)
			{
				/*
				foreach (var view in window.Subviews)
					view.RemoveFromSuperview();

				firstView = false;
				window.Add(touchNavigation.View);
				window.MakeKeyAndVisible();
				*/
			}
		}
		protected virtual void showLoading()
		{

		}

		protected virtual void hideLoading()
		{

		}
	}
}

