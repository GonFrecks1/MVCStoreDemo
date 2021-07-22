using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppECartDemo.Models;
using WebAppECartDemo.ViewModel;

namespace WebAppECartDemo.Controllers
{
    public class ShoppingController : Controller
    {
        private ECartDBEntities objCartDBEntities;
        private List<ShoppingCartModel> lstShoppingCartModel;

        public ShoppingController()
        {
            objCartDBEntities = new ECartDBEntities();
            lstShoppingCartModel = new List<ShoppingCartModel>();
        }

        // GET: Shopping
        public ActionResult Index()
        {

            IEnumerable<ShoppingViewModel> lstShoppingViewModels = (from item in objCartDBEntities.Items
                                                                    join
                                                                    Cate in objCartDBEntities.Categories
                                                                    on item.CategoryId equals Cate.CategoryId

                                                                    select new ShoppingViewModel()
                                                                    {
                                                                        ImagePath = item.ImagePath,
                                                                        ItemName = item.ItemName,
                                                                        Description = item.Description,
                                                                        ItemPrice = item.ItemPrice,
                                                                        ItemId = item.ItemId,
                                                                        CategoryName = Cate.CategoryName,
                                                                        ItemCode = item.ItemCode
                                                                    }
                                                                  ).ToList();

            return View(lstShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(string ItemId)
        {


            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Items objItem = objCartDBEntities.Items.Single(model => model.ItemId.ToString() == ItemId);
            if (Session["CartCounter"] != null)
            {
                lstShoppingCartModel = Session["CartItem"] as List<ShoppingCartModel>;
            }
            if (lstShoppingCartModel.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = lstShoppingCartModel.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                lstShoppingCartModel.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = lstShoppingCartModel.Count;
            Session["CartItem"] = lstShoppingCartModel;

            return Json(new { Success = true, Counter = lstShoppingCartModel.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            lstShoppingCartModel = Session["CartItem"] as List<ShoppingCartModel>;
            return View(lstShoppingCartModel);
        }

        [HttpPost]
        public ActionResult AddOrder(string ItemId)
        {
            int OrderId = 0;
            lstShoppingCartModel = Session["CartItem"] as List<ShoppingCartModel>;
            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}", DateTime.Now)
            };

            objCartDBEntities.Orders.Add(order);
            objCartDBEntities.SaveChanges();
            OrderId = order.OrderId;

            foreach (var item in lstShoppingCartModel)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Total = item.Total;
                orderDetail.ItemId = item.ItemId;
                orderDetail.OrderId = OrderId;
                orderDetail.Quantity = item.Quantity;
                orderDetail.UnitPrice = item.UnitPrice;
                objCartDBEntities.OrderDetails.Add(orderDetail);
                objCartDBEntities.SaveChanges();

            }
            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
        }
    }
}