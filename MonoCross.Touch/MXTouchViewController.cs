using System;

using MonoTouch.UIKit;
using MonoTouch.Dialog;

using MonoCross.Navigation;
using MonoTouch.Foundation;

namespace MonoCross.Touch
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MXTouchViewController<T>: UIViewController, IMXView
    {
        public MXTouchViewController ()
        {
        }

        public MXTouchViewController (string nibname, NSBundle bundle) : base(nibname, bundle)
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
    }

    public abstract class MXTouchTableViewController<T>: UITableViewController, IMXView
    {
        public MXTouchTableViewController()
        {
        }

        public MXTouchTableViewController(UITableViewStyle style) : base(style)
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
    }
	
	public abstract class MXTouchDialogView<T>: DialogViewController, IMXView
	{
        public MXTouchDialogView(UITableViewStyle style, RootElement root, bool pushing):
			base(style, root, pushing)
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
	}
}
