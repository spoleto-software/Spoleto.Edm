namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Идентифицирующие реквизиты экономического субъекта, составившего документ (РеквИдРекСост). Заполняется одно свойство.
    /// </summary>
    public record DocumentCreator
    {
        /// <summary>
        /// ИНН организации (ИННЮЛ).
        /// </summary>
        public string? InnLegalEntity { get; set; }

        /// <summary>
        /// ИНН физического лица, в том числе ИП (ИННФЛ).
        /// </summary>
        public string? InnIndividual { get; set; }

        /// <summary>
        /// Данные об иностранной организации или гражданине (ДаннИно).
        /// </summary>
        public ForeignEntity? ForeignEntity { get; set; }

        /// <summary>
        /// Краткое наименование органа исполнительной власти, выдавшего документ (НаимОИВ).
        /// </summary>
        public string? AuthorityShortName { get; set; }
    }
}
