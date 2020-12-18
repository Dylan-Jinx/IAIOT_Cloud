using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Final_project_IAIOTCloud.Utility
{
    public class HttpResponseEntity
    {
		public WebHeaderCollection Headers
		{
			get;
			set;
		}

		public string Bodys
		{
			get;
			set;
		}

		public string Cookies
		{
			get;
			set;
		}
	}
}
