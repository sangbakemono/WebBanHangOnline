using Model.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model.Dao
{
    public class CategoryDao
    {
        WebBanHangDbContext db = null;
        public CategoryDao()
        {
            db = new WebBanHangDbContext();
        }

        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }

        public SanPhamCategory ViewDetail(long id)
        {
            return db.SanPhamCategories.Find(id);
        }
    }
}
