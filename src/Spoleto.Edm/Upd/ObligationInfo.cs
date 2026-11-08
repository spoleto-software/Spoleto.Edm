namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Вид обязательства по справочнику LIM_02 «Вид обязательств» ФГИС ЕСНСИ (ВидОбяз).
    /// </summary>
    public record ObligationInfo
    {
        /// <summary>
        /// Код вида обязательств, поле CODE_VALUE справочника (КодВидОбяз).
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Наименование вида обязательств, поле NAME_VALUE справочника (НаимВидОбяз).
        /// </summary>
        public string? Name { get; set; }
    }
}
