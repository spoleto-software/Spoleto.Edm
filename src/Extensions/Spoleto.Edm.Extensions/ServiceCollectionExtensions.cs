using Microsoft.Extensions.DependencyInjection;

namespace Spoleto.Edm.Extensions
{
    /// <summary>
    /// Регистрация <see cref="IEdmService"/> в DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// <see cref="IEdmService"/> с провайдером по умолчанию.
        /// </summary>
        public static EdmServiceBuilder AddEdm(this IServiceCollection serviceCollection, string defaultProviderName)
            => AddEdm(serviceCollection, options => options.DefaultProvider = defaultProviderName);

        /// <summary>
        /// <see cref="IEdmService"/> с настройками.
        /// </summary>
        /// <remarks>
        /// Сервис и провайдеры - одиночки: провайдеры хранят токены операторов, а каждый новый токен - это подпись в КриптоПро.
        /// </remarks>
        public static EdmServiceBuilder AddEdm(this IServiceCollection serviceCollection, Action<EdmServiceOptions> config)
        {
            ArgumentNullException.ThrowIfNull(config);

            // loads the options
            var options = new EdmServiceOptions();
            config(options);

            serviceCollection.AddSingleton(s => options);
            serviceCollection.AddSingleton<IEdmService, EdmService>();

            // registers the providers on this instance:
            return new EdmServiceBuilder(serviceCollection);
        }
    }
}
