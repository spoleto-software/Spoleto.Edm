using Spoleto.RestClient;
using Spoleto.TrueApi.Auth.Clients;

namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Методы, которые есть только у «ЭДО Лайт» (<see cref="ITrueApiEdmProvider"/>).
    /// </summary>
    public partial class TrueApiEdmProvider
    {
        /// <inheritdoc />
        public SignedDocumentInfo GetIncomingSignedDocumentInfo(EdmAccount account, string documentId)
            => GetIncomingSignedDocumentInfoAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public Task<SignedDocumentInfo> GetIncomingSignedDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetSignedDocumentInfoAsync(account, IncomingDocumentsPath, documentId, cancellationToken);

        /// <inheritdoc />
        public SignedDocumentInfo GetOutgoingSignedDocumentInfo(EdmAccount account, string documentId)
            => GetOutgoingSignedDocumentInfoAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public Task<SignedDocumentInfo> GetOutgoingSignedDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetSignedDocumentInfoAsync(account, OutgoingDocumentsPath, documentId, cancellationToken);

        /// <inheritdoc />
        public void SignOutgoingDocument(EdmAccount account, string documentId)
            => SignOutgoingDocumentAsync(account, documentId).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task SignOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
        {
            // Подписываем байты, полученные от сервиса, без перекодирования: подпись сверяется с файлом черновика.
            var content = await GetOutgoingDocumentAsync(account, documentId, cancellationToken).ConfigureAwait(false);
            var signature = SignDetached(account, content);

            var uri = CreateUri($"{EdoOutgoingDocumentsPath}/{Uri.EscapeDataString(documentId)}/signature");

            await ExecuteAsStringAsync(account, TrueApiRestClient.CreateRequest(RestHttpMethod.Post, uri, CreateSignatureContent(signature)), cancellationToken).ConfigureAwait(false);
        }
    }
}
