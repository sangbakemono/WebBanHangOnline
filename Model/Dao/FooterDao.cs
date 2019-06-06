using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;

namespace Model.Dao
{
    public class FooterDao
    {
        WebBanHangDbContext db = null;
        public FooterDao()
        {
            db = new WebBanHangDbContext();
        }
        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true);
        }
    }
}
