using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GMManageSystem.Models
{
    public class Result
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public object data { get; set; }


    }
}