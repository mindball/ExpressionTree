using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace ExampleClasses
{
    public class PropertyHelper
    {
        private static readonly Type TypeOfObject = typeof(object);

        private static readonly ConcurrentDictionary<Type, PropertyHelper[]> PropertiesCache =
            new ConcurrentDictionary<Type, PropertyHelper[]>();

        public string Name { get; set; }

        public Func<object, object> Getter { get; set; }

        public static PropertyHelper[] Get<T>(T obj)
        {
            // стремим се да направим T obj => obj.Property
            var type = obj.GetType();

            return PropertiesCache.GetOrAdd(type, _ =>
            {
                return type
                .GetProperties()
                .Select(pr =>
                {
                    //object obj
                    var parameter = Expression.Parameter(TypeOfObject, "obj");

                    //(T)obj
                    var convertedParameter = Expression.Convert(parameter, type);

                    //(T)obj.Property
                    var propertyGetter = Expression.MakeMemberAccess(convertedParameter, pr);

                    // (object)((T)obj.Property)
                    var convertedPropertyObject = Expression.Convert(propertyGetter, TypeOfObject);

                    var lambda = Expression.Lambda<Func<object, object>>(convertedPropertyObject, parameter);
                    return new PropertyHelper
                    {
                        Name = pr.Name,
                        Getter = lambda.Compile()
                    };
                })
                .ToArray();
            });             
        }
    }
}
