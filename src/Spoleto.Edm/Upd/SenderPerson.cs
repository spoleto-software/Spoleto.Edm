namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о лице, передавшем товар (СвЛицПер). Заполняется одно свойство.
    /// </summary>
    public record SenderPerson
    {
        /// <summary>
        /// Работник организации продавца (РабОргПрод).
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// Иное лицо (ИнЛицо).
        /// </summary>
        public OtherIssuer? OtherIssuer { get; set; }
    }
}
