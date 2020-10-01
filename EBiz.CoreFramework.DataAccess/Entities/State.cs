using System;
using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Entities
{
    public class State
    {
        [Key]
        public Int64 StateId { get; set; }
        public string StateName { get; set; }
        public Int64 CountryId { get; set; }
    }
}
