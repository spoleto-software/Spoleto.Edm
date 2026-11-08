namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Вид платежа (ВидПлат) в информации покупателя для Федерального казначейства.
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// Пусто.
        /// </summary>
        Empty = 0,

        /// <summary>
        /// Срочно.
        /// </summary>
        Urgent = 4
    }
}
