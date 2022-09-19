using GMManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;
using GMManageSystem.Common;

namespace GMManageSystem.Controllers
{
    public class UserInfoController : Controller
    {
        private GMMS db = new GMMS();
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }
        [LoginFilter]
        public ActionResult OperateIndex()
        {
            return View();
        }
        /// <summary>
        /// 获取所有用户人员信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示条数</param>
        /// <param name="adminName">账户</param>
        /// <param name="adminName">用户名</param>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult GetUserInfo(int page, int limit, string nameOrAccount, string remark, string mac)
        {
            Result result = new Result();
            var userInfo = db.UserInfoes.Where(u => u.Id > 0);
            if (!string.IsNullOrWhiteSpace(nameOrAccount))
            {
                userInfo = userInfo.Where(u => u.Name.Contains(nameOrAccount) || u.Account.Contains(nameOrAccount));
            }
            if (!string.IsNullOrWhiteSpace(remark))
            {
                userInfo = userInfo.Where(u => remark.Contains(u.Remark));
            }
            if (!string.IsNullOrWhiteSpace(mac))
            {
                userInfo = userInfo.Where(u => u.LastLoginMAC.Contains(mac));
            }
            string sql = userInfo.ToString();
            result.code = 0;
            result.count = userInfo.Count();
            result.data = userInfo.OrderByDescending(u => u.CreateDate).Skip(limit * (page - 1)).Take(limit).ToList();
            return Json(result);
        }
        [LoginFilter]
        public ActionResult OperateUser(UserInfoDto userInfoDto)
        {
            Result result = new Result();
            try
            {
                UserInfo userInfo = new UserInfo();
                if (userInfoDto.Id > 0)
                {
                    userInfo = db.UserInfoes.Find(userInfoDto.Id);
                }
                userInfo.Account = userInfoDto.Account;
                userInfo.Name = userInfoDto.Name;
                userInfo.PassWord = userInfoDto.PassWord;
                userInfo.AuthorizationMAC = userInfoDto.AuthorizationMAC;
                ContactDetail contactDetail = new ContactDetail();
                contactDetail.Phone = userInfoDto.Phone;
                contactDetail.Tel = userInfoDto.Tel;
                contactDetail.Email = userInfoDto.Email;
                contactDetail.QQ = userInfoDto.QQ;
                contactDetail.WeChat = userInfoDto.WeChat;
                userInfo.ContactDetails = JsonConvert.SerializeObject(contactDetail);
                userInfo.Remark = userInfoDto.Remark;
                if (userInfo.Id > 0)
                {
                    db.Entry(userInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    result.code = 0;
                    result.msg = "修改成功！";
                }
                else
                {
                    if (db.UserInfoes.Where(x => x.Account == userInfo.Account).FirstOrDefault() != null)
                    {
                        result.code = 1;
                        result.msg = "账号已存在，输入重新输入！";
                    }
                    else
                    {
                        userInfo.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        db.UserInfoes.Add(userInfo);
                        db.SaveChanges();
                        result.code = 0;
                        result.msg = "添加成功！";
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
                result.code = 1;
                return Json(result);
            }
        }
        [LoginFilter]
        public ActionResult UpdateUser(int id, int status)
        {
            Result result = new Result();
            try
            {
                UserInfo userInfo = db.UserInfoes.Find(id);
                userInfo.Status = status;
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                result.msg = "状态更新成功！";
                result.code = 0;
                return Json(result);
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
                result.code = 1;
                return Json(result);
            }
        }
    }
}