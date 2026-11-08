namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Настройки провайдера «ЭДО Лайт».
    /// </summary>
    public record TrueApiEdmOptions
    {
        /// <summary>
        /// Промышленный контур True API.
        /// </summary>
        public static TrueApiEdmOptions Production => new() { ServiceUrl = "https://markirovka.crpt.ru" };

        /// <summary>
        /// Тестовый стенд True API.
        /// </summary>
        public static TrueApiEdmOptions Sandbox => new() { ServiceUrl = "https://markirovka.sandbox.crptech.ru" };

        /// <summary>
        /// Адрес True API: по нему и методы «ЭДО Лайт», и токен.
        /// </summary>
        public string ServiceUrl { get; set; } = null!;

        /// <summary>
        /// Проверка: адрес True API обязателен.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ServiceUrl))
                throw new ArgumentNullException(nameof(ServiceUrl));
        }
    }
}
