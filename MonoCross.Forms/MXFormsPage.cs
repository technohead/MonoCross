using System;
using Xamarin.Forms;
using MonoCross.Navigation;

namespace MonoCross.Forms
{
	abstract public class MXFormsPage<T> : ContentPage, IMXView 
	{
		public MXFormsPage ()
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

