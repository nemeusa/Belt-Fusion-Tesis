using System;
using System.Collections.Generic;

namespace PopupFramework.Core
{
    /// <summary>
    /// Service locator mínimo: registra una implementación detrás de una
    /// interfaz y la resuelve desde cualquier lado sin conocer quién la creó
    /// (DIP). Se registra una sola vez, normalmente desde una escena de
    /// bootstrap que carga antes que el resto la necesite.
    /// </summary>
    public sealed class ServiceLocator
    {
        public static ServiceLocator Instance => _instance ??= new ServiceLocator();
        static ServiceLocator _instance;

        readonly Dictionary<Type, object> _dependencies = new();

        public void RegisterDependency<T>(T dependency)
        {
            _dependencies[typeof(T)] = dependency;
        }

        public T GetDependency<T>()
        {
            return _dependencies.TryGetValue(typeof(T), out var value) ? (T)value : default;
        }

        public bool TryGetDependency<T>(out T dependency)
        {
            if (_dependencies.TryGetValue(typeof(T), out var value))
            {
                dependency = (T)value;
                return true;
            }

            dependency = default;
            return false;
        }

        public void RemoveDependency<T>(T dependency)
        {
            if (!_dependencies.TryGetValue(typeof(T), out var current)) return;
            if (!Equals(current, dependency)) return;

            _dependencies.Remove(typeof(T));
        }
    }
}
