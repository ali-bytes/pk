using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;


namespace Domain.EStore.Repositories
{
    public interface IRepository<T>
          where T : class
    {
        IQueryable<T> FindAll();


        IQueryable<T> Find(Expression<Func<T, bool>> predicate);


        T FindById(int id);


        T FindFirstOrDefault(int id);
        T First();

        bool Add(T newEntity);

        void Flush();

        bool Remove(T entity);

        DbEntityValidationResult IsValid(T newEntity);
        bool Remove(int entityId);

        bool SaveChange();

        bool RemoveAll(IQueryable<T> entities);
    }


    public class Repository<T> : IRepository<T> where T : class,IEntity
    {
        private readonly string _cacheName;
        readonly Cache _cacheService = new Cache();
        DbContext _context;
        protected DbSet<T> Objectset;
        const string ConnectionStringName = "EFContext";


        string connectionString = ConfigurationManager
                          .ConnectionStrings[ConnectionStringName]
                          .ConnectionString;
        public Repository(string cacheName)
        {
            connectionString = ConfigurationManager
                         .ConnectionStrings[ConnectionStringName]
                         .ConnectionString;
            _cacheName = cacheName;

            //_context = new DbContext(connectionString);
            //_context.Configuration.LazyLoadingEnabled = false;
            //Objectset = _context.Set<T>();


        }



        public IQueryable<T> FindAll()
        {
            var items = new List<T>();
            items = _cacheService.Get(_cacheName) as List<T>;
            if (items == null)
            {
                DbSet<T> objectset1;
                using (var context = new DbContext(connectionString))
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    objectset1 = context.Set<T>();
                    Objectset = objectset1;
                    items = objectset1.ToList();
                }

                Objectset = objectset1;


                _cacheService.Set(_cacheName, items);
            }
            return items.AsQueryable();

        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }

        public T FindById(int id)
        {
            return FindAll().ToList().Single(o => o.Id == id);
        }

        public T FindFirstOrDefault(int id)
        {
            return FindAll().FirstOrDefault(o => o.Id == id);
        }

        public T First()
        {
            return FindAll().FirstOrDefault();
        }



        public bool Add(T newEntity)
        {
            bool isSaved = false;

            using (var context = new DbContext(connectionString))
            {

                DbEntityValidationResult validationResult = context.Entry(newEntity).GetValidationResult();

                if (validationResult.IsValid)
                {

                    if (newEntity.Id == 0)
                    {
                        context.Set<T>().Add(newEntity);
                    }
                    else
                    {
                        // var attachedEntry = context.Entry(FindById(newEntity.Id));
                        //  attachedEntry.CurrentValues.SetValues(newEntity);
                        context.Entry(newEntity).State = EntityState.Modified;
                    }
                    if (context.SaveChanges() > 0)
                    {
                        _cacheService.Delete(_cacheName);
                        isSaved = true;
                    }
                }
               


            }

            return isSaved;

        }

        public void Flush()
        {

            _cacheService.Flush();


        }

        public bool SaveChange()
        {
            if (_context.SaveChanges() > 0)
            {
                _cacheService.Flush();
                return true;
            }
            return false;
        }

        public bool Remove(T entity)
        {
            bool isDeleted = false;
            using (var context = new DbContext(connectionString))
            {
                var dentity = FindFirstOrDefault(entity.Id);
                if (dentity != null)
                {





                    context.Entry(entity).State = EntityState.Deleted;

                    if (context.SaveChanges() > 0)
                    {
                        _cacheService.Delete(_cacheName);
                        isDeleted = true;
                    }
                }
            }
            return isDeleted;
        }

        public bool Remove(int entityId)
        {
            bool isDeleted = false;
            using (var context = new DbContext(connectionString))
            {
                var entity = FindFirstOrDefault(entityId);
                if (entity != null)
                {

                    context.Entry(entity).State = EntityState.Deleted;

                    if (context.SaveChanges() > 0)
                    {
                        _cacheService.Delete(_cacheName);
                        isDeleted = true;
                    }
                }
            }
            return isDeleted;
        }

        public bool RemoveAll(IQueryable<T> entities)
        {
            bool isDeleted = false;
            using (var context = new DbContext(connectionString))
            {
                foreach (var entity in entities)
                {
                    var dentity = FindById(entity.Id);
                    if (dentity != null)
                    {
                        context.Entry(entity).State = EntityState.Deleted;

                        if (context.SaveChanges() > 0)
                        {
                            _cacheService.Delete(_cacheName);
                            isDeleted = true;
                        }
                    }

                }
            }
            return isDeleted;
        }

        
        public DbEntityValidationResult IsValid(T newEntity)
        {
            DbEntityValidationResult validationResult;
                using (var context = new DbContext(connectionString))
                {

                     validationResult = context.Entry(newEntity).GetValidationResult();

                  
                    
                }
            return validationResult;

        }
    }
}