using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.Infrastructure.Helper;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class CompanyController : BaseController
	{
		#region 'Declair Variable'
		private readonly ICompanyService _companyService;
        private readonly IImageHelper _imageHelper;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly SiteFolders _siteFolders;
        private readonly SiteSettings _siteSettings;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public CompanyController(
            ICompanyService companyService
            , IOptions<SiteFolders> siteFolders
            , IOptions<SiteSettings> siteSettings
            , IImageHelper imageHelper
            , IHostingEnvironment appEnvironment)
		{
            _companyService = companyService;
            _siteFolders = siteFolders.Value;
            _siteSettings = siteSettings.Value;
            _imageHelper = imageHelper;
            _appEnvironment = appEnvironment;
        }
		#endregion 'Constructor'

		#region 'Get'
		public async Task<IActionResult> Index()
		{
            var model = new Company();
            var result = await _companyService.GetAsync();

            if (result.Data != null)
            {
                var editModel = new Company()
                {
                    company_id = result.Data.company_id,
                    company_name = result.Data.company_name,
                    email_address = result.Data.email_address,
                    contact_number = result.Data.contact_number,
                    whatsapp_number = result.Data.whatsapp_number,
                    address = result.Data.address,
                    country = result.Data.country,
                    state = result.Data.state,
                    city = result.Data.city,
                    pincode = result.Data.pincode,
                    website_url = result.Data.website_url,
                    facebook_url = result.Data.facebook_url,
                    google_url = result.Data.google_url,
                    twitter_url = result.Data.twitter_url,
                    instagram_url = result.Data.instagram_url,
                    youtube_url = result.Data.youtube_url,
                    ImageName = result.Data.company_logo,
                    company_logo = !string.IsNullOrEmpty(result.Data.company_logo) ? _siteSettings.BaseUrl + _imageHelper.GetImagePath(result.Data.company_logo, _siteFolders.CompanyFolder).Replace("\\", "/") : result.Data.company_logo
                };

                return View(editModel);
            }

            return View(model);
        }

        #endregion 'Get'

        #region 'Post'

        [HttpPost]
        public async Task<ApiResponse> SaveAsync()
        {
            var model = JsonConvert.DeserializeObject<Company>(HttpContext.Request.Form["model"][0]);
            model.created_by = CurrentUser.UserId;
            model.updated_by = CurrentUser.UserId;
            model.created_on = DateTime.Now;
            model.updated_on = DateTime.Now;

            var files = HttpContext.Request.Form.Files;
            string Webrootpath = _appEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(model.company_logo))
            {
                model.company_logo = model.ImageName;
            }

            var result = await _companyService.SaveAsync(model);

            if (!string.IsNullOrEmpty(model.company_logo))
            {
                model.company_logo = _siteSettings.BaseUrl + _imageHelper.GetImagePath(model.ImageName, _siteFolders.CompanyFolder).Replace("\\", "/");
            }

            if (result.Status && files.Count > 0)
            {
                var guid = !string.IsNullOrEmpty(model.ImageName) ? Path.GetFileNameWithoutExtension(model.ImageName) : Guid.NewGuid().ToString();
                model.company_logo = _imageHelper.SaveImage(files[0], guid, "Company", model.ImageName);
                _companyService.UpdateLogoPath(model.company_id, guid + Path.GetExtension(model.company_logo));
            }

            return result;
        }

        #endregion 'Post'
    }
}
