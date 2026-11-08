namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о физическом лице (СвФЛУч).
    /// </summary>
    public record PhysicalPerson : Person
    {
        /// <summary>
        /// ИНН (ИННФЛ).
        /// </summary>
        public string? Inn { get; set; }

        /// <summary>
        /// Идентификация статуса лица (ИдСтатЛ): «0» - покупатель при розничной продаже, «1» - иное.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Иные сведения, идентифицирующие физическое лицо (ИныеСвед).
        /// </summary>
        public string? OtherInfo { get; set; }
    }
}
