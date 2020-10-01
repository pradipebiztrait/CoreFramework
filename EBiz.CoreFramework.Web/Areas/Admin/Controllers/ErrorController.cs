using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using EBiz.CoreFramework.Service.Services;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class ErrorController : BaseController
    {

		#region 'Constructor'
		public ErrorController()
		{
			
		}
        #endregion 'Constructor'

        #region 'Get'

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            var errorModel = new ErrorModel();
            if (code == 403)
            {
                errorModel.StatusCode = code;
                errorModel.ErrorTitle = "Access Denied/Forbidden";
                errorModel.ErrorMessage = "You don't have permission to access this page.";
            }
            else if (code == 404)
            {
                errorModel.StatusCode = code;
                errorModel.ErrorTitle = "Oops! Page not found.";
                errorModel.ErrorMessage = "We could not find the page you were looking for.";
            }
            else
            {
                errorModel.StatusCode = code;
                errorModel.ErrorTitle = "Oops! Page not found.";
                errorModel.ErrorMessage = "We could not find the page you were looking for.";
            }
            return View(errorModel);
        }

        #endregion 'Get'
    }
}