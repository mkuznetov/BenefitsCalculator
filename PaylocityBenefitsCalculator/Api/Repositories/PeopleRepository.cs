using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Repositories
{
    // Common repository for both employees and dependents
    public class PeopleRepository : IPeopleRepository
    {
        // For production there must be a persistent layer (DB) which may enforce
        // some of the business requirements (unique IDs, not more than 1 spouse / partner)
        // and also the repository must ensure the concurrency safety (for CRUD operations)
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

        // Cache of employees and dependents
        private Dictionary<int, GetEmployeeDto> _idsToEmployees = new Dictionary<int, GetEmployeeDto>();
        private Dictionary<int, GetDependentDto> _idsToDependents = new Dictionary<int, GetDependentDto>();

        public PeopleRepository()
        {
            // In production, cached employees and the dependents probably have to be lazily populated
            foreach (var employee in _employees)
            {
                // Validate the employee fits the business requirements:
                //   - no more than 1 spouse or domestic partner
                if (employee.Dependents.Count(
                    d => d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner)
                    > 1)
                {
                    // Log the error and skip
                    continue;
                }
                // All employees must have unique IDs
                if (!_idsToEmployees.TryAdd(employee.Id, employee))
                {
                    // Log the error and skip
                }
                // Assuming all dependents must have unique IDs
                foreach (var dependent in employee.Dependents)
                {
                    if (!_idsToDependents.TryAdd(dependent.Id, dependent))
                    {
                        // Log the error and skip
                    }
                }
            }
        }

        //public GetEmployeeDto GetEmployeeById(int id) => _employees.FirstOrDefault(e => e.Id == id);
        public GetEmployeeDto GetEmployeeById(int id) => _idsToEmployees.TryGetValue(id, out var employee) ? employee : null;

        //public List<GetEmployeeDto> GetEmployees() => _employees;
        public List<GetEmployeeDto> GetEmployees() => _idsToEmployees.Values.ToList();

        public GetDependentDto GetDependentById(int id) => _idsToDependents.TryGetValue(id, out var dependent) ? dependent : null;

        public List<GetDependentDto> GetDependents() => _idsToDependents.Values.ToList();
    }
}
