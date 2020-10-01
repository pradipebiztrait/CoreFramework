using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Utility
{
	public interface IImageHelper : IScopedService
	{
		string GetImagePath(string imagePath, string folder);
		string SaveImage(IFormFileCollection files, string guid, string folder);
		string SaveImage(IFormFile file, string guid, string folder, string fileName);
		string SavePdf(IFormFileCollection files, string guid, string folder);
		void SaveBase64ToImage(string base64String, string folder, string guid);
		Task<ApiResponse> AwsSaveImageAsync(IFormFile files, string guid, string folder);
	}
}
