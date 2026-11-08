namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о передаче вещи, изготовленной по договору подряда (СвПерВещи).
    /// </summary>
    public record CreatedThingTransferInfo
    {
        /// <summary>
        /// Дата передачи вещи (ДатаПерВещ).
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Сведения о передаче (СвПерВещ).
        /// </summary>
        public string? Information { get; set; }

        /// <summary>
        /// Реквизиты документа о передаче вещи (ДокПерВещ).
        /// </summary>
        public DocumentDetails? Document { get; set; }
    }
}
