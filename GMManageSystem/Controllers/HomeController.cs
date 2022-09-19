using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GMManageSystem.Common;
using GMManageSystem.Models;
using Newtonsoft.Json;

namespace GMManageSystem.Controllers
{
    public class HomeController : Controller
    {
        private GMMS db = new GMMS();


        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }
        [LoginFilter]
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult CreateDB()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Account = "admin";
            userInfo.Name = "超级管理员";
            userInfo.PassWord = "admin";
            userInfo.Status = 0;
            userInfo.Power = 1;
            userInfo.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (db.UserInfoes.Where(u => u.Account == userInfo.Account && u.Name == userInfo.Name && u.PassWord == userInfo.PassWord && u.Status == 0 && u.Power == userInfo.Power).FirstOrDefault() == null)
            {
                db.UserInfoes.Add(userInfo);
                db.SaveChanges();
            }
            ViewBag.UserInfo = userInfo;
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(UserInfo userInfo)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(userInfo.Account) || string.IsNullOrWhiteSpace(userInfo.PassWord))
            {
                result.code = 1;
                result.msg = "账号和密码不能为空";
                return Json(result);
            }
            userInfo = db.UserInfoes.Where(u => u.Account == userInfo.Account).FirstOrDefault();
            string strMac = Common.Utility.GetMacAddress();
            if (userInfo == null)
            {
                result.code = 1;
                result.msg = "账号不存在，请检查输入账户是否有误！";
            }
            else if (userInfo.Status != 0)
            {
                result.code = 1;
                result.msg = "账号已经停用，请联系管理员开启！";
            }
            else if (userInfo.PassWord != userInfo.PassWord)
            {
                result.code = 1;
                result.msg = "密码错误，请检查输入密码是否有误！";
            }
            else if (userInfo.AuthorizationMAC != strMac && userInfo.Power == 0)
            {
                result.code = 1;
                result.msg = "授权Mac和登录Mac不一致，拒绝登录！";
            }
            else
            {
                userInfo.LastLoginMAC = Common.Utility.GetMacAddress();
                if (string.IsNullOrWhiteSpace(userInfo.LastLoginMAC))
                {
                    result.code = 1;
                    result.msg = "异常登录，MAC地址获取失败！";
                    return Json(result);
                }
                userInfo.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(userInfo.Id.ToString(), true);
                result.code = 0;
                result.msg = "登录成功！";
                result.data = userInfo.Name;
            }
            return Json(result);
        }
    }
}
