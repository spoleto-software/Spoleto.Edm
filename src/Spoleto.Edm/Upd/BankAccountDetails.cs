namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Банковские реквизиты (БанкРекв).
    /// </summary>
    public record BankAccountDetails
    {
        /// <summary>
        /// Номер банковского счета (НомерСчета).
        /// </summary>
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// Сведения о банке (СвБанк).
        /// </summary>
        public BankDetails? BankDetails { get; set; }

        public override string ToString() => $"{BankAccountNumber}, {BankDetails}";
    }
}
