namespace Spoleto.Edm
{
    /// <summary>
    /// Документ не отправлен оператору ЭДО: исключение из <see cref="EdmPostResult.EnsureSuccess"/>.
    /// </summary>
    /// <remarks>
    /// Текст ошибки и исходное исключение - во <see cref="Exception.InnerException"/>, файл, который не принял оператор, - в <see cref="Result"/>.
    /// </remarks>
    [Serializable]
    public class EdmPostException : Exception
    {
        public EdmPostException(EdmPostResult result)
            : base(result.Exception?.Message ?? $"Оператор ЭДО не вернул идентификатор файла {result.FileId}.", result.Exception)
        {
            Result = result;
        }

        /// <summary>
        /// Результат отправки: файл и ошибка.
        /// </summary>
        public EdmPostResult Result { get; }
    }
}
