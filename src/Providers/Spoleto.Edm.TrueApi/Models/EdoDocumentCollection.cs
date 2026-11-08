using System.Text.Json.Serialization;

namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Страница списка документов «ЭДО Лайт».
    /// </summary>
    internal record EdoDocumentCollection
    {
        /// <summary>
        /// Цепочки документов, найденные по заданным параметрам.
        /// </summary>
        [JsonPropertyName("items")]
        public List<EdoDocumentGroup> Items { get; set; } = [];

        /// <summary>
        /// Есть данные на следующей странице.
        /// </summary>
        [JsonPropertyName("has_next_page")]
        public bool HasNextPage { get; set; }

        public override string ToString() => $"Count: {Items.Count}";
    }
}
