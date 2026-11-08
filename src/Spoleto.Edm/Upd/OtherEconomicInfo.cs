namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Информационное поле факта хозяйственной жизни (ИнфПолФХЖ1, ИнфПолФХЖ3, ИнфПолФХЖ4).
    /// </summary>
    public record OtherEconomicInfo
    {
        /// <summary>
        /// Идентификатор файла информационного поля, GUID (ИдФайлИнфПол).
        /// </summary>
        public string? InfoFileId { get; set; }

        /// <summary>
        /// Текстовая информация (ТекстИнф).
        /// </summary>
        public List<OtherEconomicInfoItem>? Items { get; set; }
    }
}
