namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о лице, принявшем товары (груз) (СвЛицПрин). Заполняется одно свойство.
    /// </summary>
    public record ReceiverPerson
    {
        /// <summary>
        /// Работник организации покупателя (РабОргПок).
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// Иное лицо (ИнЛицо): представитель организации, которой доверено принятие товаров (ПредОргПрин),
        /// или физическое лицо, которому доверено принятие товаров (ФЛПрин).
        /// </summary>
        public OtherIssuer? OtherIssuer { get; set; }
    }
}
