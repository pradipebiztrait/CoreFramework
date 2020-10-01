using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Service.Services;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
    [ApiController]
    public class StaffController : ApiBaseController
	{
		#region 'Declair Variable'
		public readonly IUserService _userService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public StaffController(IUserService userService)
		{
			_userService = userService;
		}
		#endregion 'Constructor'

		#region 'POST'
		[HttpPost("GetAllStaff")]
		public async Task<IActionResult> GetAllStaff()
		{
			return Ok(await _userService.GetAllAsync());
		}

		#endregion 'POST'
	}
}