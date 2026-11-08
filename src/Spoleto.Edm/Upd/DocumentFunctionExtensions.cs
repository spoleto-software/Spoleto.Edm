namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Наименования документа по его функции: их подставляет генератор, если в модели они не заданы.
    /// </summary>
    public static class DocumentFunctionExtensions
    {
        /// <summary>
        /// Наименование первичного документа (НаимДокОпр). У счета-фактуры (<see cref="DocumentFunction.Invoice"/>) его нет.
        /// </summary>
        public static string? GetDocumentName(this DocumentFunction function) => function switch
        {
            DocumentFunction.InvoiceAndPrimaryDocument => "Счет-фактура и документ об отгрузке товаров (выполнении работ), передаче имущественных прав (документ об оказании услуг)",
            DocumentFunction.PrimaryDocument => "Документ об отгрузке товаров (выполнении работ), передаче имущественных прав (документ об оказании услуг)",
            _ => null
        };

        /// <summary>
        /// Наименование документа по факту хозяйственной жизни (ПоФактХЖ).
        /// </summary>
        public static string? GetDocumentEconomicName(this DocumentFunction function) => function switch
        {
            DocumentFunction.InvoiceAndPrimaryDocument or DocumentFunction.PrimaryDocument => "Документ об отгрузке товаров (выполнении работ), передаче имущественных прав (документ об оказании услуг)",
            DocumentFunction.CommissionSale => "Документ, включающий в себя сведения о факте реализации комиссионером (агентом, действующим от собственного имени) товаров комитента (принципала)",
            DocumentFunction.CommissionPurchase => "Документ, включающий в себя сведения о факте закупки комиссионером (агентом, действующим от собственного имени) товаров для комитента (принципала)",
            _ => null
        };
    }
}
