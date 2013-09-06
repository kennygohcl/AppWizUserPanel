using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;


namespace WorkerRole.EmailNotification.Base
{
     public class BaseServiceContext<T> : TableServiceContext where T : TableServiceEntity
    {
        protected virtual string EntityName { get; private set; }

        public BaseServiceContext(string baseAddress, StorageCredentials credentials, string entityName = null)
            : base(baseAddress, credentials)
        {
            IgnoreResourceNotFoundException = true;
            IgnoreMissingProperties = true;
            EntityName = entityName;

            // infer the name from type
            if (string.IsNullOrEmpty(EntityName))
            {
                EntityName = typeof (T).Name + "Entity";
            }
            
            // hack
            new CloudTableClient(baseAddress, credentials).CreateTableIfNotExist(EntityName);   
        }


        public IQueryable<T> QuerableEntities
        {
            get
            {
                return CreateQuery<T>(EntityName);
            }
        }

        public T GetEntity(string partitionKey, string rowKey)
        {
            var T = (from e in CreateQuery<T>(EntityName)
                               where e.RowKey == rowKey && e.PartitionKey == partitionKey
                               select e).FirstOrDefault();

            return T;
        }

        public IQueryable<T> GetQueryableByPartitionKey(string partitionKey)
        {
            var T = (from e in CreateQuery<T>(EntityName)
                     where e.PartitionKey == partitionKey
                     select e);

            return T;
        }

        public void Add(T T)
        {
            AddObject(EntityName, T);
            SaveChanges();
        }

        public void Delete(T T)
        {
            DeleteObject(T);
            SaveChanges();
        }

        public void Update(T T)
        {
            UpdateObject(T);
            SaveChanges();
        }
    }
}

