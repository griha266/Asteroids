using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Utils
{
    public class GameObjectPool : IDisposable
    {
        private readonly UnityEngine.Transform _container;
        private readonly GameObject _prefab;
        private readonly List<GameObject> _instances;
        
        public GameObjectPool(GameObject prefab, int preloadPrefabsCount = 0, string name = null)
        {
            _prefab = prefab;
            if (string.IsNullOrEmpty(name))
            {
                name = prefab.name;
            }

            var containerGameObject = new GameObject($"{name}Pool");
            _container = containerGameObject.transform;
            _instances = new List<GameObject>();
            for (int i = 0; i < preloadPrefabsCount; i++)
            {
                var instance = Object.Instantiate(prefab, _container);
                instance.SetActive(false);
                _instances.Add(instance);
            }
        }

        public GameObject Get(UnityEngine.Transform parent = null, bool isActiveOnReceiving = true)
        {
            var result = _instances.FirstOrDefault();
            if (result)
            {
                _instances.Remove(result);
            }
            else
            {
                result = Object.Instantiate(_prefab);
            }

            if (parent)
            {
                result.transform.SetParent(parent);
            }
            result.SetActive(isActiveOnReceiving);
            return result;
        }

        public void Release(GameObject instance)
        {
            instance.SetActive(false);
            instance.transform.SetParent(_container);
            _instances.Add(instance);
        }
        
        
        public void Dispose()
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                Object.Destroy(_instances[i]);
            }
            _instances.Clear();
            Object.Destroy(_container.gameObject);
        }
    }
}