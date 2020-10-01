using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Entities
{
	public class Pages
	{
		[Key]
		public Int64 PageId { get; set; }
		public string PageTitle { get; set; }
		public string PageUrl { get; set; }
		public string PageDescription { get; set; }
		public int? IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		public Int64? CreatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
		public Int64? UpdatedBy { get; set; }
	}
}
