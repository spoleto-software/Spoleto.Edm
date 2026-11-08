namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Иное лицо, передавшее товар в информации продавца или принявшее товар в информации покупателя (ИнЛицо). Заполняется одно свойство.
    /// </summary>
    public record OtherIssuer
    {
        /// <summary>
        /// Представитель организации, которой доверена отгрузка товаров (ПредОргПер) или их принятие (ПредОргПрин).
        /// </summary>
        public TransferOrganizationPerson? OrganizationPerson { get; set; }

        /// <summary>
        /// Физическое лицо, которому доверена отгрузка товаров (ФЛПер) или их принятие (ФЛПрин).
        /// </summary>
        public TransferPhysicalPerson? PhysicalPerson { get; set; }
    }
}
