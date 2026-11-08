using Spoleto.Edm.Upd;

namespace Spoleto.Edm.Providers
{
    /// <summary>
    /// Основа провайдера ЭДО: синхронные методы вызывают асинхронные.
    /// </summary>
    public abstract class EdmProviderBase : IEdmProvider
    {
        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public virtual EdmPostResult PostUniversalTransferDocument(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false)
            => PostUniversalTransferDocumentAsync(account, document, isDraft).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual EdmPostResult ReceiptUniversalTransferDocument(EdmAccount account, BuyerUniversalTransferDocument document)
            => ReceiptUniversalTransferDocumentAsync(account, document).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual DocumentCollection GetIncomingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetIncomingDocumentListAsync(account, searchModel).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<DocumentCollection> GetIncomingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual DocumentCollection GetOutgoingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetOutgoingDocumentListAsync(account, searchModel).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual byte[] GetIncomingDocument(EdmAccount account, string documentId)
            => GetIncomingDocumentAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<byte[]> GetIncomingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual byte[] GetOutgoingDocument(EdmAccount account, string documentId)
            => GetOutgoingDocumentAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<byte[]> GetOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual SellerDocumentInfo GetIncomingSellerDocumentInfo(EdmAccount account, string documentId)
            => GetIncomingSellerDocumentInfoAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual SellerDocumentInfo GetOutgoingSellerDocumentInfo(EdmAccount account, string documentId)
            => GetOutgoingSellerDocumentInfoAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public abstract Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default);

        public override string ToString() => Name;
    }
}
