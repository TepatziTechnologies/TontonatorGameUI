using System;
using System.Collections;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Tontonator.Models;

namespace Tontonator.Core.Data.BaseRepository
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        protected FirestoreDb _firestoreDb;
        protected string collection;
        private string filepath = "tontonatoruaq-firebase-adminsdk-oww74-029b8e3492.json";

        public EntityBaseRepository(string collectionName)
        {
            this.collection = collectionName;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            _firestoreDb = FirestoreDb.Create("tontonatoruaq");
        }

        public T Add(T entity)
        {
            if (!Tontonator.Instance.DATABASE_OFF) { 
                DocumentReference document = _firestoreDb.Collection(this.collection).Document();
                entity.Id = document.Id;
                var result = document.SetAsync(entity.ToDictionary()).GetAwaiter().GetResult();
            }

            return entity;
        }

        public Task Delete(T entity)
        {
            // Need to check for this.
            throw new NotImplementedException();
        }

        public T Read(string field, string queryValue)
        {
            List<T> values = new List<T>();
            T? entity = new T();

            CollectionReference parentCollection = _firestoreDb.Collection(this.collection);
            Query query = parentCollection.WhereEqualTo(field, queryValue);

            foreach (var data in query.GetSnapshotAsync().GetAwaiter().GetResult())
            {
                if (data.Exists)
                {
                    var dictionary = data.ToDictionary();
                    var jsonString = JsonConvert.SerializeObject(dictionary);
                    entity = JsonConvert.DeserializeObject<T>(jsonString);
                }
            }

            return entity;
        }

        public List<T> ReadAll(string field, object queryValue)
        {
            //

            List<T> values = new List<T>();
            T? entity = new T();

            CollectionReference parentCollection = _firestoreDb.Collection(this.collection);
            Query query = parentCollection.WhereEqualTo(field, queryValue);

            foreach (var data in query.GetSnapshotAsync().GetAwaiter().GetResult())
            {
                if (data.Exists)
                {
                    var dictionary = data.ToDictionary();
                    var jsonString = JsonConvert.SerializeObject(dictionary);
                    entity = JsonConvert.DeserializeObject<T>(jsonString);
                    if (entity != null) values.Add(entity);
                }
            }

            return values;
        }

        public Task Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}

