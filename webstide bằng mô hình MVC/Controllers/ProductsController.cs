using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webstide_bằng_mô_hình_MVC.Models;

namespace webstide_bằng_mô_hình_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private My_StoreEntities2 db = new My_StoreEntities2();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
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

        // GET: Products/Edit/5
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
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


        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Products
        public ActionResult ProductList(int? categoryId, string searchString, decimal? min, decimal? max)
        {
            // Lấy danh sách các loại sản phẩm từ cơ sở dữ liệu
            ViewBag.Categories = db.Categories.ToList();

            // Lấy danh sách sản phẩm
            var products = db.Products.AsQueryable();

            // Lọc sản phẩm theo loại
            if (categoryId.HasValue && categoryId > 0)
            {
                products = products.Where(p => p.CategoryID == categoryId);
            }

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
            }

            // Lọc sản phẩm theo khoảng giá
            if (min.HasValue && max.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= min && p.ProductPrice <= max);
            }

            return View(products.ToList());
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
