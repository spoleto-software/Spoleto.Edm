using System.ComponentModel;

namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Поле сортировки списка документов «ЭДО Лайт».
    /// </summary>
    internal enum EdoDocumentSortField
    {
        /// <summary>
        /// Время создания документа.
        /// </summary>
        [Description("created_at")]
        CreatedAt
    }
}
