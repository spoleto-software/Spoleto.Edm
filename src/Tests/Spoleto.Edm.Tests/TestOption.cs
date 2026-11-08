namespace Spoleto.Edm.Tests
{
    /// <summary>
    /// Настройки стенда True API из секретов пользователя: тот же раздел, что у тестов Spoleto.TrueApi.
    /// </summary>
    public record TestOption
    {
        /// <summary>
        /// Отпечаток сертификата.
        /// </summary>
        public string CertificateThumbprint { get; set; } = null!;

        /// <summary>
        /// Адрес True API стенда.
        /// </summary>
        public string ServiceUrl { get; set; } = null!;

        /// <summary>
        /// ИНН организации сертификата.
        /// </summary>
        public string Inn { get; set; } = null!;
    }
}
