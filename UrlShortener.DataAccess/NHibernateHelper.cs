using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using UrlShortener.DataAccess.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UrlShortener.DataAccess
{
    public class NHibernateHelper 
    {
        private static ISessionFactory _sessionFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<NHibernateHelper> _logger;

        public NHibernateHelper(IConfiguration configuration, ILogger<NHibernateHelper> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = CreateSessionFactory();
                }
                return _sessionFactory;
            }
        }

        private ISessionFactory CreateSessionFactory()
        {
            var server = _configuration["MySQLConfiguration:Server"];
            var databaseName = _configuration["MySQLConfiguration:Database"];
            var username = _configuration["MySQLConfiguration:Username"];
            var password = _configuration["MySQLConfiguration:Password"];

            _logger.LogInformation($"Connecting to MySQL: Server={server}, Database={databaseName}, Username={username}");

            EnsureDatabaseExists(server, username, password, databaseName);

            var configuration = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                    .ConnectionString(cs => cs
                        .Server(server)
                        .Database(databaseName)
                        .Username(username)
                        .Password(password)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UrlMap>());

            configuration.ExposeConfiguration(cfg =>
            {
                new SchemaUpdate(cfg).Execute(false, true);
            });

            return configuration.BuildSessionFactory();
        }

        private void EnsureDatabaseExists(string server, string username, string password, string databaseName)
        {
            try
            {
                _logger.LogInformation("Ensuring database {DatabaseName} exists", databaseName);
                var connectionStringWithoutDatabase = $"Server={server};Uid={username};Pwd={password};";

                using (var connection = new MySqlConnection(connectionStringWithoutDatabase))
                {
                    connection.Open();

                    using (var command = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {databaseName};", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                _logger.LogInformation("Database {DatabaseName} ensured successfully", databaseName);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Failed to ensure database {DatabaseName} exists", databaseName);
                throw;
            }
        }

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}