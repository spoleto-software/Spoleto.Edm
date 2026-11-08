using Microsoft.Extensions.Configuration;

namespace Spoleto.Edm.Tests
{
    internal static class ConfigurationHelper
    {
        private static readonly IConfigurationRoot _config;

        static ConfigurationHelper()
        {
            _config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true)
               .AddUserSecrets("f71a6813-0278-4906-bc0f-460521647e8f")
               .Build();
        }

        public static IConfigurationRoot Configuration => _config;

        public static TestOption GetTestOption()
        {
            var option = _config.GetSection(nameof(TestOption)).Get<TestOption>()!;

            return option;
        }
    }
}
