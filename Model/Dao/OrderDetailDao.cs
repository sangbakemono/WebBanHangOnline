using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        WebBanHangDbContext db = null;
        public OrderDetailDao()
        {
            db = new WebBanHangDbContext();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
