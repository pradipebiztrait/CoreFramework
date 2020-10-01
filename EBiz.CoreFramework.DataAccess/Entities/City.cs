using System;
using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Entities
{
    public class City
    {
        [Key]
        public Int64 CityId { get; set; }
        public string CityName { get; set; }
        public Int64 StateId { get; set; }
    }
}
