using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppECartDemo.ViewModel
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public DateTime Orderdate { get; set; }
        public string OrderNumber { get; set; }
    }
}