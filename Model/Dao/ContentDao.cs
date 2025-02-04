﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.FrameWork;
using PagedList;
using Common;

namespace Model.Dao
{
    public class ContentDao
    {
        WebBanHangDbContext db = null;
        public ContentDao()
        {
            db = new WebBanHangDbContext();
        }

        //updatae tin tức lên trang client
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)//Sử lí phân trang
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)//Sử lí phân trang
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        //lấy danh sách tin tức cho các tag
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContenTexts
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTile,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreateDate,
                             CreatedBy = a.CreateBy,
                             ID = a.ID

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTile = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateDate = x.CreatedDate,
                             CreateBy = x.CreatedBy,
                             ID = x.ID
                         });
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
        public long Create(Content content)
        {
            //Alias
            if (string.IsNullOrEmpty(content.MetaTile))
            {
                content.MetaTile = StringHelper.ToUnsignString(content.Name);
            }
            content.CreateDate = DateTime.Now;
            db.Contents.Add(content);
            db.SaveChanges();
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //chèn vào bảng tag
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }
                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);

                }
            }
            return content.ID;
        }
        public long Edit(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTile))
            {
                content.MetaTile = StringHelper.ToUnsignString(content.Name);
            }
            content.CreateDate = DateTime.Now;
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);

                }
            }

            return content.ID;
        }
        public void RemoveAllContentTag(long contentId)
        {
            db.ContenTexts.RemoveRange(db.ContenTexts.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContenText();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContenTexts.Add(contentTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContenTexts
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
    }
}
