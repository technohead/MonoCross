using System;
using System.Collections.Generic;

namespace MonoCross.Navigation
{
    public interface IMXController
    {
        Dictionary<string, object> Parameters { get; set; }
        String Uri { get; set; }
        IMXView View { get; set; }
        Type ModelType { get; }
        object GetModel();

        string Load(Dictionary<string, object> parameters);
        void RenderView();
    }

    public abstract class MXController<T> : IMXController
    {
        public string Uri { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }

        public virtual IMXView View { get; set; }

		public object GetModel() { return Model; }
        public abstract string Load(Dictionary<string, object> parameters);
        public virtual void RenderView() { 
			if (View != null) 
				View.Render(); 
		}
    }
}
