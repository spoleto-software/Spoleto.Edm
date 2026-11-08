using System.Text.Json.Serialization;

namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// ФИО участника обмена «ЭДО Лайт».
    /// </summary>
    internal record EdoPersonName
    {
        /// <summary>
        /// Фамилия.
        /// </summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [JsonPropertyName("middle_name")]
        public string? MiddleName { get; set; }

        public override string ToString() => string.Join(" ", new[] { LastName, FirstName, MiddleName }.Where(x => !string.IsNullOrEmpty(x)));
    }
}
