namespace Spoleto.Edm
{
    /// <summary>
    /// Настройки <see cref="EdmService"/>.
    /// </summary>
    public record EdmServiceOptions
    {
        /// <summary>
        /// Имя провайдера по умолчанию: вызовы без имени провайдера идут к нему.
        /// </summary>
        public string DefaultProvider { get; set; } = null!;

        /// <summary>
        /// Проверка: провайдер по умолчанию обязателен.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(DefaultProvider))
                throw new ArgumentException("You have to specify a valid EDM provider to be used as the default.", nameof(DefaultProvider));
        }
    }
}
