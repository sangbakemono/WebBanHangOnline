using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;
using Model.ViewModel;
using PagedList;

namespace Model.Dao
{
    public class ProductDao
    {
        WebBanHangDbContext db = null;
        public ProductDao()
        {
            db = new WebBanHangDbContext();
        }
        public List<SanPham> ListNewProduct(int top)
        {
            return db.SanPhams.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<string> ListName(string keyword)
        {
            return db.SanPhams.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        /// <summary>
        /// Get list product by category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<SanPham> ListByCategoryId(long categoryID)
        {
            return db.SanPhams.Where(x => x.CategoryID == categoryID).ToList();
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.SanPhams.Where(x => x.Name == keyword).Count();
            var model = (from a in db.SanPhams
                         join b in db.SanPhamCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTile,
                             CateName = b.Name,
                             CreatedDate = a.CreateDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTile,
                             Price = a.Price
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public List<SanPham> ListFeatureProduct(int top)
        {
            return db.SanPhams.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<SanPham> ListRelatedProducts(long productId)
        {
            var product = db.SanPhams.Find(productId);
            return db.SanPhams.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        public SanPham ViewDetail(long id)
        {
            return db.SanPhams.Find(id);
        }

        public IEnumerable<SanPham> ListAllPaging(string searchString, int page, int pageSize)//Sử lí phân trang
        {
            IQueryable<SanPham> model = db.SanPhams;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
    }
}
