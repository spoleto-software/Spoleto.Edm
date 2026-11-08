using System.Diagnostics.CodeAnalysis;

namespace Spoleto.Edm
{
    /// <summary>
    /// Результат отправки документа оператору ЭДО: сформированный файл и что с ним стало.
    /// </summary>
    /// <remarks>
    /// Ошибка отправки сформированного файла (подпись, сеть, отказ оператора) не бросается, а возвращается в <see cref="Exception"/>
    /// вместе с файлом: XML нужен и при неудаче, например, для журнала. Чтобы получить исключение, вызовите <see cref="EnsureSuccess"/>.
    /// <para>
    /// Ошибка до формирования файла - неверная модель документа, недоступный файл продавца для титула покупателя - бросается: файла ещё нет.
    /// </para>
    /// </remarks>
    public record EdmPostResult
    {
        /// <summary>
        /// Идентификатор, который вернул оператор. <see langword="null"/>, если документ не отправлен.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// Идентификатор файла (ИдФайл), он же имя файла без расширения.
        /// </summary>
        public required string FileId { get; init; }

        /// <summary>
        /// XML файла.
        /// </summary>
        public required string Content { get; init; }

        /// <summary>
        /// Файл байт в байт, в кодировке из заголовка XML (для УПД - windows-1251). Подпись делается по этим байтам.
        /// </summary>
        public required byte[] RawContent { get; init; }

        /// <summary>
        /// Ошибка отправки. <see langword="null"/>, если документ отправлен.
        /// </summary>
        public Exception? Exception { get; init; }

        /// <summary>
        /// Документ отправлен: ошибки нет, и оператор вернул идентификатор.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Id))]
        public bool Success => Exception == null && !string.IsNullOrEmpty(Id);

        /// <summary>
        /// Результат, если документ отправлен, иначе - <see cref="EdmPostException"/> с этим результатом.
        /// </summary>
        /// <exception cref="EdmPostException">Документ не отправлен.</exception>
        public EdmPostResult EnsureSuccess() => Success ? this : throw new EdmPostException(this);

        public override string ToString() => Success ? $"{Id} ({FileId})" : $"{FileId}: {Exception?.Message}";
    }
}
