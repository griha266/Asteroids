using System;
using System.Collections.Generic;

namespace Asteroids.Core.Models
{
    public static class ModelsCollectionUtils
    {
        public static IEnumerable<Type> GetModelsTypes(this Type modelType)
        {
            var currentType = modelType;
            while (currentType != null)
            {
                yield return currentType;
                var baseType = currentType.BaseType;
                if (baseType == typeof(object))
                {
                    break;
                }
                currentType = baseType;
            }
            
            var interfaces = modelType.GetInterfaces();
            for (var i = 0; i < interfaces.Length; i++)
            {
                yield return interfaces[i];
            }
            
        }
    }
}