namespace Spoleto.Edm
{
    /// <summary>
    /// Файл документооборота у оператора ЭДО.
    /// </summary>
    public record DocumentInfo
    {
        /// <summary>
        /// Идентификатор файла у оператора ЭДО.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Время создания.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата документа.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Время последней обработки.
        /// </summary>
        public DateTime ProcessedAt { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        public DocumentState Status { get; set; }

        /// <summary>
        /// Сумма с НДС.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Сумма НДС.
        /// </summary>
        public decimal TotalVatAmount { get; set; }

        /// <summary>
        /// Тип документа.
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// Документ подписан получателем.
        /// </summary>
        public bool? IsTitleSigned { get; set; }

        public override string? ToString() => Number;
    }
}
