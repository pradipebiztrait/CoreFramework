using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{
    [ViewComponent(Name = "ManageStaffComponent")]
    public class ManageStaffComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IStaffService _service;

        public ManageStaffComponent(ApplicationDbContext context, IStaffService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(Int64 id)
        {
            var model = new User();
            var _roles = _context.roles.Where(t => t.normalized_role != "SUPERADMIN").ToList();

            if (_roles != null)
            {
                model.Roles = _roles;
            }

            if (id > 0)
            {
                var result = await _service.GetById(id);
               
                model = (User)result;
                model.Roles = _roles;
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
