using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
	[ApiController]
	public class LoginController : ApiBaseController
	{
		#region 'Declair Variable'
		private readonly IUserService _userService;
		private readonly ITokenService _tokenService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public LoginController(IUserService userService, ITokenService tokenService)
		{
			_userService = userService;
			_tokenService = tokenService;
		}
		#endregion 'Constructor'

		#region 'Post'
		[HttpPost("Login")]
		public async Task<ApiResponse> Login([FromBody]JObject req)
		{
			var response = new ApiResponse();
			try
			{
				var request = new AccessDevice();
				request.UserName = req["EmailAddress"].ToObject<string>();
				request.Email = req["EmailAddress"].ToObject<string>();
				request.Password = req["Password"].ToObject<string>();
				request.deviceToken = req["DeviceToken"].ToObject<string>();
				request.deviceType = req["DeviceType"].ToObject<int>();
				request.udid = req["UDID"].ToObject<string>();
				request.RoleId = req["RoleId"].ToObject<int>();

				var data = await _tokenService.TokenGenerateWithUDID(request);
				if (data.Item1 != null && data.Item3.Status)
				{
					await _userService.UpdateUserToken(request, data.Item2.UserId, data.Item1);
					data.Item2.Token = data.Item1;
					data.Item2.Password = null;

					response.Status = data.Item3.Status;
					response.Message = data.Item3.Message.ToString();
					response.Data = data.Item3.Status ? data.Item2 : null;
				}
				else
				{
					response.Status = false;
					response.Message = data.Item3.Message;
					response.Data = null;
				}
			}
			catch (Exception ex)
			{
				response.Status = false;
				response.Message = ex.Message.ToString();
				response.Data = null;
			}

			return response;
		}

		[HttpPost("SignUp")]
		public async Task<ApiResponse> SignUp([FromBody]JObject req)
		{
			var response = new ApiResponse();
			try
			{
				var model = new User();

				model.UserId = req["UserId"].ToObject<Int64>();
				model.EmailAddress = req["EmailAddress"].ToObject<string>();
				model.Password = req["Password"].ToObject<string>();
				model.FirstName = req["FirstName"].ToObject<string>();
				model.LastName = req["LastName"].ToObject<string>();
				model.DeviceType = req["DeviceType"].ToObject<int>();
				model.DeviceToken = req["DeviceToken"].ToObject<string>();
				model.UdId = req["UDID"].ToObject<string>();
				model.CityId = req["CityId"].ToObject<Int64>();
				model.StateId = req["StateId"].ToObject<Int64>();
				model.CountryId = req["CountryId"].ToObject<Int64>();
				model.RoleId = req["RoleId"].ToObject<int>();

				var userData = _userService.SignUpAsync(model).Result;

				
				if (userData.Status)
				{
					string tokenData = _tokenService.TokenGenerateSignUp(userData.Data.EmailAddress, userData.Data.UserId);

					var accessTokenModel = new AccessDevice();
					accessTokenModel.Email = userData.Data.EmailAddress;
					accessTokenModel.deviceToken = userData.Data.DeviceToken;
					accessTokenModel.deviceType = userData.Data.DeviceType;
					accessTokenModel.udid = userData.Data.UdId;

					await _userService.UpdateUserToken(accessTokenModel, userData.Data.UserId, tokenData);

					response.Status = true;
					response.Message = "User registered has been successfully.";
					userData.Data.Password = "";
					userData.Data.Token = tokenData;
					response.Data = userData.Data;
				}
				else
				{
					response.Status = false;
					response.Message = userData.Message;
				}
			}
			catch (Exception ex)
			{
				response.Status = false;
				response.Message = ex.Message.ToString();
			}

			return response;
		}
		#endregion 'Post'
	}
}