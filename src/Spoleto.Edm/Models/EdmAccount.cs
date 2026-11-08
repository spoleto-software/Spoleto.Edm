namespace Spoleto.Edm
{
    /// <summary>
    /// От чьего имени идут вызовы оператора ЭДО: сертификат и организация.
    /// </summary>
    /// <remarks>
    /// Меняется от вызова к вызову, поэтому передаётся в каждый метод. Настройки самого оператора (адрес сервиса) - в настройках провайдера.
    /// </remarks>
    /// <param name="CertificateThumbprint">Отпечаток сертификата: им подписываются документы и, если оператор так требует, выполняется вход.</param>
    /// <param name="Inn">ИНН организации. Нужен, если пользователь работает по МЧД от нескольких организаций.</param>
    public record EdmAccount(string CertificateThumbprint, string? Inn = null)
    {
        /// <summary>
        /// Проверка: отпечаток сертификата обязателен.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CertificateThumbprint))
                throw new ArgumentNullException(nameof(CertificateThumbprint));
        }

        public override string ToString() => string.IsNullOrEmpty(Inn) ? CertificateThumbprint : $"{Inn} ({CertificateThumbprint})";
    }
}
