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
        public ShoppingController()
        {
            objCartDBEntities = new ECartDBEntities();
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
    }
}