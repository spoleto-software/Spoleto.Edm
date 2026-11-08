namespace Spoleto.Edm
{
    /// <summary>
    /// Страница списка документов.
    /// </summary>
    public record DocumentCollection
    {
        /// <summary>
        /// Документы страницы.
        /// </summary>
        public List<Document> Items { get; set; } = [];

        /// <summary>
        /// Есть следующая страница.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Число документов на странице.
        /// </summary>
        public int Count => Items?.Count ?? 0;

        public override string ToString() => $"Count: {Count}";
    }
}
