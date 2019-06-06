using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;

namespace Model.Dao
{
   public class SlideDao
    {
        WebBanHangDbContext db = null;
        public SlideDao()
        {
            db = new WebBanHangDbContext();
        }
        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOder).ToList();
        }
    }

}
