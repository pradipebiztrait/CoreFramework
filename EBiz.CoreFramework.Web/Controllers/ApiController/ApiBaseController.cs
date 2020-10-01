using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Web.Utility;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
    [Route("api")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
		public string _key()
		{
			var _k = HttpContext.Request.Headers["Authorization"].ToString();
			return !string.IsNullOrEmpty(_k) ? _k.Substring("Bearer ".Length).Trim() : null;
		}

		public string ApiToken { get { return _key(); } }
	}
}