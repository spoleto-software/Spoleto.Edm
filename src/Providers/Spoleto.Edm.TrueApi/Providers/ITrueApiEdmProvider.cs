namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Провайдер «ЭДО Лайт» ЦРПТ: методы <see cref="IEdmProvider"/> и то, что есть только у «ЭДО Лайт».
    /// </summary>
    public interface ITrueApiEdmProvider : IEdmProvider
    {
        /// <summary>
        /// Получение файла входящего документа с его открепленной подписью.
        /// </summary>
        /// <remarks>
        /// Скачивает ZIP-архив документа с подписями и квитанциями. Из архива берётся файл, по которому назван архив, а без имени архива - информация продавца УПД / УПДи.
        /// Архив есть только у УПД, УПДи, УКД и УКДи.
        /// </remarks>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор документа в «ЭДО Лайт».</param>
        /// <returns>XML документа строкой и байтами как есть, подпись как есть.</returns>
        SignedDocumentInfo GetIncomingSignedDocumentInfo(EdmAccount account, string documentId);

        /// <inheritdoc cref="GetIncomingSignedDocumentInfo"/>
        Task<SignedDocumentInfo> GetIncomingSignedDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение файла исходящего документа с его открепленной подписью.
        /// </summary>
        /// <remarks>
        /// Скачивает ZIP-архив документа с подписями и квитанциями. Из архива берётся файл, по которому назван архив, а без имени архива - информация продавца УПД / УПДи.
        /// Архив есть только у УПД, УПДи, УКД и УКДи.
        /// </remarks>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор документа в «ЭДО Лайт».</param>
        /// <returns>XML документа строкой и байтами как есть, подпись как есть.</returns>
        SignedDocumentInfo GetOutgoingSignedDocumentInfo(EdmAccount account, string documentId);

        /// <inheritdoc cref="GetOutgoingSignedDocumentInfo"/>
        Task<SignedDocumentInfo> GetOutgoingSignedDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Подписание черновика исходящего документа: УПД, УПДи, УКД или УКДи. После подписания документ уходит получателю.
        /// </summary>
        /// <remarks>
        /// Скачивает файл черновика, как <see cref="IEdmProvider.GetOutgoingDocumentAsync"/>, подписывает его байты как есть открепленной подписью
        /// сертификатом <see cref="EdmAccount.CertificateThumbprint"/> и отправляет подпись.
        /// </remarks>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор черновика в «ЭДО Лайт», полученный при его загрузке.</param>
        void SignOutgoingDocument(EdmAccount account, string documentId);

        /// <inheritdoc cref="SignOutgoingDocument"/>
        Task SignOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);
    }
}
