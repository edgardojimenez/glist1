using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Groceries.Entities;

namespace Groceries.Controllers {
    public class HomeController : Controller {
        private readonly DataContext _data = new DataContext("name=GroceriesConnection");

        public ActionResult Index() {
            IsAjax();
            ViewBag.Id = "MPIndex";
            ViewBag.Header = "Groceries List";

            return View(GetGroceries());
        }

        public ActionResult RemoveListItem(int id) {
            IsAjax();
            ViewBag.Id = "MPIndex";
            ViewBag.Header = "Groceries List";

            var itemToRemove = _data.Groceries.SingleOrDefault(g => g.Id == id);
            if (itemToRemove != null) {
                _data.Groceries.Remove(itemToRemove);
                _data.SaveChanges();
            }
            
            return View("index", GetGroceries());
        }

        public ActionResult Item() {
            ViewBag.Id     = "MPItem";
            ViewBag.Header = "Products";

            var dividerProducts = GetDividerProducts();

            return View(dividerProducts);
        }

        public ActionResult AddItem(int id) {
            IsAjax();
            ViewBag.Id = "MPItem";
            ViewBag.Header = "Products";

            if (_data.Groceries.FirstOrDefault(p => p.ProductId == id) == null) {
                _data.Groceries.Add(new Entities.Groceries() { DateCreated = DateTime.Now, ProductId = id });
                _data.SaveChanges();
            }

            var dividerProducts = GetDividerProducts();

            return View("Item", dividerProducts);
        }

        public ActionResult RemoveItem(int id) {
            IsAjax();
            ViewBag.Id = "MPItem";
            ViewBag.Header = "Products";

            var product = _data.Product.FirstOrDefault(p => p.Id == id);
            if (product != null) {
                _data.Product.Remove(product);
                _data.SaveChanges();
            }

            var dividerProducts = GetDividerProducts();

            return View("Item", dividerProducts);
        }

        public ActionResult ClearList() {
            IsAjax();

            ViewBag.Id = "MPIndex";
            ViewBag.Header = "Groceries List";

            var list = _data.Groceries.ToList();
            foreach (var item in list) {
                _data.Groceries.Remove(item);
            }
            _data.SaveChanges();

            return View("Index", new List<Entities.Groceries>());
        }

        public ActionResult AddProduct() {
            ViewBag.Id = "MPAddProduct";
            ViewBag.Header = "Add Products";
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(string productName) {
            ViewBag.Id = "MPAddProduct";
            ViewBag.Header = "Add Products";

            if (!string.IsNullOrWhiteSpace(productName)) {
                var currentProduct = _data.Product.FirstOrDefault(p => p.Name.ToLower() == productName.ToUpper());
                if (currentProduct != null) {
                    ViewBag.Message = string.Format("Product '{0}' already exists.", productName);
                } else {
                    _data.Product.Add(new Product() {Name = productName});
                    _data.SaveChanges();
                    ViewBag.Message = string.Format("Added Product '{0}'.", productName);
                }
            } else {
                ViewBag.Message = "No Product Entered!";
            }

            return View();
        }

        private void IsAjax() {
            if (Request.IsAjaxRequest()) {
                Debug.WriteLine("Ajax Request YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
            } else {
                Debug.WriteLine("Ajax Request NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
            }
        }

        private List<Product> GetDividerProducts() {
            List<Product> selectedProducts = null;
            if (_data.Groceries.Count() == 0) {
                selectedProducts = _data.Product.ToList();
            } else {

                var groceries = _data.Groceries.Select(p => p.ProductId).ToArray();
                var products = _data.Product.Select(p => p.Id).ToArray();
                var unselected = products.Except(groceries);

                selectedProducts = _data.Product.Where(p => unselected.Contains(p.Id)).ToList();
            }

            var alphabet = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var dividerProducts = new List<Product>();
            var orderProductList = selectedProducts.OrderBy(o => o.Name).ToList();
            foreach (var product in orderProductList) {
                var letter = product.Name.Substring(0, 1).ToUpper();
                if (alphabet.Contains(letter)) {
                    dividerProducts.Add(new Product() { Id = -1, Name = letter });
                    alphabet.Remove(letter);
                }
                dividerProducts.Add(product);
            }
            return dividerProducts;
        }

        private List<Entities.Groceries> GetGroceries() {
            return _data.Groceries.OrderBy(o => o.Product.Name).ToList();
        }
    }
}
