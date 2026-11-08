using System.Collections.Specialized;
using Spoleto.Edm.Providers;
using Spoleto.Edm.Upd;

namespace Spoleto.Edm
{
    /// <summary>
    /// Работа с операторами ЭДО через зарегистрированных провайдеров.
    /// </summary>
    public class EdmService : IEdmService
    {
        private readonly IEdmProvider _defaultProvider;
        private readonly OrderedDictionary _providers;

        public EdmService(IEnumerable<IEdmProvider> providers, EdmServiceOptions options)
        {
            ArgumentNullException.ThrowIfNull(providers);
            ArgumentNullException.ThrowIfNull(options);

            if (!providers.Any())
                throw new ArgumentException("You have to specify at least one EDM provider, the EDM provider list is empty.", nameof(providers));

            // Validates if the options are valid:
            options.Validate();
            Options = options;

            // Inits the providers dictionary:
            _providers = new OrderedDictionary(providers.Count());
            foreach (var provider in providers)
            {
                _providers.Add(provider.Name, provider);
            }

            // Checks if the default EDM provider exists:
            _defaultProvider = GetProvider(options.DefaultProvider);
        }

        /// <summary>
        /// Настройки сервиса.
        /// </summary>
        public EdmServiceOptions Options { get; }

        /// <inheritdoc />
        public IEnumerable<IEdmProvider> Providers
        {
            get
            {
                foreach (IEdmProvider provider in _providers.Values)
                    yield return provider;
            }
        }

        /// <inheritdoc />
        public IEdmProvider DefaultProvider => _defaultProvider;

        /// <inheritdoc />
        public IEdmProvider GetProvider(string providerName)
        {
            ArgumentNullException.ThrowIfNull(providerName);

            return _providers[providerName] as IEdmProvider ?? throw new EdmProviderNotFoundException(providerName);
        }

        /// <inheritdoc />
        public IEdmProvider GetProvider(EdmProviderName providerName) => GetProvider(providerName.ToString());

        /// <inheritdoc />
        public EdmPostResult PostUniversalTransferDocument(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false)
            => PostUniversalTransferDocument(_defaultProvider, account, document, isDraft);

        /// <inheritdoc />
        public EdmPostResult PostUniversalTransferDocument(string providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false)
            => PostUniversalTransferDocument(GetProvider(providerName), account, document, isDraft);

        /// <inheritdoc />
        public EdmPostResult PostUniversalTransferDocument(EdmProviderName providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false)
            => PostUniversalTransferDocument(providerName.ToString(), account, document, isDraft);

        /// <inheritdoc />
        public EdmPostResult PostUniversalTransferDocument(IEdmProvider provider, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(document);

            return provider.PostUniversalTransferDocument(account, document, isDraft);
        }

        /// <inheritdoc />
        public Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default)
            => PostUniversalTransferDocumentAsync(_defaultProvider, account, document, isDraft, cancellationToken);

        /// <inheritdoc />
        public Task<EdmPostResult> PostUniversalTransferDocumentAsync(string providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default)
            => PostUniversalTransferDocumentAsync(GetProvider(providerName), account, document, isDraft, cancellationToken);

