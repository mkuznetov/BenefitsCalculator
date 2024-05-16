using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class PaycheckIntegrationTests : IntegrationTest
{
    // TODO: use the DI instead of explicit creation
    private IBenefitsCalculator calculator = new BenefitsCalculator();

    // TODO: setup the mock with the use of some mocking library
    private List<GetEmployeeDto> _employees = new List<GetEmployeeDto>
        {
            new ()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new ()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new ()
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
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };

    [Fact]
    public async Task PaycheckOfEveryEmployee1_ShouldReturnCorrectAmount()
    {
        foreach (var employee in _employees)
        {
            var response = await HttpClient.GetAsync($"/api/v1/paycheck/{employee.Id}");
            var paycheck = calculator.CalculatePaycheckForEmployee(employee);
            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }
    }

    // Additional test for validating invalid input
    [Fact]
    public async Task WhenAskedForAPaycheckForNonExistingEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/paycheck/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    // Additional test for validating invalid input
    [Fact]
    public async Task WhenAskedForAPaycheckWithInvalidEmployeeId_ShouldReturn400()
    {
        var response = await HttpClient.GetAsync($"/api/v1/paycheck/not-a-number");
        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }

    // Additional test for validating invalid input
    [Fact]
    public async Task WhenAskedForAPaycheckForEmployeeWithIdExceedingMaximum_ShouldReturn400()
    {
        var response = await HttpClient.GetAsync($"/api/v1/paycheck/{(long)(int.MaxValue) + 1}");
        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }
}
