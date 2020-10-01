using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using EBiz.CoreFramework.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{

    [ViewComponent(Name = "ManageRoleComponent")]
    public class ManageRoleComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;

        public ManageRoleComponent(ApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Int64 id)
        {
            var model = new Roles();

            if (id > 0)
            {
                var result = await _roleService.GetAsync(id);

                model.role_id = result.Data.role_id;
                model.role_name = result.Data.role_name;
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }

    [ViewComponent(Name = "ManageRolePermissionComponent")]
    public class ManageRolePermissionComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;

        public ManageRolePermissionComponent(ApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Int64 id)
        {
            var model = new List<RolePermission>();

            if (id > 0)
            {
                var result = await _roleService.GetUserPermissionAsync(id);

                model = (List<RolePermission>)result.Data;
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
