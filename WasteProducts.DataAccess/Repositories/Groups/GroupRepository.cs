﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;

namespace WasteProducts.DataAccess.Repositories.Groups
{
    class GroupRepository : IGroupRepository<GroupDB>
    {
        WasteContext db;

        public GroupRepository(WasteContext context)
        {
            db = context;
        }

        public void Create(GroupDB item)
        {
            db.GroupDBs.Add(item);
        }

        public void Update(GroupDB item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            GroupDB group = db.GroupDBs.Find(id);
            if (group != null)
                db.GroupDBs.Remove(group);
        }

        public IEnumerable<GroupDB> Find(Func<GroupDB, bool> predicate)
        {
            return db.GroupDBs.Where(predicate).ToList();
        }

        public GroupDB Get(int id)
        {
            return db.GroupDBs.Find(id);
        }

        public IEnumerable<GroupDB> GetAll()
        {
            return db.GroupDBs;
        }

    }
}
