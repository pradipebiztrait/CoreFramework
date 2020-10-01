using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class FilterRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public int SearchType { get; set; }
        public string SearchText { get; set; }
        public string SearchCol { get; set; }
        public string SortCol { get; set; }
        public string SortDir { get; set; }
        public string Table { get; set; }
    }

    public class FilterKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class FilterQuery
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public string order { get; set; }
        public string sort { get; set; }
        public string search { get; set; }
        public string filter { get; set; }
    }
}
