namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о доверенности в форме документа на бумажном носителе (СвДоверБум).
    /// </summary>
    public record PaperPoAInfo
    {
        /// <summary>
        /// Дата выдачи доверенности (ДатаВыдДовер).
        /// </summary>
        public required DateTime IssueDate { get; set; }

        /// <summary>
        /// Внутренний номер доверенности (ВнНомДовер). При отсутствии номера - «б/н».
        /// </summary>
        public required string InternalNumber { get; set; }

        /// <summary>
        /// Сведения, идентифицирующие доверителя (СвИдДовер).
        /// </summary>
        public string? TrusteeInfo { get; set; }

        /// <summary>
        /// ФИО лица, подписавшего доверенность (ФИО).
        /// </summary>
        public Person? Signer { get; set; }
    }
}
