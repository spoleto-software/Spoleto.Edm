namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Фамилия, имя, отчество (ФИО).
    /// </summary>
    public record Person
    {
        /// <summary>
        /// Фамилия (Фамилия).
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// Имя (Имя).
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Отчество (Отчество).
        /// </summary>
        public string? Patronymic { get; set; }

        public override string ToString() => string.Join(" ", new[] { Surname, FirstName, Patronymic }.Where(x => !string.IsNullOrEmpty(x)));
    }
}
