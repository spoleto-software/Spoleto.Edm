namespace Spoleto.Edm
{
    /// <summary>
    /// Критерии отбора документов.
    /// </summary>
    /// <remarks>
    /// Даты с <see cref="DateTimeKind.Unspecified"/> или <see cref="DateTimeKind.Local"/> считаются местным временем.
    /// </remarks>
    public record DocumentCollectionSearchModel
    {
        /// <summary>
        /// Число документов на странице.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Сколько документов пропустить.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Созданы не раньше.
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Созданы не позже.
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// ИНН контрагента.
        /// </summary>
        public string? PartnerInn { get; set; }

        /// <summary>
        /// Статусы: документ подходит, если его статус - один из списка. Пустой список - любой статус.
        /// </summary>
        public List<DocumentState>? Status { get; set; }

        /// <summary>
        /// Типы: документ подходит, если его тип - один из списка. Пустой список - любой тип.
        /// </summary>
        public List<DocumentType>? Type { get; set; }

        /// <summary>
        /// Сортировка.
        /// </summary>
        public SortField? SortBy { get; set; }

        /// <summary>
        /// Сортировка по возрастанию.
        /// </summary>
        public bool? Asc { get; set; }
    }
}
