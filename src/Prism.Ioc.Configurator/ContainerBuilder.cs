namespace Prism.Ioc.Configurator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The dependency injection container builder.
    /// </summary>
    public class ContainerBuilder
    {
        private readonly List<RegistrationConfig> _configs = new List<RegistrationConfig>();

        /// <summary>
        /// Registers the class types in the supplied assembly.
        /// </summary>
        /// <param name="assembly">The assembly to register the types from.</param>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig RegisterAssemblyTypes(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var types = assembly
                .GetTypes()
                .Where(type => type.IsClass)
                .Where(type => !type.IsAbstract)
                .ToList();
            var config = new RegistrationConfig(types);
            _configs.Add(config);
            return config;
        }

        /// <summary>
        /// Registers the supplied type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig RegisterType<T>()
        {
            var type = typeof(T);
            if (!type.IsClass)
            {
                throw new ArgumentException($"The supplied type isn't a class ({type.FullName}).");
            }
            else if (type.IsAbstract)
            {
                throw new ArgumentException($"The supplied type cannot be an abstract class ({type.FullName}).");
            }

            var config = new RegistrationConfig(type);
            _configs.Add(config);
            return config;
        }

        /// <summary>
        /// Registers the supplied instance.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <returns>The registration configuration.</returns>
        public RegistrationConfig RegisterInstance<T>(T instance)
            where T : class
        {
            var type = typeof(T);
            var config = new RegistrationConfig(type, instance);
            _configs.Add(config);
            return config;
        }

        /// <summary>
        /// Updated the registry container with the configuration in the container builder.
        /// </summary>
        /// <param name="container">The container registry.</param>
        public void Update(IContainerRegistry container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            foreach (var config in _configs)
            {
                config.Update(container);
            }
        }
    }
}
