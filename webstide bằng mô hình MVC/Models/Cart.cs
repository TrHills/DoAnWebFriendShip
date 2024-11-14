using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webstide_bằng_mô_hình_MVC.Models
{
    public class CartItem
    {
        public Product _product { get; set; }
        public int _quantity { get; set; }
    }
    public class Cart
    {
        //dung cau truc list de luu tru Item nhu mot bang tam
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items { get { return items; } }


        //phuong thuc lay san pham bo vao gio hang
        //tham so la product(_pro) san pham va so luong(_qua)
        public void Add_Product_Cart(Product _pro, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s =>s._product.ProductID == _pro.ProductID);
            if (item == null)
                items.Add(new CartItem { _product = _pro, _quantity = _quan });
            else item._quantity += _quan;
        }

        //tinh tong so luong trong gio hang
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }

        //ham tinh tien cho moi san pham trong gio hang
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * s._product.ProductPrice);
            return(decimal)total;
        }

        //cap nhat so luong san pham cua khach
        public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s=>s._product.ProductID == id);
            if (item != null)
                item._quantity = _new_quan;
        }

        //xoa san pham trong gio hang
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s=>s._product.ProductID==id);
        }

        // xoa gio hang sau khi khach da thanh toan
        public void ClearCart()
        {
            items.Clear();
        }
    }
}