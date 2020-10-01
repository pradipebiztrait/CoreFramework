using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
	public class ApiResponse
	{
		public string Message { get; set; }
		public bool Status { get; set; }
		public int StatusCode { get; set; }
		public dynamic Data { get; set; }
		public int TotalPage { get; set; }
		public int PageNumber { get; set; }
		public int TotalRecord { get; set; }
		public dynamic ReturnId { get; set; }
	}
}
