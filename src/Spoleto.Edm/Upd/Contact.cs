namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Контактные данные (Контакт).
    /// </summary>
    public record Contact
    {
        /// <summary>
        /// Номера телефона или факса (Тлф).
        /// </summary>
        public List<string>? PhoneList { get; set; }

        /// <summary>
        /// Адреса электронной почты (ЭлПочта).
        /// </summary>
        public List<string>? EmailList { get; set; }

        /// <summary>
        /// Иные контактные данные (ИнКонт).
        /// </summary>
        public string? OtherInfo { get; set; }
    }
}
