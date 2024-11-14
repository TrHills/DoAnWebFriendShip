using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webstide_bằng_mô_hình_MVC.Models;

namespace webstide_bằng_mô_hình_MVC.Areas.ADMIN.Controllers
{
    public class SanPhamController : Controller
    {
        #region THONGTIN
        private My_StoreEntities2 db = new My_StoreEntities2();

        // GET: ADMIN/SanPham
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: ADMIN/SanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ADMIN/SanPham/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: ADMIN/SanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductDecription,ProductPrice")] Product product, HttpPostedFileBase ProductImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ProductImageFile != null && ProductImageFile.ContentLength > 0)
                {
                    // Lưu đường dẫn ảnh
                    string fileName = System.IO.Path.GetFileName(ProductImageFile.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    ProductImageFile.SaveAs(path);

                    product.ProductImage = "/Content/Images/" + fileName;
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: ADMIN/SanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: ADMIN/SanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductDecription,ProductPrice")] Product product, HttpPostedFileBase ProductImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ProductImageFile != null && ProductImageFile.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(ProductImageFile.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    ProductImageFile.SaveAs(path);

                    product.ProductImage = "/Content/Images/" + fileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: ADMIN/SanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ADMIN/SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //override ghi đè
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region HINHANH
        public ActionResult TaiHinh()
        {
            return View();
        }

        // POST: ADMIN/SanPham/TaiHinh
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaiHinh(HttpPostedFileBase file, string moTa)
        {
            if (file != null && file.ContentLength > 0)
            {
                // Lưu file vào thư mục trên server
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                file.SaveAs(path);

                // Tạo đối tượng Product mới
                var product = new Product
                {
                    ProductName = moTa,
                    ProductDecription = moTa,
                    ProductImage = "/Content/Images/" + fileName // Lưu đường dẫn ảnh
                };

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Vui lòng chọn một tệp ảnh hợp lệ.";
            return View();
        }


        // GET: ADMIN/SanPham/GetImage
        public ActionResult GetImage(int id)
        {
            var product = db.Products.Find(id);
            if (product != null && product.ProductImage != null)
            {
                return File(product.ProductImage, "image/jpeg"); // hoặc image/png
            }
            return HttpNotFound();
        }
    }
    #endregion

}

