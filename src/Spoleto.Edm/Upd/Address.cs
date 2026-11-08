namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Адрес (Адрес). Заполняется ровно один вид: адрес в РФ, адрес по ГАР или адрес за пределами РФ.
    /// </summary>
    public record Address
    {
        /// <summary>
        /// Глобальный идентификационный номер места GLN (ГЛНМеста).
        /// </summary>
        public string? GlobalLocationNumber { get; set; }

        /// <summary>
        /// Адрес в Российской Федерации (АдрРФ).
        /// </summary>
        public RussianAddress? RussianAddress { get; set; }

        /// <summary>
        /// Адрес по уникальному номеру в государственном адресном реестре (АдрГАР).
        /// </summary>
        public AddressCode? AddressCode { get; set; }

        /// <summary>
        /// Адрес за пределами Российской Федерации (АдрИнф).
        /// </summary>
        public AddressInformation? AddressInformation { get; set; }

        public override string ToString() => RussianAddress?.ToString() ?? AddressInformation?.ToString() ?? AddressCode?.ToString() ?? string.Empty;
    }
}
