namespace Spoleto.Edm
{
    /// <summary>
    /// Участник документооборота: отправитель или получатель.
    /// </summary>
    public record FirmInfo
    {
        /// <summary>
        /// Идентификатор участника у оператора ЭДО.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дополнительный идентификатор.
        /// </summary>
        public string? ExtraId { get; set; }

        /// <summary>
        /// ИНН.
        /// </summary>
        public string? Inn { get; set; }

        /// <summary>
        /// КПП.
        /// </summary>
        public string? Kpp { get; set; }

        /// <summary>
        /// ОГРН.
        /// </summary>
        public string? Ogrn { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string? Email { get; set; }

        public override string? ToString() => Name;
    }
}
