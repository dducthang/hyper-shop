﻿
using HyperShop.DataAccess.Data;
using HyperShop.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (prop == "ProductVariation")
                    {
                        var prop2 = "ProductVariation.Size";
                        var prop3 = "ProductVariation.Color";
                        var prop4 = "ProductVariation.Product";
                        query = query.Include(prop2).Include(prop3).Include(prop4);
                    }
                    else
                    {
                        query = query.Include(prop);
                    }
                }
            }
            
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (prop == "ProductVariation")
                    {
                        var prop2 = "ProductVariation.Size";
                        var prop3 = "ProductVariation.Color";
                        var prop4 = "ProductVariation.Product";
                        query = query.Include(prop2).Include(prop3).Include(prop4);
                    }
                    else
                    {
                        query = query.Include(prop);
                    }
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T item)
        {
            dbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items)
;        }
    }
}
