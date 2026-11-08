namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о факте отгрузки товаров, выполнения работ, передачи имущественных прав (СвПродПер).
    /// </summary>
    public record TransferInfo
    {
        /// <summary>
        /// Сведения о передаче (СвПер).
        /// </summary>
        public required TransferDetails TransferDetails { get; set; }

        /// <summary>
        /// Информационное поле факта хозяйственной жизни 3 (ИнфПолФХЖ3).
        /// </summary>
        public OtherEconomicInfo? OtherEconomicInfo { get; set; }
    }
}
