using System;
using MonoCross.Navigation;
using System.Collections.Generic;

namespace MCForms.Controllers
{
	public class HomeController : MXController<Home>
	{
		public HomeController ()
		{
		}

		public override string Load (Dictionary<string,object> parameters)
		{
			return string.Empty;
		}
	}
}

