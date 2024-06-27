using System.Collections.Generic;
using System.Linq;

namespace SampleCachingApp.Services
{
    public class EmployeeService
    {
        private readonly SampleCachingAppContext _sampleCachingAppContext;

        public EmployeeService(SampleCachingAppContext sampleCachingAppContext)
        {
            _sampleCachingAppContext = sampleCachingAppContext;
        }

        public List<Employee> GetEmployees(Dictionary<string, string> filters, int pageNo, int pageSize, string sortProperty, bool ascendingSort)
        {
            // Get employees from database based on provided parameters
            var employeesQueryable = _sampleCachingAppContext.Employee.AsQueryable();

            // Apply filters
            foreach (var filter in filters)
            {
                switch (filter.Key.ToLower())
                {
                    case "name":
                        employeesQueryable = employeesQueryable.Where(x => x.Name.Contains(filter.Value));
                        break;
                    case "designation":
                        employeesQueryable = employeesQueryable.Where(x => x.Designation.Contains(filter.Value));
                        break;
                        // Add more cases for other filterable properties as needed
                }
            }

            // Apply sorting
            switch (sortProperty.ToLower())
            {
                case "name":
                    employeesQueryable = ascendingSort ? employeesQueryable.OrderBy(x => x.Name) : employeesQueryable.OrderByDescending(x => x.Name);
                    break;
                case "designation":
                    employeesQueryable = ascendingSort ? employeesQueryable.OrderBy(x => x.Designation) : employeesQueryable.OrderByDescending(x => x.Designation);
                    break;
                case "id":
                default:
                    employeesQueryable = ascendingSort ? employeesQueryable.OrderBy(x => x.Id) : employeesQueryable.OrderByDescending(x => x.Id);
                    break;
            }

            // Apply paging
            employeesQueryable = employeesQueryable.Skip(pageNo * pageSize).Take(pageSize);

            return employeesQueryable.ToList();
        }
    }
}
