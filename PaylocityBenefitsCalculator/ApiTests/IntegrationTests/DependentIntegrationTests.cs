using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
    // TODO: setup the mock with the use of some mocking library
    private List<GetDependentDto> _dependents = new List<GetDependentDto>
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        };

    [Fact]
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        await response.ShouldReturn(HttpStatusCode.OK, _dependents);
    }

    [Fact]
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    // Additional test for validating invalid input
    [Fact]
    public async Task WhenAskedForADependentWithInvalidId_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/not-a-number");
        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }

    // Additional test for validating invalid input
    [Fact]
    public async Task WhenAskedForAInvalidDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{(long)(int.MaxValue) + 1}");
        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }
}
