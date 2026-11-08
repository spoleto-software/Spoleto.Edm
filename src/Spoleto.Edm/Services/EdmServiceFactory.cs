using Spoleto.Edm.Providers;

namespace Spoleto.Edm
{
    /// <summary>
    /// Сборка <see cref="IEdmService"/> без DI: настройки и провайдеры, затем <see cref="Build"/>.
    /// </summary>
    public class EdmServiceFactory
    {
        private readonly EdmServiceOptions _options = new();
        private readonly List<IEdmProvider> _providers = [];

        /// <summary>
        /// Настройки сервиса.
        /// </summary>
        public EdmServiceFactory WithOptions(Action<EdmServiceOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);

            // set the EDM options and validate it.
            options(_options);
            _options.Validate();

            return this;
        }

        /// <summary>
        /// Провайдер оператора ЭДО.
        /// </summary>
        public EdmServiceFactory AddProvider(IEdmProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);

            _providers.Add(provider);

            return this;
        }

        /// <summary>
        /// Сервис с добавленными провайдерами.
        /// </summary>
        public IEdmService Build() => new EdmService(_providers, _options);
    }
}
