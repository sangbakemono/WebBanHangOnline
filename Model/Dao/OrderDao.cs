using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;

namespace Model.Dao
{
    public class OrderDao
    {
        WebBanHangDbContext db = null;
        public OrderDao()
        {
            db = new WebBanHangDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
    }
}
