namespace Prism.Ioc.Configurator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The registration configuration for types.
    /// </summary>
    public class RegistrationConfig
    {
        private readonly ICollection<Type> _types;
        private bool _asImplementedInterfaces;
        private Type? _asType;
        private bool _singleInstance;
        private Func<Type, bool>? _whereExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationConfig"/> class.
        /// </summary>
        /// <param name="types">The types that are going to be registered.</param>
        public RegistrationConfig(ICollection<Type> types)
        {
            _types = types ?? throw new ArgumentNullException(nameof(types));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationConfig"/> class.
        /// </summary>
        /// <param name="type">The type that is going to be registered.</param>
        public RegistrationConfig(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            _types = new List<Type>
            {
                type,
            };
        }

        /// <summary>
        /// Configures as which type the supplied class should be registered.
        /// </summary>
        /// <typeparam name="T">The type the supplied class should be registered.</typeparam>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig As<T>()
        {
            _asType = typeof(T);
            return this;
        }

        /// <summary>
        /// Configures that the supplied class type(s) should be registered with their implemented interfaces.
        /// </summary>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig AsImplementedInterfaces()
        {
            _asImplementedInterfaces = true;
            return this;
        }

        /// <summary>
        /// Configures that the supplied class type(s) should be registered with their own type.
        /// </summary>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig AsSelf()
        {
            _asImplementedInterfaces = false;
            return this;
        }

        /// <summary>
        /// Configures that the supplied class type(s) should be registered as single instance(s).
        /// </summary>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig SingleInstance()
        {
            _singleInstance = true;
            return this;
        }

        /// <summary>
        /// Configures that the supplied class type(s) should be registered as instance per dependency.
        /// </summary>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig InstancePerDependency()
        {
            _singleInstance = false;
            return this;
        }

        /// <summary>
        /// Configures a filter that determines which types should be used for registration.
        /// </summary>
        /// <param name="filterExpression">The expression that filters the supplied type.</param>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig Where(Expression<Func<Type, bool>> filterExpression)
        {
            _whereExpression = filterExpression.Compile();
            return this;
        }

        /// <summary>
        /// Updated the registry container with the configuration in the container builder.
        /// </summary>
        /// <param name="container">The container registry.</param>
        internal void Update(IContainerRegistry container)
        {
            var types = GetFilteredType(_types);
            foreach (var type in types)
            {
                if (_singleInstance)
                {
                    if (_asType != null)
                    {
                        container.RegisterSingleton(_asType, type);
                    }
                    else if (_asImplementedInterfaces)
                    {
                        foreach (var interfaceType in type.GetInterfaces())
                        {
                            container.RegisterSingleton(interfaceType, type);
                        }
                    }
                    else
                    {
                        container.RegisterSingleton(type);
                    }
                }
                else
                {
                    if (_asType != null)
                    {
                        container.Register(_asType, type);
                    }
                    else if (_asImplementedInterfaces)
                    {
                        foreach (var interfaceType in type.GetInterfaces())
                        {
                            container.Register(interfaceType, type);
                        }
                    }
                    else
                    {
                        container.Register(type);
                    }
                }
            }
        }

        private IEnumerable<Type> GetFilteredType(ICollection<Type> types)
        {
            if (_whereExpression == null)
            {
                return types;
            }

            return types.Where(_whereExpression);
        }
    }
}
