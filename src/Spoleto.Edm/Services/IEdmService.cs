using Spoleto.Edm.Providers;
using Spoleto.Edm.Upd;

namespace Spoleto.Edm
{
    /// <summary>
    /// Работа с операторами ЭДО: вызов уходит провайдеру по умолчанию, провайдеру с заданным именем или переданному провайдеру.
    /// </summary>
    public interface IEdmService
    {
        /// <summary>
        /// Зарегистрированные провайдеры.
        /// </summary>
        IEnumerable<IEdmProvider> Providers { get; }

        /// <summary>
        /// Провайдер по умолчанию (<see cref="EdmServiceOptions.DefaultProvider"/>).
        /// </summary>
        IEdmProvider DefaultProvider { get; }

        /// <summary>
        /// Провайдер по имени, например, по коду оператора ЭДО организации.
        /// </summary>
        /// <exception cref="EdmProviderNotFoundException">Провайдера с таким именем нет.</exception>
        IEdmProvider GetProvider(string providerName);

        /// <inheritdoc cref="GetProvider(string)"/>
        IEdmProvider GetProvider(EdmProviderName providerName);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocument"/>
        EdmPostResult PostUniversalTransferDocument(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocument"/>
        EdmPostResult PostUniversalTransferDocument(string providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocument"/>
        EdmPostResult PostUniversalTransferDocument(EdmProviderName providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocument"/>
        EdmPostResult PostUniversalTransferDocument(IEdmProvider provider, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> PostUniversalTransferDocumentAsync(string providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmProviderName providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.PostUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> PostUniversalTransferDocumentAsync(IEdmProvider provider, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocument"/>
        EdmPostResult ReceiptUniversalTransferDocument(EdmAccount account, BuyerUniversalTransferDocument document);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocument"/>
        EdmPostResult ReceiptUniversalTransferDocument(string providerName, EdmAccount account, BuyerUniversalTransferDocument document);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocument"/>
        EdmPostResult ReceiptUniversalTransferDocument(EdmProviderName providerName, EdmAccount account, BuyerUniversalTransferDocument document);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocument"/>
        EdmPostResult ReceiptUniversalTransferDocument(IEdmProvider provider, EdmAccount account, BuyerUniversalTransferDocument document);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(string providerName, EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmProviderName providerName, EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.ReceiptUniversalTransferDocumentAsync"/>
        Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(IEdmProvider provider, EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentList"/>
        DocumentCollection GetIncomingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentList"/>
        DocumentCollection GetIncomingDocumentList(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentList"/>
        DocumentCollection GetIncomingDocumentList(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentList"/>
        DocumentCollection GetIncomingDocumentList(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentListAsync"/>
        Task<DocumentCollection> GetIncomingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentListAsync"/>
        Task<DocumentCollection> GetIncomingDocumentListAsync(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentListAsync"/>
        Task<DocumentCollection> GetIncomingDocumentListAsync(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentListAsync"/>
        Task<DocumentCollection> GetIncomingDocumentListAsync(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentList"/>
        DocumentCollection GetOutgoingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentList"/>
        DocumentCollection GetOutgoingDocumentList(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentList"/>
        DocumentCollection GetOutgoingDocumentList(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentList"/>
        DocumentCollection GetOutgoingDocumentList(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentListAsync"/>
        Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentListAsync"/>
        Task<DocumentCollection> GetOutgoingDocumentListAsync(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentListAsync"/>
        Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentListAsync"/>
        Task<DocumentCollection> GetOutgoingDocumentListAsync(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocument"/>
        byte[] GetIncomingDocument(EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocument"/>
        byte[] GetIncomingDocument(string providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocument"/>
        byte[] GetIncomingDocument(EdmProviderName providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocument"/>
        byte[] GetIncomingDocument(IEdmProvider provider, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentAsync"/>
        Task<byte[]> GetIncomingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentAsync"/>
        Task<byte[]> GetIncomingDocumentAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentAsync"/>
        Task<byte[]> GetIncomingDocumentAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingDocumentAsync"/>
        Task<byte[]> GetIncomingDocumentAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocument"/>
        byte[] GetOutgoingDocument(EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocument"/>
        byte[] GetOutgoingDocument(string providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocument"/>
        byte[] GetOutgoingDocument(EdmProviderName providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocument"/>
        byte[] GetOutgoingDocument(IEdmProvider provider, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentAsync"/>
        Task<byte[]> GetOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentAsync"/>
        Task<byte[]> GetOutgoingDocumentAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentAsync"/>
        Task<byte[]> GetOutgoingDocumentAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingDocumentAsync"/>
        Task<byte[]> GetOutgoingDocumentAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfo"/>
        SellerDocumentInfo GetIncomingSellerDocumentInfo(EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfo"/>
        SellerDocumentInfo GetIncomingSellerDocumentInfo(string providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfo"/>
        SellerDocumentInfo GetIncomingSellerDocumentInfo(EdmProviderName providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfo"/>
        SellerDocumentInfo GetIncomingSellerDocumentInfo(IEdmProvider provider, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetIncomingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfo"/>
        SellerDocumentInfo GetOutgoingSellerDocumentInfo(EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfo"/>
        SellerDocumentInfo GetOutgoingSellerDocumentInfo(string providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfo"/>
        SellerDocumentInfo GetOutgoingSellerDocumentInfo(EdmProviderName providerName, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfo"/>
        SellerDocumentInfo GetOutgoingSellerDocumentInfo(IEdmProvider provider, EdmAccount account, string documentId);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEdmProvider.GetOutgoingSellerDocumentInfoAsync"/>
        Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default);
    }
}
