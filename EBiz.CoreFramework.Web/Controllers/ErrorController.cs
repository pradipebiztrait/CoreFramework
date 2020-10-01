using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Infrastructure.Utility;
using System;
using EBiz.CoreFramework.DataAccess.Models;
using System.Linq;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.DbContext;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Controllers
{
	public class ErrorController : Controller
    {
        #region 'Declair Variable'
        #endregion 'Declair Variable'

        #region 'Constructor'
        public ErrorController()
        {
        }
        #endregion 'Constructor'

        #region 'Get'
        public IActionResult Index()
        {
			return View();
        }

		
		#endregion 'Get'

		#region 'Post'
		
        #endregion
    }
}
