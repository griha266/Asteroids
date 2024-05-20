using System;
using System.Collections.Generic;

namespace Asteroids.Core.Models
{
    public class ModelsCollections : IModelsCollection
    {
        private readonly Dictionary<Type, IModelsCollection> _collections = new();

        public IReadOnlyList<TModel> GetModels<TModel>()
            where TModel : class
        {
            var modelCollection = GetModelsCollection<TModel>();
            return modelCollection.Models;
        }

        public void Add(object model)
        {
            var type = model.GetType();
            if (type.IsValueType)
            {
                throw new Exception("Cannot register value type in collection");
            }
            foreach (var modelType in type.GetModelsTypes())
            {
                var collection = GetModelsCollection(modelType);
                collection.Add(model);
            }
        }

        public void Remove(object model)
        {
            var type = model.GetType();
            if (type.IsValueType)
            {
                throw new Exception("Cannot register value type in collection");
            }
            
            foreach (var modelType in type.GetModelsTypes())
            {
                var collection = GetModelsCollection(modelType);
                collection.Remove(model);
            }
        }

        private IModelsCollection GetModelsCollection(Type type)
        {
            if (!_collections.ContainsKey(type))
            {
                var genericType = typeof(ModelsCollection<>).MakeGenericType(type);
                var collection = (IModelsCollection)Activator.CreateInstance(genericType);
                _collections.Add(type, collection);
                return collection;
            }

            return _collections[type];
        }
        
        private ModelsCollection<TModel> GetModelsCollection<TModel>()
            where TModel : class
        {
            var type = typeof(TModel);
            if (!_collections.ContainsKey(type))
            {
                var collection = new ModelsCollection<TModel>();
                _collections.Add(type, collection);
                return collection;
            }
            
            return _collections[type] as ModelsCollection<TModel>;
        }
        

        public void Update(float deltaTime)
        {
            foreach (var collection in _collections.Values)
            {
                collection.Update(deltaTime);
            }
        }
    }
}