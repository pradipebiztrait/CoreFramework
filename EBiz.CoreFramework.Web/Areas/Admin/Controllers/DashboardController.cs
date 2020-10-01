using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.Infrastructure.Enum;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	[AuthorizeRole(RoleType.ADMIN, RoleType.SUBADMIN)]
    public class DashboardController : BaseController
    {

		#region 'Declair Variable'
		private readonly IDashboardService _dashboardService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public DashboardController(IDashboardService dashboardService)
		{
			_dashboardService = dashboardService;
		}
		#endregion 'Constructor'

		public IActionResult Index()
        {
			return View();
        }

		#region 'ViewComponent'
		
		#endregion 'ViewComponent'
	}
}