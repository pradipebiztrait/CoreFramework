using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
	public class Notification
	{
		[Key]
		public Int64 NotificationId { get; set; }
		public Int64 UserId { get; set; }
		public string Message { get; set; }
		public int? IsDelete { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		[NotMapped]
		public string Time { get; set; }
	}
}
