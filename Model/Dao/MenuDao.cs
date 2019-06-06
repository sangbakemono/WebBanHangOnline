using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;

namespace Model.Dao
{
    
    public class MenuDao
    {
        WebBanHangDbContext db = null;
        public MenuDao()
        {
            db = new WebBanHangDbContext();
        }

        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId && x.Status == true).OrderBy(x => x.DisplayOder).ToList();
        }    
    }
}
