using EBiz.CoreFramework.Infrastructure.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Utility
{
	public class AuthorizeRole : ActionFilterAttribute
	{
		public RoleType[] _roleType { get; set; }

		public AuthorizeRole(params RoleType[] roleTypes)
		{
			_roleType = roleTypes;
		}

		public override void OnActionExecuting(ActionExecutingContext _fC)
		{
			if (string.IsNullOrEmpty(_fC.HttpContext.Session.GetString("_userRole")))
			{
				_fC.Result = new RedirectResult("~/");
				return;
			}

			var _ur = _fC.HttpContext.Session.GetString("_userRole").ToString();
			if (!_roleType.Any(t => t.ToString() == _ur.ToUpper()))
			{
				if (_ur.ToUpper() == RoleType.ADMIN.ToString() || _ur.ToUpper() == RoleType.SUBADMIN.ToString())
				{
					_fC.Result = new RedirectResult("~/Admin/Dashboard");
				}
				else
				{
					_fC.Result = new RedirectResult("~/Home");
				}

				return;
			}
			base.OnActionExecuting(_fC);
		}
	}
}
