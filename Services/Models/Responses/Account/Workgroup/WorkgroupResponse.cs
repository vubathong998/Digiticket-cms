using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class WorkgroupResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public string Url { get; set; }
        public double CreditLine { get; set; }
        public DateTime CreateTime { get; set; }
        public long CreateBy { get; set; }
        public object CreateByName { get; set; }
        public object CreateByAvartar { get; set; }
        public object CreateByDisplayName { get; set; }
        public long UpdateBy { get; set; }
        public object UpdateByName { get; set; }
        public object UpdateByAvartar { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
