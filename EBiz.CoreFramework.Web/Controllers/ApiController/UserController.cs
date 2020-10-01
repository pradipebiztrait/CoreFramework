using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.Infrastructure.Helper;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
    [ApiController]
    public class UserController : ApiBaseController
    {
		#region 'Declair Variable'
		public readonly IUserService _userService;
		//public readonly IImageHelper _imageHelper;
		//public readonly SiteSettings _siteSettings;
		//public readonly SiteFolders _siteFolders;
		public readonly AWSProperty _awsProperty;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public UserController(
			IUserService userService
			//, IImageHelper imageHelper
			//, IOptions<SiteSettings> siteSettings
			//, IOptions<SiteFolders> siteFolders
			, IOptions<AWSProperty> awsProperty
			)
		{
			_userService = userService;
			//_imageHelper = imageHelper;
			//_siteSettings = siteSettings.Value;
			//_siteFolders = siteFolders.Value;
			_awsProperty = awsProperty.Value;
		}
		#endregion 'Constructor'

		#region 'POST'
		[HttpPost("GetProfile")]
		public async Task<IActionResult> GetProfile([FromBody]JObject req)
		{
			var id = req["UserId"].ToObject<Int64>();
			return Ok(await _userService.GetProfile(ApiToken,id));
		}

        //[HttpPost("SaveProfile")]
        //public async Task<ApiResponse> SaveProfile([FromForm]User model)
        //{
        //	var response = new ApiResponse();

        //	model.Token = ApiToken;
        //	var data = await _userRepository.UpdateUserProfileAsync(model);

        //	if (model.ProfileImage != null)
        //	{
        //		var guid = !string.IsNullOrEmpty(model.ImageName) ? Path.GetFileNameWithoutExtension(model.ImageName) : Guid.NewGuid().ToString();

        //		var ext = Path.GetExtension(model.ProfileImage.FileName);
        //		var _path = _siteFolders.UserFolder + "/" + model.UserId;

        //		var dt = await _imageHelper.AwsSaveImageAsync(model.ProfileImage, guid, _path);

        //		var _imgPath = dt.Data;
        //		model.ImageName = guid + ext;

        //		if (dt.Status)
        //		{
        //			_userRepository.UpdateUserImagePath(model.UserId, model.ImageName);

        //			data.Data.ImagePath = _awsProperty.BaseUrl + "/images/User/" + data.Data.UserId + "/" + model.ImageName;
        //		}
        //	}			

        //	data.Data.Password = null;

        //	response.Data = data.Status ? data.Data : null;
        //	response.Message = data.Message;
        //	response.Status = data.Status;

        //	return response;
        //}
        [HttpPost("GetAllUserAsync")]
        public async Task<JsonResult> GetAllUserAsync([FromQuery]FilterQuery request)
        {

            var result = await _userService.GetAllUserByFiltersAsync(new FilterHelper().Set(request));

            return null;
        }
        #endregion 'POST'
    }
}