        /// <inheritdoc />
        public Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmProviderName providerName, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default)
            => PostUniversalTransferDocumentAsync(providerName.ToString(), account, document, isDraft, cancellationToken);

        /// <inheritdoc />
        public Task<EdmPostResult> PostUniversalTransferDocumentAsync(IEdmProvider provider, EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(document);

            return provider.PostUniversalTransferDocumentAsync(account, document, isDraft, cancellationToken);
        }

        /// <inheritdoc />
        public EdmPostResult ReceiptUniversalTransferDocument(EdmAccount account, BuyerUniversalTransferDocument document)
            => ReceiptUniversalTransferDocument(_defaultProvider, account, document);

        /// <inheritdoc />
        public EdmPostResult ReceiptUniversalTransferDocument(string providerName, EdmAccount account, BuyerUniversalTransferDocument document)
            => ReceiptUniversalTransferDocument(GetProvider(providerName), account, document);

        /// <inheritdoc />
        public EdmPostResult ReceiptUniversalTransferDocument(EdmProviderName providerName, EdmAccount account, BuyerUniversalTransferDocument document)
            => ReceiptUniversalTransferDocument(providerName.ToString(), account, document);

        /// <inheritdoc />
        public EdmPostResult ReceiptUniversalTransferDocument(IEdmProvider provider, EdmAccount account, BuyerUniversalTransferDocument document)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(document);

            return provider.ReceiptUniversalTransferDocument(account, document);
        }

        /// <inheritdoc />
        public Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default)
            => ReceiptUniversalTransferDocumentAsync(_defaultProvider, account, document, cancellationToken);

        /// <inheritdoc />
        public Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(string providerName, EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default)
            => ReceiptUniversalTransferDocumentAsync(GetProvider(providerName), account, document, cancellationToken);

        /// <inheritdoc />
        public Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmProviderName providerName, EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default)
            => ReceiptUniversalTransferDocumentAsync(providerName.ToString(), account, document, cancellationToken);

        /// <inheritdoc />
        public Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(IEdmProvider provider, EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(document);

            return provider.ReceiptUniversalTransferDocumentAsync(account, document, cancellationToken);
        }

        /// <inheritdoc />
        public DocumentCollection GetIncomingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetIncomingDocumentList(_defaultProvider, account, searchModel);

        /// <inheritdoc />
        public DocumentCollection GetIncomingDocumentList(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetIncomingDocumentList(GetProvider(providerName), account, searchModel);

        /// <inheritdoc />
        public DocumentCollection GetIncomingDocumentList(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetIncomingDocumentList(providerName.ToString(), account, searchModel);

        /// <inheritdoc />
        public DocumentCollection GetIncomingDocumentList(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);

            return provider.GetIncomingDocumentList(account, searchModel);
        }

        /// <inheritdoc />
        public Task<DocumentCollection> GetIncomingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetIncomingDocumentListAsync(_defaultProvider, account, searchModel, cancellationToken);

        /// <inheritdoc />
        public Task<DocumentCollection> GetIncomingDocumentListAsync(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetIncomingDocumentListAsync(GetProvider(providerName), account, searchModel, cancellationToken);

        /// <inheritdoc />
        public Task<DocumentCollection> GetIncomingDocumentListAsync(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetIncomingDocumentListAsync(providerName.ToString(), account, searchModel, cancellationToken);

        /// <inheritdoc />
        public Task<DocumentCollection> GetIncomingDocumentListAsync(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);

            return provider.GetIncomingDocumentListAsync(account, searchModel, cancellationToken);
        }

        /// <inheritdoc />
        public DocumentCollection GetOutgoingDocumentList(EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetOutgoingDocumentList(_defaultProvider, account, searchModel);

        /// <inheritdoc />
        public DocumentCollection GetOutgoingDocumentList(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetOutgoingDocumentList(GetProvider(providerName), account, searchModel);

        /// <inheritdoc />
        public DocumentCollection GetOutgoingDocumentList(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
            => GetOutgoingDocumentList(providerName.ToString(), account, searchModel);

        /// <inheritdoc />
        public DocumentCollection GetOutgoingDocumentList(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);

            return provider.GetOutgoingDocumentList(account, searchModel);
        }

        /// <inheritdoc />
        public Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetOutgoingDocumentListAsync(_defaultProvider, account, searchModel, cancellationToken);

        /// <inheritdoc />
        public Task<DocumentCollection> GetOutgoingDocumentListAsync(string providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetOutgoingDocumentListAsync(GetProvider(providerName), account, searchModel, cancellationToken);

        /// <inheritdoc />
        public Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmProviderName providerName, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetOutgoingDocumentListAsync(providerName.ToString(), account, searchModel, cancellationToken);

        /// <inheritdoc />
        public Task<DocumentCollection> GetOutgoingDocumentListAsync(IEdmProvider provider, EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);

            return provider.GetOutgoingDocumentListAsync(account, searchModel, cancellationToken);
        }

        /// <inheritdoc />
        public byte[] GetIncomingDocument(EdmAccount account, string documentId)
            => GetIncomingDocument(_defaultProvider, account, documentId);

        /// <inheritdoc />
        public byte[] GetIncomingDocument(string providerName, EdmAccount account, string documentId)
            => GetIncomingDocument(GetProvider(providerName), account, documentId);

        /// <inheritdoc />
        public byte[] GetIncomingDocument(EdmProviderName providerName, EdmAccount account, string documentId)
            => GetIncomingDocument(providerName.ToString(), account, documentId);

        /// <inheritdoc />
        public byte[] GetIncomingDocument(IEdmProvider provider, EdmAccount account, string documentId)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetIncomingDocument(account, documentId);
        }

        /// <inheritdoc />
        public Task<byte[]> GetIncomingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetIncomingDocumentAsync(_defaultProvider, account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<byte[]> GetIncomingDocumentAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetIncomingDocumentAsync(GetProvider(providerName), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<byte[]> GetIncomingDocumentAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetIncomingDocumentAsync(providerName.ToString(), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<byte[]> GetIncomingDocumentAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetIncomingDocumentAsync(account, documentId, cancellationToken);
        }

        /// <inheritdoc />
        public byte[] GetOutgoingDocument(EdmAccount account, string documentId)
            => GetOutgoingDocument(_defaultProvider, account, documentId);

        /// <inheritdoc />
        public byte[] GetOutgoingDocument(string providerName, EdmAccount account, string documentId)
            => GetOutgoingDocument(GetProvider(providerName), account, documentId);

        /// <inheritdoc />
        public byte[] GetOutgoingDocument(EdmProviderName providerName, EdmAccount account, string documentId)
            => GetOutgoingDocument(providerName.ToString(), account, documentId);

        /// <inheritdoc />
        public byte[] GetOutgoingDocument(IEdmProvider provider, EdmAccount account, string documentId)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetOutgoingDocument(account, documentId);
        }

        /// <inheritdoc />
        public Task<byte[]> GetOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetOutgoingDocumentAsync(_defaultProvider, account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<byte[]> GetOutgoingDocumentAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetOutgoingDocumentAsync(GetProvider(providerName), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<byte[]> GetOutgoingDocumentAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetOutgoingDocumentAsync(providerName.ToString(), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<byte[]> GetOutgoingDocumentAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetOutgoingDocumentAsync(account, documentId, cancellationToken);
        }

        /// <inheritdoc />
        public SellerDocumentInfo GetIncomingSellerDocumentInfo(EdmAccount account, string documentId)
            => GetIncomingSellerDocumentInfo(_defaultProvider, account, documentId);

        /// <inheritdoc />
        public SellerDocumentInfo GetIncomingSellerDocumentInfo(string providerName, EdmAccount account, string documentId)
            => GetIncomingSellerDocumentInfo(GetProvider(providerName), account, documentId);

        /// <inheritdoc />
        public SellerDocumentInfo GetIncomingSellerDocumentInfo(EdmProviderName providerName, EdmAccount account, string documentId)
            => GetIncomingSellerDocumentInfo(providerName.ToString(), account, documentId);

        /// <inheritdoc />
        public SellerDocumentInfo GetIncomingSellerDocumentInfo(IEdmProvider provider, EdmAccount account, string documentId)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetIncomingSellerDocumentInfo(account, documentId);
        }

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetIncomingSellerDocumentInfoAsync(_defaultProvider, account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetIncomingSellerDocumentInfoAsync(GetProvider(providerName), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetIncomingSellerDocumentInfoAsync(providerName.ToString(), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetIncomingSellerDocumentInfoAsync(account, documentId, cancellationToken);
        }

        /// <inheritdoc />
        public SellerDocumentInfo GetOutgoingSellerDocumentInfo(EdmAccount account, string documentId)
            => GetOutgoingSellerDocumentInfo(_defaultProvider, account, documentId);

        /// <inheritdoc />
        public SellerDocumentInfo GetOutgoingSellerDocumentInfo(string providerName, EdmAccount account, string documentId)
            => GetOutgoingSellerDocumentInfo(GetProvider(providerName), account, documentId);

        /// <inheritdoc />
        public SellerDocumentInfo GetOutgoingSellerDocumentInfo(EdmProviderName providerName, EdmAccount account, string documentId)
            => GetOutgoingSellerDocumentInfo(providerName.ToString(), account, documentId);

        /// <inheritdoc />
        public SellerDocumentInfo GetOutgoingSellerDocumentInfo(IEdmProvider provider, EdmAccount account, string documentId)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetOutgoingSellerDocumentInfo(account, documentId);
        }

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetOutgoingSellerDocumentInfoAsync(_defaultProvider, account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(string providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetOutgoingSellerDocumentInfoAsync(GetProvider(providerName), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmProviderName providerName, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetOutgoingSellerDocumentInfoAsync(providerName.ToString(), account, documentId, cancellationToken);

        /// <inheritdoc />
        public Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(IEdmProvider provider, EdmAccount account, string documentId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(account);
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            return provider.GetOutgoingSellerDocumentInfoAsync(account, documentId, cancellationToken);
        }
    }
}
