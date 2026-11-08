namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Сведения о юридическом лице, состоящем на учете в налоговых органах (СвЮЛУч).
    /// </summary>
    public record LegalPerson
    {
        /// <summary>
        /// Полное наименование (НаимОрг).
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// ИНН (ИННЮЛ).
        /// </summary>
        public required string Inn { get; set; }

        /// <summary>
        /// КПП (КПП).
        /// </summary>
        public string? Kpp { get; set; }

        public override string ToString() => Name;
    }
}
