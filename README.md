# Spoleto.Edm

**Spoleto.Edm** is a library to work with electronic document management (EDM, ЭДО) operators through one interface, `IEdmProvider`, in the same way as [Spoleto.SMS](https://github.com/spoleto-software/Spoleto.SMS) and [Spoleto.Delivery](https://github.com/spoleto-software/Spoleto.Delivery).

**Spoleto.Edm.TrueApi** is a provider for «ЭДО Лайт» of CRPT (ЦРПТ), section 9 of [True API](https://docs.crpt.ru/gismt/True_API/).

**Spoleto.Edm.Extensions** and **Spoleto.Edm.Extensions.TrueApi** register them in Microsoft DependencyInjection.

## Installation

```bash
Install-Package Spoleto.Edm.TrueApi
```

```bash
Install-Package Spoleto.Edm.Extensions.TrueApi
```

## Устройство

- **Ядро (Spoleto.Edm):** интерфейс `IEdmProvider`, основа `EdmProviderBase`, сервис `IEdmService`, модели списков и статусов, модели УПД формата 970 (`Spoleto.Edm.Upd`), генератор и разбор XML.
  - Формат УПД задаёт ФНС (приказ № ЕД-7-26/970@), он один для всех операторов, поэтому модели и XML живут в ядре.
  - Провайдер только отправляет файл и читает ответы оператора.
- **Настройки оператора** (адрес сервиса) провайдер получает в конструкторе.
- **От чьего имени вызов** - `EdmAccount` (отпечаток сертификата и ИНН) - передаётся в каждый метод: организация и сертификат меняются от вызова к вызову.
- **Модели и коды** - как в CIS.EDM: имена методов, `DocumentCollectionSearchModel`, `DocumentState`, `DocumentType`. Коды статусов и типов совпадают с «ЭДО Лайт».

## Usage

### Регистрация

```csharp
services.AddEdm(TrueApiEdmProvider.ProviderName)
    .AddTrueApi(options => options.ServiceUrl = "https://markirovka.crpt.ru");

// Или готовый провайдер: свой HttpClient и провайдер токенов на EdmAccount.
var provider = new TrueApiEdmProvider(TrueApiEdmOptions.Production, httpClient, false,
    account => tokenProviders.Get(account.CertificateThumbprint));
services.AddEdm(TrueApiEdmProvider.ProviderName).AddTrueApi(provider);

// Без DI:
var edmService = new EdmServiceFactory()
    .WithOptions(x => x.DefaultProvider = TrueApiEdmProvider.ProviderName)
    .AddTrueApi("https://markirovka.crpt.ru")
    .Build();
```

- Имя провайдера «ЭДО Лайт» - `CRPT` (`EdmProviderName.CRPT`), как код оператора ЭДО организации.
- Провайдер хранит токен True API на каждый `EdmAccount`, а каждый новый токен - это подпись в КриптоПро.

### Вызовы

```csharp
var account = new EdmAccount(certificateThumbprint, firmInn);
var provider = edmService.GetProvider(firm.EdmOperatorName); // или edmService.PostUniversalTransferDocument(name, account, document)

var result = provider.PostUniversalTransferDocument(account, document);
if (!result.Success)
{
    logger.Log(result.Content);           // XML, который не принял оператор
    throw new Exception("Ошибка при отправке ЭДО.", result.Exception);
}

var list = provider.GetIncomingDocumentList(account, new DocumentCollectionSearchModel
{
    CreatedFrom = DateTime.Today.AddDays(-1),
    Status = [DocumentState.DeliveredSignatureRequired, DocumentState.ReviewedSignatureRequired]
});

var sellerFileId = provider.GetIncomingSellerDocumentInfo(account, documentId).FileId;
```

- У всех методов есть асинхронные варианты с `CancellationToken`.
- Методы, которые есть только у «ЭДО Лайт» - подписание черновика, файл с подписью, - в `ITrueApiEdmProvider`.

### Отправка и ошибки

- Отправка УПД и титула покупателя возвращает `EdmPostResult`: `Id`, `FileId`, `Content` (XML), `RawContent` (байты), `Exception`.
  - Ошибка отправки сформированного файла - подпись, сеть, отказ оператора - не бросается, а попадает в `Exception` вместе с XML.
  - `Success` - документ отправлен; `EnsureSuccess()` бросает `EdmPostException` с этим результатом.
  - Ошибка до формирования файла (неверная модель, не скачался файл продавца для титула) бросается: файла ещё нет. Отмена через `CancellationToken` тоже бросается.
- Остальные методы бросают ошибки. Ошибка сервиса - `HttpRequestException` с кодом ответа в `StatusCode` и текстом ошибки сервиса.
- На 401 провайдер сбрасывает токен и повторяет запрос с новым один раз.

### Списки «ЭДО Лайт»

- В True API статус и тип в запросе - одно значение. Поэтому на каждое сочетание статусов и типов из `DocumentCollectionSearchModel` уходит свой запрос, а ответы объединяются и сортируются заново по времени создания.
- Если в `Type` все типы, отбора по типу нет: уходит один запрос, документы неизвестных типов (договоры с оператором ГИС МТ) отбрасываются.
- Сортировка - только `created_at`.
- Даты - местное время: в ответе, и у границ `CreatedFrom`/`CreatedTo` с `DateTimeKind.Unspecified` или `Local`.

## Переход с CIS.EDM и Spoleto.TrueApi.Edo

| Было | Стало |
|---|---|
| `IEdmProvider` / `IEdmProvider<T>` (CIS.EDM) | `IEdmProvider` (Spoleto.Edm) |
| `IEdmOption` в каждом вызове, `OptionType` | `EdmAccount` в вызове, `TrueApiEdmOptions` в конструкторе |
| `ResultInfo` | `EdmPostResult`: те же `Id`, `Content`, `Exception` и ещё `FileId`, `RawContent`, `Success` |
| Модели CIS.EDM V5_03, модели `Edo*` Spoleto.TrueApi.Edo | Модели `Spoleto.Edm.Upd`: имена как в V5_03, без префикса `Edo` |
| `TrueApiCRPTProvider` (CIS.EDM.CRPT) | `TrueApiEdmProvider` (Spoleto.Edm.TrueApi) |
| `CRPTOption.AuthUrl` - адрес True API | `TrueApiEdmOptions.ServiceUrl` |

## Тесты

- `TrueApiEdmProviderStubTests` и `EdmServiceTests` работают без стенда.
- `TrueApiEdmProviderTests` вызывают стенд True API: нужны КриптоПро, сертификат и раздел `TestOption` (`CertificateThumbprint`, `ServiceUrl`, `Inn`) в секретах пользователя. UserSecretsId - тот же, что у тестов Spoleto.TrueApi.
- Отправка УПД, подписание черновика и титул покупателя меняют документы на стенде и запускаются только явно.
