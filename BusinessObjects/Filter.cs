using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BusinessObjects
{
    public class Filter : IFilter
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public int? Id { get; set; }

        [JsonIgnore]
        public string TableName { get; set; }

        [JsonIgnore]
        public string WhereCondition { get; set; }

        [JsonIgnore]
        public string OrderByCondition { get; set; }

        [JsonIgnore]
        public string[] Columns { get; set; }

        [JsonIgnore]
        public string[] FunctionParams { get; set; }

        public Filter()
        {
            Columns = new string[] { };
            FunctionParams = new string[] { };
        }

        
    }

    public interface IFilter
    {
        int PageNo { get; set; }
        int PageSize { get; set; }
        string TableName { get; set; }
        string WhereCondition { get; set; }
        string OrderByCondition { get; set; }
        string[] Columns { get; set; }
        string[] FunctionParams { get; set; }
    }
}