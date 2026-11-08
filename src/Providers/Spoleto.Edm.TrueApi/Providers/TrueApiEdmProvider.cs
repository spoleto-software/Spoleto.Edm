using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CIS.Cryptography;
using Spoleto.Common.Helpers;
using Spoleto.Edm.Helpers;
using Spoleto.Edm.Upd;
using Spoleto.RestClient;
using Spoleto.TrueApi.Auth.Clients;
using Spoleto.TrueApi.Auth.Providers;

namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Провайдер «ЭДО Лайт» ЦРПТ: методы раздела 9 True API.
    /// </summary>
    /// <remarks>
    /// Токен True API - на каждый <see cref="EdmAccount"/> свой: провайдер токенов хранит один токен, а каждый новый токен - это подпись в КриптоПро,
    /// поэтому провайдер стоит держать один на приложение. Свои провайдеры токенов можно передать через <c>tokenProviderFactory</c>.
    /// </remarks>
    public partial class TrueApiEdmProvider : EdmProviderBase, ITrueApiEdmProvider, IDisposable
    {
        /// <summary>
        /// Имя провайдера - код оператора ЭДО.
        /// </summary>
        public const string ProviderName = nameof(EdmProviderName.CRPT);

        private readonly TrueApiEdmOptions _options;
        private readonly HttpClient _httpClient;
        private readonly bool _disposeHttpClient;
        private readonly Func<EdmAccount, ITrueApiTokenProvider>? _tokenProviderFactory;
        private readonly ConcurrentDictionary<EdmAccount, ITrueApiTokenProvider> _tokenProviders = new();

        /// <summary>
        /// Провайдер со своим <see cref="HttpClient"/>.
        /// </summary>
        public TrueApiEdmProvider(TrueApiEdmOptions options) : this(options, CreateHttpClient(), true)
        {
        }

        /// <summary>
        /// Провайдер с переданным <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="options">Настройки провайдера.</param>
        /// <param name="httpClient">Клиент для запросов к True API.</param>
        /// <param name="disposeHttpClient">Освободить <paramref name="httpClient"/> вместе с провайдером.</param>
        /// <param name="tokenProviderFactory">Провайдер токенов для <see cref="EdmAccount"/>. Без него провайдер создаёт и хранит свой на каждый <see cref="EdmAccount"/>.</param>
        public TrueApiEdmProvider(TrueApiEdmOptions options, HttpClient httpClient, bool disposeHttpClient = false, Func<EdmAccount, ITrueApiTokenProvider>? tokenProviderFactory = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(httpClient);

            // Validates if the options are valid
            options.Validate();

            _options = options;
            _httpClient = httpClient;
            _disposeHttpClient = disposeHttpClient;
            _tokenProviderFactory = tokenProviderFactory;
        }

        /// <inheritdoc />
        public override string Name => ProviderName;

        #region IDisposable
        bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                // Провайдеры токенов из tokenProviderFactory не наши, освобождаем только свои.
                foreach (var tokenProvider in _tokenProviders.Values)
                {
                    (tokenProvider as IDisposable)?.Dispose();
                }

                if (_disposeHttpClient)
                {
                    _httpClient.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.ConfigureHttpClient();

            return httpClient;
        }

        private ITrueApiTokenProvider GetTokenProvider(EdmAccount account)
            => _tokenProviderFactory != null
            ? _tokenProviderFactory(account)
            : _tokenProviders.GetOrAdd(account, _ => new TrueApiTokenProvider(_httpClient, false));

        /// <summary>
        /// Клиент на вызов: токен берётся по <see cref="EdmAccount"/> вызова. Ошибки и повтор запроса после 401 - в <see cref="TrueApiRestClient"/>.
        /// </summary>
        private TrueApiRestClient CreateRestClient(EdmAccount account)
        {
            ArgumentNullException.ThrowIfNull(account);
            account.Validate();

            return new(_httpClient, new TrueApiAuthenticator(GetTokenProvider(account), new(_options.ServiceUrl, account.CertificateThumbprint, account.Inn, true)));
        }

        private Uri CreateUri(string path) => new(UriHelper.UrlCombine(_options.ServiceUrl, path));

        private async Task<string> ExecuteAsStringAsync(EdmAccount account, RestRequest request, CancellationToken cancellationToken)
        {
            using var restClient = CreateRestClient(account);
            var response = await restClient.ExecuteAsStringAsync(request, cancellationToken).ConfigureAwait(false);

            return response.Content;
        }

        private async Task<BinaryRestResponse> ExecuteAsBytesAsync(EdmAccount account, RestRequest request, CancellationToken cancellationToken)
        {
            using var restClient = CreateRestClient(account);

            return await restClient.ExecuteAsBytesAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> InvokeAsync<T>(EdmAccount account, RestHttpMethod method, Uri uri, HttpContent? content, CancellationToken cancellationToken)
        {
            var result = await ExecuteAsStringAsync(account, TrueApiRestClient.CreateRequest(method, uri, content), cancellationToken).ConfigureAwait(false);
            try
            {
                return JsonHelper.FromJson<T>(result)!;
            }
            catch (JsonException ex)
            {
                throw new JsonParsingException(result, ex);
            }
        }

        private async Task<byte[]> InvokeFileAsync(EdmAccount account, RestHttpMethod method, Uri uri, CancellationToken cancellationToken)
        {
            var response = await ExecuteAsBytesAsync(account, TrueApiRestClient.CreateRequest(method, uri, isFileResponse: true), cancellationToken).ConfigureAwait(false);

            return response.Content;
        }

        private const string IncomingDocumentsPath = "/api/v3/true-api/elk/incoming-documents";

        private const string OutgoingDocumentsPath = "/api/v3/true-api/elk/outgoing-documents";

        // Подписание исходящих документов и титул покупателя - в разделе edo, а списки и файлы - в elk.
        private const string EdoOutgoingDocumentsPath = "/api/v3/true-api/edo/outgoing-documents";

        private const string EdoIncomingDocumentsPath = "/api/v3/true-api/edo/incoming-documents";

        // Загрузка УПДи - отдельный метод.
        private const string UpdiPath = "/xml/updi";

        // Титул покупателя к УПД и к УПДи - разные методы.
        private const string UpdTitlePath = "/xml/upd/title/970";

        private const string UpdiTitlePath = "/xml/updi/title/970";

        private const string Base64ContentEncoding = "base64";

        private const string XmlMediaType = "text/xml";

        private const string ContentDispositionHeaderName = "Content-Disposition";

        private async Task<DocumentCollection> GetDocumentListAsync(EdmAccount account, string documentsPath, DocumentCollectionSearchModel? searchModel, CancellationToken cancellationToken)
        {
            // Критерии переводим до запроса: неподдерживаемая сортировка не должна стоить токена.
            var edoSearchModels = searchModel.ToEdoSearchModels();

            var collections = new List<EdoDocumentCollection>(edoSearchModels.Count);
            foreach (var edoSearchModel in edoSearchModels)
            {
                collections.Add(await GetEdoDocumentListAsync(account, documentsPath, edoSearchModel, cancellationToken).ConfigureAwait(false));
            }

            return collections.ToDocumentCollection(searchModel);
        }

        private async Task<EdoDocumentCollection> GetEdoDocumentListAsync(EdmAccount account, string documentsPath, EdoDocumentSearchModel? searchModel, CancellationToken cancellationToken)
        {
            var path = documentsPath;
            if (searchModel != null)
            {
                var queryString = HttpHelper.ToQueryString(searchModel);
                if (queryString.Length > 0)
                {
                    path += $"?{queryString}";
                }
            }

            return await InvokeAsync<EdoDocumentCollection>(account, RestHttpMethod.Get, CreateUri(path), null, cancellationToken).ConfigureAwait(false);
        }

        private async Task<byte[]> GetDocumentAsync(EdmAccount account, string documentsPath, string documentId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            var uri = CreateUri($"{documentsPath}/{Uri.EscapeDataString(documentId)}/content");

            return await InvokeFileAsync(account, RestHttpMethod.Get, uri, cancellationToken).ConfigureAwait(false);
        }

        private async Task<SignedDocumentInfo> GetSignedDocumentInfoAsync(EdmAccount account, string documentsPath, string documentId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(documentId);

            var uri = CreateUri($"{documentsPath}/{Uri.EscapeDataString(documentId)}");

            var response = await ExecuteAsBytesAsync(account, TrueApiRestClient.CreateRequest(RestHttpMethod.Get, uri, isFileResponse: true), cancellationToken).ConfigureAwait(false);

            ContentDispositionHeaderValue? contentDisposition = null;
            if (response.Headers.TryGetValue(ContentDispositionHeaderName, out var values))
            {
                ContentDispositionHeaderValue.TryParse(values.FirstOrDefault(), out contentDisposition);
            }

            var archiveFileName = contentDisposition?.FileNameStar ?? contentDisposition?.FileName?.Trim('"');

            using var archive = new System.IO.Compression.ZipArchive(new MemoryStream(response.Content), System.IO.Compression.ZipArchiveMode.Read);

            return EdoArchiveHelper.ReadSignedDocument(archive, archiveFileName);
        }

        private async Task<SellerDocumentInfo> GetSellerDocumentInfoAsync(EdmAccount account, string documentsPath, string documentId, CancellationToken cancellationToken)
        {
            var document = await GetSignedDocumentInfoAsync(account, documentsPath, documentId, cancellationToken).ConfigureAwait(false);

            return SellerDocumentHelper.ReadSellerDocumentInfo(document);
        }

        /// <summary>
        /// Открепленная подпись файла сертификатом <see cref="EdmAccount.CertificateThumbprint"/>.
        /// </summary>
        /// <param name="account">От чьего имени вызов: отпечаток сертификата.</param>
        /// <param name="content">Файл документа как есть, байт в байт.</param>
        /// <returns>Подпись CMS в Base64.</returns>
        protected virtual string SignDetached(EdmAccount account, byte[] content)
            => CryptographyHelper.SignBase64Data(Convert.ToBase64String(content), detached: true, thumbprint: account.CertificateThumbprint);

        private static StringContent CreateSignatureContent(string signature)
        {
            var content = new StringContent(signature, Encoding.UTF8, ContentTypes.TextPlain);
            content.Headers.ContentEncoding.Add(Base64ContentEncoding);

            return content;
        }

        /// <summary>
        /// Тело загрузки файла: файл в поле content, для титула покупателя - идентификатор документа продавца в поле doc_id
        /// и, если документ не черновик, подпись в поле signature.
        /// </summary>
        private static MultipartFormDataContent CreateDocumentContent(string fileId, byte[] rawContent, string? signature, string? documentId = null)
        {
            var fileContent = new ByteArrayContent(rawContent);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(XmlMediaType);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "\"content\"", FileName = $"\"{fileId}.xml\"" };

            var content = new MultipartFormDataContent { fileContent };
            if (documentId != null)
            {
                content.Add(new StringContent(documentId, Encoding.UTF8, ContentTypes.TextPlain), "doc_id");
            }

            if (signature != null)
            {
                content.Add(CreateSignatureContent(signature), "signature");
            }

            return content;
        }

        /// <summary>
        /// Подпись и загрузка сформированного файла. Ошибка не бросается, а возвращается в <see cref="EdmPostResult.Exception"/> вместе с файлом.
        /// </summary>
        /// <remarks>
        /// Отмена вызывающим бросается: это не ошибка отправки. Таймаут <see cref="HttpClient"/> - ошибка отправки.
        /// </remarks>
        private static async Task<EdmPostResult> PostFileAsync(string fileId, string content, byte[] rawContent, Func<Task<string?>> post, string noIdMessage, CancellationToken cancellationToken)
        {
            try
            {
                var id = await post().ConfigureAwait(false);

                return new EdmPostResult
                {
                    Id = id,
                    FileId = fileId,
                    Content = content,
                    RawContent = rawContent,
                    Exception = string.IsNullOrEmpty(id) ? new InvalidDataException(noIdMessage) : null
                };
            }
            catch (Exception ex) when (!(ex is OperationCanceledException && cancellationToken.IsCancellationRequested))
            {
                return new EdmPostResult { FileId = fileId, Content = content, RawContent = rawContent, Exception = ex };
            }
        }

        /// <inheritdoc />
        public override Task<DocumentCollection> GetIncomingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetDocumentListAsync(account, IncomingDocumentsPath, searchModel, cancellationToken);

        /// <inheritdoc />
        public override Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
            => GetDocumentListAsync(account, OutgoingDocumentsPath, searchModel, cancellationToken);

        /// <inheritdoc />
        /// <remarks>
        /// XML для УПД, УКД и квитанций (для УПД - windows-1251), PDF для договоров с оператором ГИС МТ.
        /// </remarks>
        public override Task<byte[]> GetIncomingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetDocumentAsync(account, IncomingDocumentsPath, documentId, cancellationToken);

        /// <inheritdoc />
        public override Task<byte[]> GetOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetDocumentAsync(account, OutgoingDocumentsPath, documentId, cancellationToken);

        /// <inheritdoc />
        /// <remarks>
        /// Скачивает файл с подписью, как <see cref="GetIncomingSignedDocumentInfoAsync"/>, и разбирает XML формата 970.
        /// </remarks>
        public override Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetSellerDocumentInfoAsync(account, IncomingDocumentsPath, documentId, cancellationToken);

        /// <inheritdoc />
        /// <remarks>
        /// Скачивает файл с подписью, как <see cref="GetOutgoingSignedDocumentInfoAsync"/>, и разбирает XML формата 970.
        /// </remarks>
        public override Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
            => GetSellerDocumentInfoAsync(account, OutgoingDocumentsPath, documentId, cancellationToken);

        /// <inheritdoc />
        /// <remarks>
        /// Если задан <see cref="SellerUniversalTransferDocument.RevisionNumber"/>, документ загружается как УПДи.
        /// Черновик не подписывается и остаётся у отправителя, его потом отправляет <see cref="SignOutgoingDocumentAsync"/>.
        /// </remarks>
        public override Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(document);
            account.Validate();

            var fileId = string.IsNullOrEmpty(document.FileId) ? UniversalTransferDocumentXmlHelper.GetFileId(document) : document.FileId;
            var content = UniversalTransferDocumentXmlHelper.CreateXml(document, fileId);
            var rawContent = UniversalTransferDocumentXmlHelper.FileEncoding.GetBytes(content);

            return PostFileAsync(fileId, content, rawContent, async () =>
            {
                // Без подписи документ остаётся черновиком, с подписью сразу уходит получателю.
                var signature = isDraft ? null : SignDetached(account, rawContent);

                var path = document.IsUPDi ? EdoOutgoingDocumentsPath + UpdiPath : EdoOutgoingDocumentsPath;

                var result = await InvokeAsync<EdoIdResult>(account, RestHttpMethod.Post, CreateUri(path), CreateDocumentContent(fileId, rawContent, signature), cancellationToken).ConfigureAwait(false);

                return result?.Id;
            }, $"«ЭДО Лайт» не вернул идентификатор загруженного файла {fileId}.", cancellationToken);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Скачивает файл продавца <see cref="BuyerUniversalTransferDocument.DocumentId"/>, как <see cref="GetIncomingSellerDocumentInfoAsync"/>:
        /// из него берутся реквизиты документа, подпись продавца, отправитель и получатель, а по наличию исправления выбирается метод для УПД или УПДи.
        /// Ошибка скачивания бросается: файла покупателя ещё нет.
        /// </remarks>
        public override async Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(document);
            ArgumentException.ThrowIfNullOrEmpty(document.DocumentId);

            // Информация покупателя повторяет реквизиты и подпись файла продавца, а её имя - участников и признаки из имени файла продавца.
            var sellerDocumentInfo = await GetIncomingSellerDocumentInfoAsync(account, document.DocumentId, cancellationToken).ConfigureAwait(false);

            var fileId = string.IsNullOrEmpty(document.FileId) ? UniversalTransferDocumentXmlHelper.GetFileId(document, sellerDocumentInfo) : document.FileId;
            var content = UniversalTransferDocumentXmlHelper.CreateXml(document, sellerDocumentInfo, fileId);
            var rawContent = UniversalTransferDocumentXmlHelper.FileEncoding.GetBytes(content);

            return await PostFileAsync(fileId, content, rawContent, async () =>
            {
                // Титул покупателя загружается только с подписью: черновиков у него нет.
                var signature = SignDetached(account, rawContent);

                var path = EdoIncomingDocumentsPath + (sellerDocumentInfo.IsUPDi ? UpdiTitlePath : UpdTitlePath);

                var result = await InvokeAsync<EdoIdResult>(account, RestHttpMethod.Post, CreateUri(path), CreateDocumentContent(fileId, rawContent, signature, document.DocumentId), cancellationToken).ConfigureAwait(false);

                // Описание метода 9.2.9 обещает event_id, а тестовый стенд 29.10.2026 вернул только id.
                return result?.EventId ?? result?.Id;
            }, $"«ЭДО Лайт» не вернул идентификатор загруженного титула покупателя {fileId}.", cancellationToken).ConfigureAwait(false);
        }
    }
}
