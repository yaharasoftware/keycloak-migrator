using Autofac;
using Keycloak.Migrator.DataService.Test.Fixtures;
using Keycloak.Migrator.DataServices;
using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;

namespace Keycloak.Migrator.DataService.Test
{
    public class TestRealmExportParser : IClassFixture<DataServiceFixture>
    {
        private readonly DataServiceFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRealmExportParser"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        public TestRealmExportParser(DataServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task TestParseRealm()
        {
            FileInfo fileInfo = new FileInfo(".\\realm-export.json");

            var realmDataParseJson = _fixture.Container.Resolve<IRealmDataParser>();

            RealmExport? realmExport = await realmDataParseJson.ParseRealmExport(fileInfo);

        }
    }
}