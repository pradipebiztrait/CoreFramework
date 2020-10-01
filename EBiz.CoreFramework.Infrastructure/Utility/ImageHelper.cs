using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Utility
{
	[ScopedService]
	public class ImageHelper : IImageHelper
	{
		private readonly IHostingEnvironment _appEnvironment;
		private static readonly RegionEndpoint bucketRegion = Amazon.RegionEndpoint.USEast1;
		private readonly SiteSettings _siteSettings;
		private readonly AWSProperty _awsProperty;

		public ImageHelper(
			IHostingEnvironment appEnvironment,
			IOptions<SiteSettings> siteSettings,
			IOptions<AWSProperty> awsProperty
			)
		{
			_appEnvironment = appEnvironment;
			_siteSettings = siteSettings.Value;
			_awsProperty = awsProperty.Value;
		}

		public string SaveImage(IFormFileCollection files, string guid, string folder)
		{
			string directoryPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder);

			bool exists = Directory.Exists(directoryPath);

			if (!exists)
				Directory.CreateDirectory(directoryPath);

			var extension = Path.GetExtension(files[0].FileName);

			var uploadPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder + "\\" + guid + extension);

			var existImage = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);

			if (System.IO.File.Exists(existImage)) System.IO.File.Delete(existImage);

			using (var filestream = new FileStream(Path.Combine(uploadPath), FileMode.Create))
			{
				files[0].CopyTo(filestream);
			}

			return Path.Combine(@"images\" + folder + "\\", guid + extension);
		}

		public string SaveImage(IFormFile file, string guid, string folder, string fileName)
		{
			string directoryPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder);

			bool exists = Directory.Exists(directoryPath);

			if (!exists)
				Directory.CreateDirectory(directoryPath);

			var extension = Path.GetExtension(file.FileName);

			var oldFilePath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder + "\\" + fileName);

			var uploadPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder + "\\" + guid + extension);

			var existImage = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);

			if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);

			using (var filestream = new FileStream(Path.Combine(uploadPath), FileMode.Create))
			{
				file.CopyTo(filestream);
			}

			return Path.Combine(@"images\" + folder + "\\", guid + extension);
		}

		public string GetImagePath(string imagePath, string folder)
		{
			return Path.Combine(@"\images\" + folder + "\\", imagePath);
		}

		public string SavePdf(IFormFileCollection files, string guid, string folder)
		{
			var extension = Path.GetExtension(files[0].FileName);
			var uploadPath = Path.Combine(_appEnvironment.WebRootPath, @"pdf\" + folder + "\\" + guid + extension);

			var existImage = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);

			if (System.IO.File.Exists(existImage)) System.IO.File.Delete(existImage);

			using (var filestream = new FileStream(Path.Combine(uploadPath), FileMode.Create))
			{
				files[0].CopyTo(filestream);
			}

			return Path.Combine(@"pdf\" + folder + "\\", guid + extension);
		}

		public void SaveBase64ToImage(string base64String, string folder, string guid)
		{
			var bytes = Convert.FromBase64String(base64String);

			string directoryPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder);

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			string file = Path.Combine(directoryPath, guid + ".png");

			if (bytes.Length > 0)
			{
				using (var stream = new FileStream(file, FileMode.Create))
				{
					stream.Write(bytes, 0, bytes.Length);
					stream.Flush();
				}
			}
		}

		public async Task<ApiResponse> AwsSaveImageAsync(IFormFile files, string guid, string folder)
		{
			var _res = new ApiResponse();
			try
			{
				//check directory
				string directoryPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder);
				bool exists = Directory.Exists(directoryPath);

				if (!exists)
					Directory.CreateDirectory(directoryPath);

				//check image
				var extension = Path.GetExtension(files.FileName);
				var uploadPath = Path.Combine(_appEnvironment.WebRootPath, @"images\" + folder + "\\" + guid + extension);
				var existImage = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
				if (System.IO.File.Exists(existImage)) System.IO.File.Delete(existImage);

				using (var filestream = new FileStream(Path.Combine(uploadPath), FileMode.Create))
				{
					files.CopyTo(filestream);
					TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
					request.InputStream = filestream;
					request.BucketName = _awsProperty.BucketName;
					request.Key = @"images/" + folder + "/" + guid + extension;
					request.CannedACL = S3CannedACL.PublicRead;

					TransferUtility transferUtility = new TransferUtility(
						_awsProperty.AccessKey,
						_awsProperty.SecretKey,
						bucketRegion);

					await transferUtility.UploadAsync(request);

					if (System.IO.File.Exists(existImage)) System.IO.File.Delete(existImage);
				}

				_res.Status = true;
				_res.Message = "Success";
				_res.Data = _awsProperty.BaseUrl + "\\" + Path.Combine(@"images\" + folder + "\\", guid + extension);
			}
			catch (Exception ex)
			{
				_res.Status = false;
				_res.Message = ex.Message;
			}

			return _res;
		}
	}
}
