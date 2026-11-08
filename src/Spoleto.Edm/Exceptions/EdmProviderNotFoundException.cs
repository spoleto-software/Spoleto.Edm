namespace Spoleto.Edm
{
    /// <summary>
    /// Провайдер ЭДО с таким именем не зарегистрирован.
    /// </summary>
    [Serializable]
    public class EdmProviderNotFoundException : Exception
    {
        public EdmProviderNotFoundException(string providerName)
            : base($"Провайдер ЭДО <{providerName}> не найден: его нужно добавить в {nameof(EdmServiceFactory)} или зарегистрировать в DI.")
        {
            ProviderName = providerName;
        }

        /// <summary>
        /// Имя провайдера, которого нет.
        /// </summary>
        public string ProviderName { get; }
    }
}
