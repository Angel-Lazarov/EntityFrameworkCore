using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            //string connectionString = "Server =.;Database = SoftUni;User Id = sa;Password = SoftUn!2021; TrustServerCertificate=True;";

            SoftUniContext context = new SoftUniContext();
            // Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployee(context));
            //Console.WriteLine(GetEmployeesInPeriod(context));
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));
            //Console.WriteLine(IncreaseSalaries(context));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context));
            //Console.WriteLine(DeleteProjectById(context));
            //Console.WriteLine(RemoveTown(context));
        }

        // 03. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new { e.FirstName, e.LastName, e.MiddleName, e.JobTitle, e.Salary })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();

            //return string.Join(Environment.NewLine,
            //    context.Employees.Select(e => $"{e.FirstName}, {e.LastName}, {e.MiddleName}, {e.JobTitle}, {e.Salary}")
            //        .ToList());

        }

        // 04. Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var richEmployees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var re in richEmployees)
            {
                sb.AppendLine($"{re.FirstName} - {re.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 05. Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var researchers = context.Employees
                .Select(e => new { e.FirstName, e.LastName, e.Department, e.Salary })
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var r in researchers)
            {
                sb.AppendLine($"{r.FirstName} {r.LastName} from {r.Department.Name} - ${r.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 06. Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var targetEmployee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            if (targetEmployee != null)
            {
                targetEmployee.Address = newAddress;
                context.SaveChanges();
            }

            List<string> employes = context.Employees
                 .OrderByDescending(e => e.AddressId)
                 .Take(10)
                 .Select(e => e.Address.AddressText)
                 .ToList();

            return string.Join(Environment.NewLine, employes);
        }

        // 07. Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var result = context.Employees
                .Take(10)
                .Select(e => new
                {
                    EmployeeNames = $"{e.FirstName} {e.LastName}",
                    ManagerNames = $"{e.Manager.FirstName} {e.Manager.LastName}",
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            ep.Project.StartDate,
                            EndDate = ep.Project.EndDate.HasValue ?
                                ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"
                        })
                });

            StringBuilder sb = new StringBuilder();

            foreach (var e in result)
            {
                sb.AppendLine($"{e.EmployeeNames} - Manager: {e.ManagerNames}");
                if (e.Projects.Any())
                {
                    foreach (var p in e.Projects)
                    {
                        sb.AppendLine($"--{p.ProjectName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - {p.EndDate}");
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }

        // 08. Addresses by Town

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var result = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    PeoplesCount = a.Employees.Count
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var r in result)
            {
                sb.AppendLine($"{r.AddressText}, {r.TownName} - {r.PeoplesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        // 09. Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var target = context.Employees.Find(147);

            var targetProjectsNames = context.EmployeesProjects
                .Where(e => e.EmployeeId == 147)
                .Select(ep => new
                {
                    ep.Project.Name
                })
                .OrderBy(p => p.Name)
                .ToList();


            sb.AppendLine($"{target.FirstName} {target.LastName} - {target.JobTitle}");

            foreach (var p in targetProjectsNames)
            {
                sb.AppendLine($"{p.Name}");
            }

            //StringBuilder sb = new StringBuilder();
            //var employee = context.Employees
            //    .Where(e => e.EmployeeId == 147)
            //    .Select(e => new
            //    {
            //        e.FirstName,
            //        e.LastName,
            //        e.JobTitle,
            //        Projects = e.EmployeesProjects
            //            .Select(p => new
            //            {
            //                p.Project.Name
            //            })
            //            .OrderBy(p => p.Name)
            //            .ToArray()
            //    })
            //    .ToList();

            // foreach (var e in employee)
            // {
            //     sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            //     foreach (var p in e.Projects)
            //     {
            //         sb.AppendLine($"{p.Name}");
            //     }
            // }

            return sb.ToString().TrimEnd();
        }

        // 10. Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {

            var result = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerNames = $"{d.Manager.FirstName} {d.Manager.LastName}",
                    //d.Employees,
                    Emplo = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName).ToList()
                }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var r in result)
            {
                sb.AppendLine($"{r.DepartmentName} - {r.ManagerNames}");

                foreach (var e in r.Emplo)
                {
                    sb.AppendLine($"{e.FirstName} - {e.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //11.	Find Latest 10 Projects

        public static string GetLatestProjects(SoftUniContext context)
        {
            var result = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();
            foreach (var r in result.OrderBy(p => p.Name))
            {
                sb.AppendLine($"{r.Name}");
                sb.AppendLine($"{r.Description}");
                sb.AppendLine($"{r.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return sb.ToString().TrimEnd();

        }

        //12.	Increase Salaries

        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] targetedDepartments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var promoted = context.Employees
                .Where(e => targetedDepartments.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var pe in promoted)
            {
                pe.Salary *= 1.12m;
                sb.AppendLine($"{pe.FirstName} {pe.LastName} (${pe.Salary:f2})");
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        //13.	Find Employees by First Name Starting with "Sa"

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var result = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var e in result)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //14.	Delete Project by Id

        public static string DeleteProjectById(SoftUniContext context)
        {
            var empToDelete = context.EmployeesProjects
                .Where(e => e.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(empToDelete);

            var projectToDelete = context.Projects
                .Find(2);

            context.Projects.Remove(projectToDelete);

            var result = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var p in result)
            {
                sb.AppendLine(p);
            }

            return sb.ToString().TrimEnd();
        }

        //15.	Remove Town

        public static string RemoveTown(SoftUniContext context)
        {

            foreach (var employee in context.Employees.Where(e => e.Address.Town.Name == "Seattle"))
            {
                employee.AddressId = null;
            }

            var addressesToDelete = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToList();

            context.Addresses.RemoveRange(addressesToDelete);

            var townToDelete = context.Towns.First(t => t.Name == "Seattle");
            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            string result = $"{addressesToDelete.Count} addresses in Seattle were deleted";

            if (addressesToDelete.Count <= 1)
            {
                result = $"{addressesToDelete.Count} address in Seattle was deleted";
            }

            return result;
        }

    }
}
