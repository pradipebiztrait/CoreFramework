using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
	public class AWSProperty
	{
		public string BucketName { get; set; }
		public string AccessKey { get; set; }
		public string SecretKey { get; set; }
		public string BaseUrl { get; set; }
	}
}
