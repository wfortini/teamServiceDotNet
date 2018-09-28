using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.TeamService.Controllers;
using StatlerWaldorfCorp.TeamService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StatlerWaldorfCorp.TeamService.Tests
{
    public class TeamControllertest
    {
        

        [Fact]
        public void QueryTeamListReturnsCorrectTeams()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var rawTeams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> teams = new List<Team>(rawTeams);
            Assert.Equal(teams.Count, 2);
            Assert.Equal(teams[0].Name, "one");
            Assert.Equal(teams[1].Name, "two");
        }

        [Fact]
        public void CreateTeamAddsTeamToList()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Team t = new Team("sample");
            var result = controller.CreateTeam(t);
            //TODO: also assert that the destination URL of the new team reflects the team's GUID
            Assert.Equal((result as ObjectResult).StatusCode, 201);

            var actionResult = controller.GetAllTeams() as ObjectResult;
            var newTeamsRaw = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            Assert.Equal(newTeams.Count, original.Count + 1);
            var sampleTeam = newTeams.FirstOrDefault(target => target.Name == "sample");
            Assert.NotNull(sampleTeam);
        }
    }
}
