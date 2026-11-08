namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Способ подтверждения полномочий представителя на подписание документа (СпосПодтПолном).
    /// </summary>
    public enum AuthorityConfirmationType
    {
        /// <summary>
        /// В соответствии с данными, содержащимися в электронной подписи.
        /// </summary>
        ElectronicSignatureData = 1,

        /// <summary>
        /// Машиночитаемая доверенность включена в каждый пакет электронных документов, подписываемых представителем.
        /// </summary>
        IncludedMachineReadablePowerOfAttorney = 2,

        /// <summary>
        /// Машиночитаемая доверенность из информационной системы, сведения для ее запроса указаны в документе (<see cref="Signer.ElectronicPoAInfo"/>).
        /// </summary>
        SystemMachineReadablePoAWithDocInfo = 3,

        /// <summary>
        /// Машиночитаемая доверенность из информационной системы, сведения для ее запроса представлены иным способом.
        /// </summary>
        SystemMachineReadablePoAWithOtherInfo = 4,

        /// <summary>
        /// Доверенность в форме документа на бумажном носителе (<see cref="Signer.PaperPoAInfo"/>).
        /// </summary>
        PaperBasedDocument = 5,

        /// <summary>
        /// Иное.
        /// </summary>
        Other = 6
    }
}
