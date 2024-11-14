using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webstide_bằng_mô_hình_MVC.Models.viewmodels;
using webstide_bằng_mô_hình_MVC.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity;
using System.Web.Security;

namespace webstide_bằng_mô_hình_MVC.Controllers
{
    public class taikhoanController : Controller
    {
        private My_StoreEntities2 db = new My_StoreEntities2();
        // GET: taikhoan
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap", "taikhoan");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(dangkyVM model)
        {
            if (ModelState.IsValid)
            {
                    var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                        return View(model);
                    }

                    var user = new User
                    {
                        Username = model.Username,
                        Password = model.Password,
                        UserRole = "Customer"
                    };
                    db.Users.Add(user);
                    var customer = new Customer
                    {
                        CustomerName = model.CustomerName,
                        CustomerEmail = model.CustomerEmail,
                        CustomerPhone = model.CustomerPhone,
                        CustomerAddress = model.CustomerAddress,
                        Username = model.Username,
                    };
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(dangnhapVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.UserRole == "Customer");
                if (user != null)
                {
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    FormsAuthentication.SetAuthCookie(user.Username,false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }
    }
}