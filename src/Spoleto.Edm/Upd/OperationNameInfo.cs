using Spoleto.Edm.Upd;

namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Код содержания операции (КодСодОпер).
    /// </summary>
    public record OperationNameInfo
    {
        /// <summary>
        /// Итог приемки товаров (работ, услуг, прав) (КодИтога).
        /// </summary>
        public OperationNameCode Code { get; set; } = OperationNameCode.Accepted;

        /// <summary>
        /// Реквизиты документа, оформляющего расхождения (РеквДокРасх).
        /// </summary>
        public DocumentDetails? DiscrepancyDocument { get; set; }
    }
}
