using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Notes.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("🔍 Начало работы DesignTimeDbContextFactory...");

            // Логируем входные аргументы
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("⚠️ Внимание: Аргументы не переданы. Используем 'pgsql' по умолчанию.");
            }
            else
            {
                Console.WriteLine($"✅ Аргументы: {string.Join(", ", args)}");
            }

            // Используем "pgsql" по умолчанию, если аргументы пустые
            var provider = (args != null && args.Length > 0) ? args[0].ToLower() : "pgsql";
            Console.WriteLine($"ℹ️ Выбранный провайдер: {provider}");

            // Определяем путь к конфигурации (чтобы корректно загружать appsettings.context.json)
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"📂 Путь к файлу конфигурации: {basePath}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.context.json", optional: false, reloadOnChange: true)
                .Build();

            // Получаем строку подключения
            var connStr = configuration.GetConnectionString(provider);
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception($"❌ Ошибка: Строка подключения для провайдера '{provider}' не найдена в appsettings.context.json");
            }
            Console.WriteLine($"🔗 Используемая строка подключения: {connStr}");

            // Определяем тип базы данных
            DbType dbType;
            if (provider == "pgsql")
                dbType = DbType.PgSql;
            else
                throw new Exception($"❌ Ошибка: Неподдерживаемый провайдер '{provider}'");

            // Включаем детальное логирование в EF Core
            var options = DbContextOptionsFactory.Create(connStr, dbType, detailedLogging: true);

            Console.WriteLine("✅ DbContext успешно создан!");
            return new MainDbContext(options);
        }
    }
}
