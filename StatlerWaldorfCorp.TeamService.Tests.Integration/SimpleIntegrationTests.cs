using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using StatlerWaldorfCorp.TeamService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace StatlerWaldorfCorp.TeamService.Tests.Integration
{
    public class SimpleIntegrationTests
    {

        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        private readonly Team teamZombie;

        public SimpleIntegrationTests()
        {
            testServer = new TestServer(new WebHostBuilder()
                    .UseStartup<Startup>());
            testClient = testServer.CreateClient();

            teamZombie = new Team()
            {
                ID = Guid.NewGuid(),
                Name = "Zombie"
            };
        }



        [Fact]
        public async System.Threading.Tasks.Task TestTeamPostAndGetAsync()
        {

            StringContent stringContent = new StringContent(
                JsonConvert.SerializeObject(teamZombie),
                UnicodeEncoding.UTF8,
                "application/json");

            // Act
            HttpResponseMessage postResponse = await testClient.PostAsync(
                "/teams",
                stringContent);
            postResponse.EnsureSuccessStatusCode();

            var getResponse = await testClient.GetAsync("/teams");
            getResponse.EnsureSuccessStatusCode();

            string raw = await getResponse.Content.ReadAsStringAsync();
            List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(raw);
            Assert.Equal(1, teams.Count());
            Assert.Equal("Zombie", teams[0].Name);
            Assert.Equal(teamZombie.ID, teams[0].ID);

        }
    }
}
