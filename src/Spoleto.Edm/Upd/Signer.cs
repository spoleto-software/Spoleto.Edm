using Spoleto.Edm.Upd;

namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Лицо, подписывающее файл информации продавца или покупателя (Подписант).
    /// </summary>
    public record Signer
    {
        /// <summary>
        /// Должность (Должн).
        /// </summary>
        public string? JobTitle { get; set; }

        /// <summary>
        /// Тип подписи (ТипПодпис).
        /// </summary>
        public SignatureType? SignatureType { get; set; }

        /// <summary>
        /// Дата подписания документа (ДатаПодДок).
        /// </summary>
        public DateTime? SigningDate { get; set; }

        /// <summary>
        /// Способ подтверждения полномочий представителя на подписание документа (СпосПодтПолном).
        /// </summary>
        public required AuthorityConfirmationType AuthorityConfirmationType { get; set; }

        /// <summary>
        /// Дополнительные сведения о подписанте (ДопСведПодп).
        /// </summary>
        public string? AdditionalInfo { get; set; }

        /// <summary>
        /// Фамилия, имя, отчество (ФИО).
        /// </summary>
        public required Person Person { get; set; }

        /// <summary>
        /// Сведения о машиночитаемой доверенности (СвДоверЭл).
        /// Обязательны при <see cref="AuthorityConfirmationType.SystemMachineReadablePoAWithDocInfo"/>.
        /// </summary>
        public ElectronicPoAInfo? ElectronicPoAInfo { get; set; }

        /// <summary>
        /// Сведения о доверенности на бумажном носителе (СвДоверБум).
        /// Обязательны при <see cref="AuthorityConfirmationType.PaperBasedDocument"/>. Не формируются, если задана <see cref="ElectronicPoAInfo"/>.
        /// </summary>
        public PaperPoAInfo? PaperPoAInfo { get; set; }

        public override string ToString() => Person.ToString();
    }
}
