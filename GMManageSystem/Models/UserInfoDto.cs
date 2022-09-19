using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GMManageSystem.Models
{

    public class UserInfoDto : ContactDetail
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public string Remark { get; set; }
        public string AuthorizationMAC { get; set; }

    }
}