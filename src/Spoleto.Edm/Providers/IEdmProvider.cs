using Spoleto.Edm.Upd;

namespace Spoleto.Edm.Providers
{
    /// <summary>
    /// Провайдер оператора ЭДО.
    /// </summary>
    /// <remarks>
    /// Настройки оператора (адрес сервиса) провайдер получает в конструкторе, а от чьего имени вызов - <see cref="EdmAccount"/> - в каждом методе.
    /// Формат документов один для всех операторов (УПД по приказу ФНС № ЕД-7-26/970@), поэтому модели документов и XML - в ядре, а провайдер их отправляет.
    /// <para>
    /// Ошибка отправки сформированного файла возвращается в <see cref="EdmPostResult.Exception"/>, остальные ошибки бросаются.
    /// </para>
    /// </remarks>
    public interface IEdmProvider
    {
        /// <summary>
        /// Имя провайдера, например, <see cref="EdmProviderName.CRPT"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Отправка информации продавца УПД / УПДи: формирование XML, подпись сертификатом <see cref="EdmAccount.CertificateThumbprint"/> и загрузка.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="document">Информация продавца.</param>
        /// <param name="isDraft">Загрузить без подписи как черновик, не отправляя получателю.</param>
        /// <returns>Идентификатор документа у оператора и отправленный файл. Ошибка отправки - в <see cref="EdmPostResult.Exception"/>.</returns>
        EdmPostResult PostUniversalTransferDocument(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false);

        /// <inheritdoc cref="PostUniversalTransferDocument"/>
        Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Отправка информации покупателя (титула покупателя) к входящему УПД / УПДи: формирование XML по файлу продавца, подпись и загрузка.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="document">Информация покупателя.</param>
        /// <returns>Идентификатор, который вернул оператор, и отправленный файл. Ошибка отправки - в <see cref="EdmPostResult.Exception"/>.</returns>
        EdmPostResult ReceiptUniversalTransferDocument(EdmAccount account, BuyerUniversalTransferDocument document);

        /// <inheritdoc cref="ReceiptUniversalTransferDocument"/>
        Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Список входящих документов.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="searchModel">Критерии отбора. <see langword="null"/> - без отбора.</param>
        DocumentCollection GetIncomingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="GetIncomingDocumentList"/>
        Task<DocumentCollection> GetIncomingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Список исходящих документов.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="searchModel">Критерии отбора. <see langword="null"/> - без отбора.</param>
        DocumentCollection GetOutgoingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="GetOutgoingDocumentList"/>
        Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Файл входящего документа как есть: XML для УПД, УКД и квитанций, у других операторов и типов - что пришло.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор документа у оператора.</param>
        byte[] GetIncomingDocument(EdmAccount account, string documentId);

        /// <inheritdoc cref="GetIncomingDocument"/>
        Task<byte[]> GetIncomingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Файл исходящего документа как есть.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор документа у оператора.</param>
        byte[] GetOutgoingDocument(EdmAccount account, string documentId);

        /// <inheritdoc cref="GetOutgoingDocument"/>
        Task<byte[]> GetOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сведения из файла информации продавца входящего УПД / УПДи, которые повторяет информация покупателя, в том числе ИдФайл.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор документа у оператора.</param>
        SellerDocumentInfo GetIncomingSellerDocumentInfo(EdmAccount account, string documentId);

        /// <inheritdoc cref="GetIncomingSellerDocumentInfo"/>
        Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сведения из файла информации продавца исходящего УПД / УПДи, в том числе ИдФайл.
        /// </summary>
        /// <param name="account">От чьего имени вызов.</param>
        /// <param name="documentId">Идентификатор документа у оператора.</param>
        SellerDocumentInfo GetOutgoingSellerDocumentInfo(EdmAccount account, string documentId);

        /// <inheritdoc cref="GetOutgoingSellerDocumentInfo"/>
        Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);
    }
}
