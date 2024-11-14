using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webstide_bằng_mô_hình_MVC.Models;

namespace webstide_bằng_mô_hình_MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        My_StoreEntities2 db=new My_StoreEntities2();
        
        //chuan bi data cho view
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("ShowCart");
            Cart _cart = Session["Cart"]as Cart;
            return View(_cart);
        }

        // tao moi gio hang nguon lay su Session
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"]=cart;
            }
            return cart;
        }

        //them san pham vao gio hang
        public ActionResult AddToCart(int id)
        {
            var _pro = db.Products.SingleOrDefault(s=>s.ProductID==id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        //cap nhat so luong, tinh tong tien
        // cap nhat so luong, tinh tong tien
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;

            if (cart == null)
            {
                return RedirectToAction("ShowCart", "ShoppingCart");
            }

            if (!string.IsNullOrEmpty(form["idPro"]) && !string.IsNullOrEmpty(form["carQuantity"]))
            {
                int id_pro;
                int _quantity;

                if (int.TryParse(form["idPro"], out id_pro) && int.TryParse(form["carQuantity"], out _quantity))
                {
                    cart.Update_quantity(id_pro, _quantity);
                }
                else
                {
                    // Handle parse error
                    return Content("Invalid input!");
                }
            }
            else
            {
                // Handle missing form values
                return Content("Missing form values!");
            }

            return RedirectToAction("ShowCart", "ShoppingCart");
        }


        //xoa san pham trong gio hang
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        //tinh tong tien don hang
        public PartialViewResult BagCart()
        {
            decimal total_money_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_money_item = cart.Total_money();
            ViewBag.totalCart = total_money_item;
            return PartialView("BagCart");
        }

        //phuong thuc cho thanh toán
public ActionResult CheckOut(FormCollection form)
{
    try
    {
        // Kiểm tra nếu giỏ hàng rỗng
        Cart cart = Session["Cart"] as Cart;
        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        // Tạo đơn hàng mới
        Order newOrder = new Order
        {
            OrderDate = DateTime.Now,
            AddressDelivery = form["AddressDelivery"]
        };

        // Kiểm tra và lấy thông tin khách hàng từ mã ID
        int customerID;
        if (!int.TryParse(form["CodeCustomer"], out customerID) || db.Customers.Find(customerID) == null)
        {
            return Content("Mã khách hàng không hợp lệ.");
        }
        newOrder.CustomerID = customerID;

        // Thêm đơn hàng vào cơ sở dữ liệu
        db.Orders.Add(newOrder);
        db.SaveChanges();

        // Tạo và thêm các chi tiết đơn hàng cho từng sản phẩm trong giỏ hàng
        foreach (var item in cart.Items)
        {
            OrderDetail orderDetail = new OrderDetail
            {
                OrderID = newOrder.OrderID,
                ProductID = item._product.ProductID,
                UnitPrice = item._product.ProductPrice,
                Quantity = item._quantity
            };
            db.OrderDetails.Add(orderDetail);
        }

        // Lưu các chi tiết đơn hàng vào cơ sở dữ liệu
        db.SaveChanges();

        // Xóa giỏ hàng sau khi thanh toán thành công
        cart.ClearCart();

        // Chuyển hướng tới trang thông báo thành công
        return RedirectToAction("CheckOut_Success", "ShoppingCart");
    }
    catch (Exception ex)
    {
        // Log lỗi nếu cần và trả về thông báo lỗi
        // Bạn có thể ghi log hoặc xử lý lỗi tùy thuộc vào yêu cầu cụ thể
        return Content("Có sai sót! Xin kiểm tra lại thông tin: " + ex.Message);
    }
}


        //thong bao thanh toan thanh cong
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}