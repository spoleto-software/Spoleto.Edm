namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Физическое лицо, которому доверена отгрузка товаров (ФЛПер) или их принятие (ФЛПрин).
    /// </summary>
    public record TransferPhysicalPerson : Person
    {
        /// <summary>
        /// ИНН физического лица (ИННФЛПер, ИННФЛПрин).
        /// </summary>
        public string? PersonInn { get; set; }

        /// <summary>
        /// Иные сведения, идентифицирующие физическое лицо (ИныеСвед).
        /// </summary>
        public string? OtherInfo { get; set; }

        /// <summary>
        /// Основание, по которому физическому лицу доверена отгрузка или принятие товаров (ОснДоверФЛ).
        /// </summary>
        public DocumentDetails? PersonBase { get; set; }
    }
}
