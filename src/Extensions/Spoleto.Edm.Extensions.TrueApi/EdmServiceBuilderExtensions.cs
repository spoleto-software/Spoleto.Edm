using Microsoft.Extensions.DependencyInjection;
using Spoleto.Edm.Providers;
using Spoleto.Edm.Providers.TrueApi;

namespace Spoleto.Edm.Extensions.TrueApi
{
    /// <summary>
    /// Регистрация провайдера «ЭДО Лайт» в DI.
    /// </summary>
    public static class EdmServiceBuilderExtensions
    {
        /// <summary>
        /// Провайдер «ЭДО Лайт» с адресом True API.
        /// </summary>
        public static EdmServiceBuilder AddTrueApi(this EdmServiceBuilder builder, string serviceUrl)
            => builder.AddTrueApi(x => x.ServiceUrl = serviceUrl);

        /// <summary>
        /// Провайдер «ЭДО Лайт» с настройками и своим <see cref="HttpClient"/>.
        /// </summary>
        public static EdmServiceBuilder AddTrueApi(this EdmServiceBuilder builder, Action<TrueApiEdmOptions> config)
        {
            ArgumentNullException.ThrowIfNull(config);

            // loads the options
            var options = new TrueApiEdmOptions();
            config(options);

            // validates the options
            options.Validate();

            builder.ServiceCollection.AddSingleton(s => options);

            return builder.AddTrueApi((IServiceProvider s) => new TrueApiEdmProvider(s.GetRequiredService<TrueApiEdmOptions>()));
        }

        /// <summary>
        /// Готовый провайдер «ЭДО Лайт», например, со своим <see cref="HttpClient"/> и провайдером токенов. Освобождает его тот, кто создал.
        /// </summary>
        public static EdmServiceBuilder AddTrueApi(this EdmServiceBuilder builder, TrueApiEdmProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);

            // Регистрации экземпляром: контейнер освобождает только то, что вернула фабрика.
            builder.ServiceCollection.AddSingleton(provider);
            builder.ServiceCollection.AddSingleton<IEdmProvider>(provider);
            builder.ServiceCollection.AddSingleton<ITrueApiEdmProvider>(provider);

            return builder;
        }

        /// <summary>
        /// Провайдер «ЭДО Лайт», который создаёт <paramref name="factory"/>.
        /// </summary>
        public static EdmServiceBuilder AddTrueApi(this EdmServiceBuilder builder, Func<IServiceProvider, TrueApiEdmProvider> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            // Один экземпляр на оба интерфейса.
            builder.ServiceCollection.AddSingleton(factory);
            builder.ServiceCollection.AddSingleton<IEdmProvider>(s => s.GetRequiredService<TrueApiEdmProvider>());
            builder.ServiceCollection.AddSingleton<ITrueApiEdmProvider>(s => s.GetRequiredService<TrueApiEdmProvider>());

            return builder;
        }
    }
}
