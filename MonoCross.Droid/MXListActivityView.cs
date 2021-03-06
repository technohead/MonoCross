using Android.App;
using Android.OS;
using MonoCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCross.Droid
{
    public abstract class MXListActivityView<T> : ListActivity, IMXView
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // fetch the model before rendering!!!
            var t = typeof(T);
            if (MXDroidContainer.ViewModels.ContainsKey(t))
            {
                SetModel(MXDroidContainer.ViewModels[t]);
            }
            else
            {
                var mapping = MXContainer.Instance.App.NavigationMap.FirstOrDefault(layer => layer.Controller.ModelType == t);
                if (mapping == null)
                {
                    throw new ApplicationException("The navigation map does not contain any controllers for type " + t);
                }
                mapping.Controller.Load(new Dictionary<string, object>());
                SetModel(mapping.Controller.GetModel());
            }

            // render the model within the view
            Render();
        }

        private T _model;
        public T Model
        {
            get { return _model; }
            set { _model = value; NotifyModelChanged(); }
        }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();

        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public event ModelEventHandler ViewModelChanged;
        protected virtual void OnViewModelChanged(object model) { }
        private void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(Model); }
    }
}