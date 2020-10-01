using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{
    [ViewComponent(Name= "ManagePageComponent")]
    public class ManagePageComponent : ViewComponent
    {
        private readonly ICmsService _cmsService;
        public ManagePageComponent(ICmsService cmsService)
        {
            _cmsService = cmsService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Int64 id)
        {
            var model = new Pages();
            if (id > 0)
            {
                var page = await _cmsService.GetAsync(id);

                model.PageId = page.Data.PageId;
                model.PageTitle = page.Data.PageTitle;
                model.PageUrl = page.Data.PageUrl;
                model.PageDescription = page.Data.PageDescription;
                model.IsActive = page.Data.IsActive;
                model.is_status = (page.Data.IsActive == 1 ? true : false);
            }
            
            return await Task.FromResult((IViewComponentResult)View("Default",model));
        }
    }
}
