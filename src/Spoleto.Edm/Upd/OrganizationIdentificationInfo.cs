namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Идентификационные сведения участника (ИдСв). Заполняется ровно одно свойство.
    /// </summary>
    public record OrganizationIdentificationInfo
    {
        /// <summary>
        /// Сведения об индивидуальном предпринимателе (СвИП).
        /// </summary>
        public IndividualEntrepreneur? IndividualEntrepreneur { get; set; }

        /// <summary>
        /// Сведения о юридическом лице, состоящем на учете в налоговых органах (СвЮЛУч).
        /// </summary>
        public LegalPerson? LegalPerson { get; set; }

        /// <summary>
        /// Сведения об иностранном лице, не состоящем на учете в налоговых органах (СвИнНеУч).
        /// </summary>
        public ForeignEntity? ForeignPerson { get; set; }

        /// <summary>
        /// Сведения о физическом лице (СвФЛУч), для функции ДОП.
        /// </summary>
        public PhysicalPerson? PhysicalPerson { get; set; }

        public override string ToString() => LegalPerson?.ToString() ?? IndividualEntrepreneur?.ToString() ?? PhysicalPerson?.ToString() ?? ForeignPerson?.ToString() ?? string.Empty;
    }
}
