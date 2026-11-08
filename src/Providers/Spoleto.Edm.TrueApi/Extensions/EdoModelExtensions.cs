namespace Spoleto.Edm.Providers.TrueApi
{
    /// <summary>
    /// Перевод моделей списков «ЭДО Лайт» в модели Spoleto.Edm и обратно.
    /// </summary>
    /// <remarks>
    /// Коды статусов и типов совпадают, перечисления переводятся по числовому значению. Даты переносятся как есть.
    /// </remarks>
    internal static class EdoModelExtensions
    {
        private static readonly EdoDocumentType[] _allDocumentTypes = Enum.GetValues<EdoDocumentType>();

        /// <summary>
        /// В критериях все типы документов «ЭДО Лайт»: это то же, что без отбора по типу.
        /// </summary>
        private static bool HasAllDocumentTypes(DocumentCollectionSearchModel? x)
            => x?.Type is { Count: > 0 } && _allDocumentTypes.All(t => x.Type.Contains((DocumentType)(int)t));

        /// <summary>
        /// Критерии отбора для запросов True API. В True API статус и тип - одно значение, а в <see cref="DocumentCollectionSearchModel"/> - списки,
        /// поэтому на каждое сочетание статуса и типа - свой запрос.
        /// </summary>
        /// <remarks>
        /// Все типы сразу уходят без отбора по типу: один запрос вместо запроса на каждый тип.
        /// Документы других типов, например договоры с оператором ГИС МТ, отбрасывает <see cref="ToDocumentCollection"/>.
        /// </remarks>
        /// <returns>Критерии каждого запроса. Без критериев - один запрос без них.</returns>
        public static List<EdoDocumentSearchModel?> ToEdoSearchModels(this DocumentCollectionSearchModel? x)
        {
            if (x == null)
                return [null];

            var searchModel = new EdoDocumentSearchModel
            {
                Limit = x.Limit,
                Offset = x.Offset,
                CreatedFrom = x.CreatedFrom,
                CreatedTo = x.CreatedTo,
                PartnerInn = x.PartnerInn,
                SortBy = x.SortBy switch
                {
                    null => null,
                    SortField.created_at => EdoDocumentSortField.CreatedAt,
                    _ => throw new NotSupportedException($"Сортировка по {x.SortBy} не поддерживается: True API сортирует документы только по времени создания ({SortField.created_at}).")
                },
                Asc = x.Asc
            };

            List<EdoDocumentStatus?> statuses = x.Status is { Count: > 0 } ? x.Status.Distinct().Select(s => (EdoDocumentStatus?)(EdoDocumentStatus)(int)s).ToList() : [null];
            List<EdoDocumentType?> types = x.Type is { Count: > 0 } && !HasAllDocumentTypes(x) ? x.Type.Distinct().Select(t => (EdoDocumentType?)(EdoDocumentType)(int)t).ToList() : [null];

            return statuses.SelectMany(s => types.Select(t => (EdoDocumentSearchModel?)(searchModel with { Status = s, Type = t }))).ToList();
        }

        /// <summary>
        /// Ответы на запросы из <see cref="ToEdoSearchModels"/> одним списком.
        /// </summary>
        /// <remarks>
        /// Ответ на один запрос остаётся в порядке сервиса. Ответы на несколько запросов объединяются и сортируются заново по времени создания:
        /// каждый отсортирован сервисом сам по себе. Каждый запрос получает свою страницу из <see cref="DocumentCollectionSearchModel.Limit"/> документов,
        /// поэтому объединённая страница бывает длиннее. Следующая страница есть, если она есть хотя бы у одного запроса.
        /// </remarks>
        public static DocumentCollection ToDocumentCollection(this IReadOnlyList<EdoDocumentCollection> collections, DocumentCollectionSearchModel? searchModel)
        {
            IEnumerable<EdoDocumentGroup> groups = collections[0].Items;
            if (collections.Count > 1)
            {
                groups = collections.SelectMany(x => x.Items).DistinctBy(x => x.Id);
                groups = searchModel?.Asc == true ? groups.OrderBy(x => x.CreatedAt) : groups.OrderByDescending(x => x.CreatedAt);
            }

            // Все типы ушли без отбора по типу (ToEdoSearchModels): оставляем только известные типы.
            if (HasAllDocumentTypes(searchModel))
            {
                groups = groups.Where(x => Enum.IsDefined(x.Type));
            }

            return new DocumentCollection
            {
                Items = groups.Select(ToDocument).ToList(),
                HasNextPage = collections.Any(x => x.HasNextPage)
            };
        }

        private static Document ToDocument(this EdoDocumentGroup x) => new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt ?? default,
            Date = x.Date ?? default,
            Documents = x.Documents.Select(ToDocumentInfo).ToList(),
            GroupId = x.GroupId,
            Number = x.Number,
            // Контрагент: у входящих True API отдаёт отправителя, у исходящих - получателя.
            Sender = (x.Sender ?? x.Recipient)?.ToFirmInfo(),
            Status = (DocumentState)(int)x.Status,
            TotalPrice = x.TotalPrice ?? 0,
            TotalVatAmount = x.TotalVatAmount ?? 0,
            Type = (DocumentType)(int)x.Type,
            CreateTimestamp = x.CreateTimestamp ?? default,
            ExportTimestamp = x.ExportTimestamp ?? default
        };

        private static DocumentInfo ToDocumentInfo(this EdoDocumentInfo x) => new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt ?? default,
            Date = x.Date ?? default,
            Number = x.Number,
            ProcessedAt = x.ProcessedAt ?? default,
            Status = (DocumentState)(int)x.Status,
            TotalPrice = x.TotalPrice ?? 0,
            TotalVatAmount = x.TotalVatAmount ?? 0,
            Type = (DocumentType)(int)x.Type,
            IsTitleSigned = x.IsTitleSigned
        };

        private static FirmInfo ToFirmInfo(this EdoParticipant x) => new()
        {
            Id = x.Id ?? 0,
            ExtraId = x.ExtraId,
            Inn = x.Inn,
            Kpp = x.Kpp,
            Ogrn = x.Ogrn,
            // У ИП наименования может не быть, только ФИО.
            Name = x.Name ?? x.Fio?.ToString()
        };
    }
}
