namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Транспортировка (Тран).
    /// </summary>
    public record Transportation
    {
        /// <summary>
        /// Сведения о транспортировке и грузе (СвТран).
        /// </summary>
        public string? TransferTextInfo { get; set; }

        /// <summary>
        /// Код условий поставки по Инкотермс (Инкотермс).
        /// </summary>
        public string? Incoterms { get; set; }

        /// <summary>
        /// Версия Инкотермс (ВерИнкотермс).
        /// </summary>
        public string? IncotermsVersion { get; set; }
    }
}
