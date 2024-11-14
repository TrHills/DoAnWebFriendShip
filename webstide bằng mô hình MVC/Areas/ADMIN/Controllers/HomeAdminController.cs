using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webstide_bằng_mô_hình_MVC.Areas.ADMIN.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: ADMIN/HomeAdmin
        public ActionResult Index()
        {
            return View();
        }
        //trang view
        public ActionResult DangNhap()
        {
            return View();
        }
    }
}