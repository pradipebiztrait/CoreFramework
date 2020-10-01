using System;
using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Entities
{
    public class Country
    {
        [Key]
        public Int64 CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}
