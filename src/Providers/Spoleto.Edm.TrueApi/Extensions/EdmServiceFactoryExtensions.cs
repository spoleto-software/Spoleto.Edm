using Spoleto.Edm.Providers.TrueApi;

namespace Spoleto.Edm
{
    /// <summary>
    /// Добавление провайдера «ЭДО Лайт» в <see cref="EdmServiceFactory"/>.
    /// </summary>
    public static class EdmServiceFactoryExtensions
    {
        /// <summary>
        /// Провайдер «ЭДО Лайт» с адресом True API.
        /// </summary>
        public static EdmServiceFactory AddTrueApi(this EdmServiceFactory builder, string serviceUrl)
            => builder.AddTrueApi(x => x.ServiceUrl = serviceUrl);

        /// <summary>
        /// Провайдер «ЭДО Лайт» с настройками.
        /// </summary>
        public static EdmServiceFactory AddTrueApi(this EdmServiceFactory builder, Action<TrueApiEdmOptions> config)
        {
            ArgumentNullException.ThrowIfNull(config);

            // loads the options
            var options = new TrueApiEdmOptions();
            config(options);

            // validates the options
            options.Validate();

            // add the provider to the EDM service factory
            builder.AddProvider(new TrueApiEdmProvider(options));

            return builder;
        }

        /// <summary>
        /// Готовый провайдер «ЭДО Лайт», например, со своим <see cref="HttpClient"/> и провайдером токенов.
        /// </summary>
        public static EdmServiceFactory AddTrueApi(this EdmServiceFactory builder, TrueApiEdmProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);

            // add the provider to the EDM service factory
            builder.AddProvider(provider);

            return builder;
        }
    }
}
