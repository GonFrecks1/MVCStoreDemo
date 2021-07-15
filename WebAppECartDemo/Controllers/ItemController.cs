using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppECartDemo.Models;
using WebAppECartDemo.ViewModel;

namespace WebAppECartDemo.Controllers
{
    public class ItemController : Controller
    {
        private ECartDBEntities eCartDBEntities;
        public ItemController()
        {
            eCartDBEntities = new ECartDBEntities();
        }
        // GET: Item
        public ActionResult Index()
        {
            ItemViewModel itemViewModel = new ItemViewModel();
            itemViewModel.CategorySelectListItems = (from m in eCartDBEntities.Categories
                                                     select new SelectListItem()
                                                     {
                                                         Text = m.CategoryName,
                                                         Value = m.CategoryId.ToString(),
                                                         Selected = true
                                                     });
            return View(itemViewModel);
        }

        [HttpPost]
        public JsonResult Index(ItemViewModel itemViewModel)
        {
            
            string NewImage = Guid.NewGuid() + Path.GetExtension(itemViewModel.ImagePath.FileName);
            itemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));

            Items items = new Items();
            items.ImagePath = "~/Images/" + NewImage;
            items.CategoryId = itemViewModel.CategoryId;
            items.Description = itemViewModel.Description;
            items.ItemCode = itemViewModel.ItemCode;
            items.ItemId = Guid.NewGuid();
            items.ItemName = itemViewModel.ItemName;
            items.ItemPrice = itemViewModel.ItemPrice;

            eCartDBEntities.Items.Add(items);
            eCartDBEntities.SaveChanges();

            return Json(new { Success = true, Message = "Items is added Successfully."}, JsonRequestBehavior.AllowGet);
        }
    }
}