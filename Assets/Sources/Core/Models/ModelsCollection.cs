using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Core.Systems;

namespace Asteroids.Core.Models
{
    public interface IModelsCollection : IUpdateable
    {
        void Add(object model);
        void Remove(object model);
    }
    
    public class ModelsCollection<TModel> : IModelsCollection
        where TModel : class
    {
        private readonly List<TModel> _models = new();
        private readonly List<TModel> _modelsForRemoval = new();
        private readonly List<TModel> _modelsToAdd = new();

        public IReadOnlyList<TModel> Models => _models;
        
        public void Add(TModel model)
        {
            _models.Add(model);
        }

        public void Add(object model)
        {
            var modelTypes = model.GetType().GetModelsTypes();
            if (!modelTypes.Contains(typeof(TModel)))
            {
                throw new Exception($"Cannot add {model.GetType().FullName} to {typeof(TModel).FullName} collection");
            }
            
            _modelsToAdd.Add(model as TModel);
        }

        public void Remove(object model)
        {
            var modelTypes = model.GetType().GetModelsTypes();
            if (!modelTypes.Contains(typeof(TModel)))
            {
                throw new Exception($"Cannot remove {model.GetType().FullName} to {typeof(TModel).FullName} collection");
            }
            _modelsForRemoval.Add(model as TModel);
        }

        // We need to remove models in the very end of loop
        public void Remove(TModel model)
        {
            _modelsForRemoval.Add(model);
        }

        public void Update(float deltaTime)
        {
            foreach (var model in _modelsToAdd)
            {
                _models.Add(model);
            }
            _modelsToAdd.Clear();
            foreach (var model in _modelsForRemoval)
            {
                _models.Remove(model);
            }
            
            _modelsForRemoval.Clear();
        }
    }
}