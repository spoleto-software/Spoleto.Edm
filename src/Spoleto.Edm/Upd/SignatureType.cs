namespace Spoleto.Edm.Upd
{
    /// <summary>
    /// Тип подписи (ТипПодпис).
    /// </summary>
    public enum SignatureType
    {
        /// <summary>
        /// Усиленная квалифицированная электронная подпись.
        /// </summary>
        QualifiedElectronicSignature = 1,

        /// <summary>
        /// Простая электронная подпись.
        /// </summary>
        SimpleElectronicSignature = 2,

        /// <summary>
        /// Усиленная неквалифицированная электронная подпись.
        /// </summary>
        UnqualifiedElectronicSignature = 3
    }
}
