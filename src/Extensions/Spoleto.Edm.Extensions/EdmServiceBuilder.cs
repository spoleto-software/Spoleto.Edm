using Microsoft.Extensions.DependencyInjection;

namespace Spoleto.Edm.Extensions
{
    /// <summary>
    /// Регистрация провайдеров ЭДО после <see cref="ServiceCollectionExtensions.AddEdm(IServiceCollection, string)"/>.
    /// </summary>
    public class EdmServiceBuilder
    {
        internal EdmServiceBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Коллекция сервисов, в которую регистрируются провайдеры.
        /// </summary>
        public IServiceCollection ServiceCollection { get; }
    }
}
