namespace Spoleto.Edm
{
    /// <summary>
    /// Документ в списке документов оператора ЭДО: документооборот с его файлами.
    /// </summary>
    public record Document
    {
        /// <summary>
        /// Идентификатор документа у оператора ЭДО.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Время создания документа.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата документа.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Файлы документооборота: информация продавца, информация покупателя, квитанции.
        /// </summary>
        public List<DocumentInfo> Documents { get; set; } = [];

        /// <summary>
        /// Идентификатор группы документов.
        /// </summary>
        public string? GroupId { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Контрагент: у входящих документов - отправитель, у исходящих - получатель.
        /// </summary>
        public FirmInfo? Sender { get; set; }

        /// <summary>
        /// Статус документа.
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
        /// Время создания записи о документе.
        /// </summary>
        public DateTime CreateTimestamp { get; set; }

        /// <summary>
        /// Время выгрузки документа.
        /// </summary>
        public DateTime ExportTimestamp { get; set; }

        public override string? ToString() => Number;
    }
}
