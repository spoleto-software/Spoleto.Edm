namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о принятии товаров (результатов выполненных работ), имущественных прав (о подтверждении факта оказания услуг) (СвПрин).
    /// </summary>
    public record AcceptanceInfo
    {
        /// <summary>
        /// Содержание операции (СодОпер), например, «Товары принял без претензий».
        /// </summary>
        /// <remarks>
        /// При итоге приемки <see cref="OperationNameCode.NotAccepted"/> не указывается.
        /// </remarks>
        public string? OperationName { get; set; }

        /// <summary>
        /// Дата принятия товаров (результатов выполненных работ), имущественных прав (подтверждения факта оказания услуг) (ДатаПрин).
        /// </summary>
        public required DateTime Date { get; set; }

        /// <summary>
        /// Код содержания операции (КодСодОпер): итог приемки и документ о расхождениях.
        /// </summary>
        public OperationNameInfo OperationNameInfo { get; set; } = new();

        /// <summary>
        /// Сведения о лице, принявшем товары (груз) (СвЛицПрин).
        /// </summary>
        public ReceiverPerson? ReceiverPerson { get; set; }
    }
}
