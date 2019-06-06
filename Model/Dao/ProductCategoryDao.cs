using Model.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model.Dao
{
    public class ProductCategoryDao
    {
        WebBanHangDbContext db = null;
        public ProductCategoryDao()
        {
            db = new WebBanHangDbContext();
        }
        public List<SanPhamCategory> ListAll()
        {
            return db.SanPhamCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public SanPhamCategory ViewDetail(long id)
        {
            return db.SanPhamCategories.Find(id);
        }
    }
}
