using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GMManageSystem.Models
{

    public class UserInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Account { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string PassWord { get; set; }
        public string ContactDetails { get; set; }
        public string Remark { get; set; }
        public string AuthorizationMAC { get; set; }
        public string LastLoginMAC { get; set; }
        public string LastLoginDate { get; set; }
        public string CreateDate { get; set; }
        public int Status { get; set; }
        public int Power { get; set; }

    }
}