using IAIOT_alpha_0_1_1.Enums;
using System.Net;
using System.Text;

namespace Final_project_IAIOTCloud.Utility
{
    public class HttpRequestEntity
    {
		public HttpMethod Method
		{
			get;
			set;
		}

		public Encoding Encoding
		{
			get;
			set;
		}

		public string ContentType
		{
			get;
			set;
		}

		public WebHeaderCollection Headers
		{
			get;
			set;
		}

		public string Cookies
		{
			get;
			set;
		}

		public string Datas
		{
			get;
			set;
		}

		public HttpRequestEntity()
		{
			this.Headers = new WebHeaderCollection();
		}
	}
}
