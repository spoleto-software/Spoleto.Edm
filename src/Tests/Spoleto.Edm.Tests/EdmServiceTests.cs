using Microsoft.Extensions.DependencyInjection;
using Spoleto.Edm.Extensions;
using Spoleto.Edm.Extensions.TrueApi;
using Spoleto.Edm.Providers;
using Spoleto.Edm.Providers.TrueApi;
using Spoleto.Edm.Upd;

namespace Spoleto.Edm.Tests
{
    /// <summary>
    /// <see cref="EdmService"/>: выбор провайдера, регистрация в DI и через <see cref="EdmServiceFactory"/>.
    /// </summary>
    public class EdmServiceTests
    {
        private static readonly EdmAccount Account = new("thumbprint", "7777777777");

        [Test]
        public void Call_WithoutProviderName_GoesToDefaultProvider()
        {
            // Arrange
            var first = new FakeEdmProvider("First");
            var second = new FakeEdmProvider("Second");
            var service = new EdmService([first, second], new EdmServiceOptions { DefaultProvider = "Second" });

            // Act
            service.GetIncomingDocumentList(Account);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(service.DefaultProvider, Is.SameAs(second));
                Assert.That(first.Calls, Is.Empty);
                Assert.That(second.Calls, Is.EqualTo(new[] { $"{nameof(IEdmProvider.GetIncomingDocumentListAsync)} {Account}" }));
            });
        }

        [Test]
        public void Call_WithProviderName_GoesToThatProvider()
        {
            // Arrange
            var first = new FakeEdmProvider("First");
            var second = new FakeEdmProvider("Second");
            var service = new EdmService([first, second], new EdmServiceOptions { DefaultProvider = "Second" });

            // Act
            service.GetOutgoingDocument("First", Account, "id");
            service.GetOutgoingDocumentList(second, Account);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(first.Calls, Is.EqualTo(new[] { $"{nameof(IEdmProvider.GetOutgoingDocumentAsync)} {Account}" }));
                Assert.That(second.Calls, Is.EqualTo(new[] { $"{nameof(IEdmProvider.GetOutgoingDocumentListAsync)} {Account}" }));
            });
        }

        [Test]
        public void GetProvider_UnknownName_Throws()
        {
            // Arrange
            var service = new EdmService([new FakeEdmProvider("First")], new EdmServiceOptions { DefaultProvider = "First" });

            // Act
            var ex = Assert.Throws<EdmProviderNotFoundException>(() => service.GetProvider("CRPT_Jwt"));

            // Assert
            Assert.That(ex!.ProviderName, Is.EqualTo("CRPT_Jwt"));
        }

        [Test]
        public void Create_DefaultProviderNotRegistered_Throws()
        {
            Assert.Throws<EdmProviderNotFoundException>(() => new EdmService([new FakeEdmProvider("First")], new EdmServiceOptions { DefaultProvider = "Second" }));
        }

        [Test]
        public void AddEdm_AddTrueApi_ResolvesOneProviderForBothInterfaces()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddEdm(TrueApiEdmProvider.ProviderName)
                .AddTrueApi(TrueApiEdmOptions.Sandbox.ServiceUrl);

            // Act
            using var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
            var service = serviceProvider.GetRequiredService<IEdmService>();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(service.DefaultProvider, Is.TypeOf<TrueApiEdmProvider>());
                Assert.That(service.GetProvider(EdmProviderName.CRPT), Is.SameAs(service.DefaultProvider));
                Assert.That(serviceProvider.GetRequiredService<ITrueApiEdmProvider>(), Is.SameAs(service.DefaultProvider));
                Assert.That(serviceProvider.GetRequiredService<IEdmService>(), Is.SameAs(service), "Сервис и провайдер - одиночки: провайдер хранит токены.");
            });
        }

        [Test]
        public void AddTrueApi_ProviderInstance_IsNotDisposedByContainer()
        {
            // Arrange
            using var provider = new TrueApiEdmProvider(TrueApiEdmOptions.Sandbox);
            var services = new ServiceCollection();
            services.AddEdm(TrueApiEdmProvider.ProviderName).AddTrueApi(provider);

            // Act
            using (var serviceProvider = services.BuildServiceProvider())
            {
                Assert.That(serviceProvider.GetRequiredService<IEdmService>().DefaultProvider, Is.SameAs(provider));
            }

            // Assert
            var disposed = typeof(TrueApiEdmProvider).GetField("_disposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            Assert.That(disposed.GetValue(provider), Is.False, "Готовый провайдер освобождает тот, кто его создал.");
        }

        [Test]
        public void AddTrueApi_Options_ProviderDisposedWithContainer()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddEdm(TrueApiEdmProvider.ProviderName).AddTrueApi(TrueApiEdmOptions.Sandbox.ServiceUrl);
            TrueApiEdmProvider provider;

            // Act
            using (var serviceProvider = services.BuildServiceProvider())
            {
                provider = serviceProvider.GetRequiredService<TrueApiEdmProvider>();
            }

            // Assert
            var disposed = typeof(TrueApiEdmProvider).GetField("_disposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            Assert.That(disposed.GetValue(provider), Is.True, "Провайдер, созданный контейнером, освобождает контейнер.");
        }

        [Test]
        public void Factory_AddTrueApi_BuildsService()
        {
            // Act
            var service = new EdmServiceFactory()
                .WithOptions(x => x.DefaultProvider = TrueApiEdmProvider.ProviderName)
                .AddTrueApi(TrueApiEdmOptions.Sandbox.ServiceUrl)
                .Build();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(service.DefaultProvider.Name, Is.EqualTo("CRPT"));
                Assert.That(service.Providers.Single(), Is.SameAs(service.DefaultProvider));
            });
        }

        [Test]
        public void PostResult_WithoutId_IsNotSuccess()
        {
            // Arrange
            var result = new EdmPostResult { FileId = "ON_NSCHFDOPPR_1", Content = "<Файл/>", RawContent = [] };

            // Act
            var ex = Assert.Throws<EdmPostException>(() => result.EnsureSuccess());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(ex!.Message, Does.Contain("ON_NSCHFDOPPR_1"));
                Assert.That(ex.InnerException, Is.Null);
            });
        }

        /// <summary>
        /// Провайдер, который запоминает вызовы и в ответ на отправку возвращает своё имя.
        /// </summary>
        private sealed class FakeEdmProvider(string name) : EdmProviderBase
        {
            public override string Name => name;

            public List<string> Calls { get; } = [];

            private Task<T> Call<T>(string method, EdmAccount account, T result)
            {
                Calls.Add($"{method} {account}");

                return Task.FromResult(result);
            }

            private EdmPostResult PostResult => new() { Id = name, FileId = "file", Content = "<Файл/>", RawContent = [] };

            public override Task<EdmPostResult> PostUniversalTransferDocumentAsync(EdmAccount account, SellerUniversalTransferDocument document, bool isDraft = false, CancellationToken cancellationToken = default)
                => Call(nameof(PostUniversalTransferDocumentAsync), account, PostResult);

            public override Task<EdmPostResult> ReceiptUniversalTransferDocumentAsync(EdmAccount account, BuyerUniversalTransferDocument document, CancellationToken cancellationToken = default)
                => Call(nameof(ReceiptUniversalTransferDocumentAsync), account, PostResult);

            public override Task<DocumentCollection> GetIncomingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
                => Call(nameof(GetIncomingDocumentListAsync), account, new DocumentCollection());

            public override Task<DocumentCollection> GetOutgoingDocumentListAsync(EdmAccount account, DocumentCollectionSearchModel? searchModel = null, CancellationToken cancellationToken = default)
                => Call(nameof(GetOutgoingDocumentListAsync), account, new DocumentCollection());

            public override Task<byte[]> GetIncomingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
                => Call(nameof(GetIncomingDocumentAsync), account, Array.Empty<byte>());

            public override Task<byte[]> GetOutgoingDocumentAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
                => Call(nameof(GetOutgoingDocumentAsync), account, Array.Empty<byte>());

            public override Task<SellerDocumentInfo> GetIncomingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
                => throw new NotSupportedException();

            public override Task<SellerDocumentInfo> GetOutgoingSellerDocumentInfoAsync(EdmAccount account, string documentId, CancellationToken cancellationToken = default)
                => throw new NotSupportedException();
        }
    }
}
