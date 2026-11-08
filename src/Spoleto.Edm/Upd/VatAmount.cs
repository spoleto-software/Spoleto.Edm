namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сумма НДС (СумНДСТип): сумма или «без НДС».
    /// </summary>
    public record VatAmount
    {
        /// <summary>
        /// Сумма налога (СумНал). Обязательна, если не <see cref="WithoutVat"/>.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Без НДС (БезНДС).
        /// </summary>
        public bool WithoutVat { get; set; }
    }
}
