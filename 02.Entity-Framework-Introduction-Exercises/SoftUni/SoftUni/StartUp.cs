using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            //Console.WriteLine(GetEmployeesFullInformation(context));
            Console.WriteLine(GetLatestProjects(context));
        }

        //3.	Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            //return string.Join(Environment.NewLine, context.Employees
            //    .Select(e=> $"{e.FirstName} {e.LastName} {e.JobTitle} {e.Salary:f2}").ToList());

            var employees = context.Employees.
                OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    Salary = e.Salary.ToString("F2")
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.JobTitle} {employee.Salary:f2}");
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
                ;

            var sb = new StringBuilder();
            foreach (var p in result.OrderBy(p => p.Name))
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return sb.ToString().TrimEnd();

        }
    }
}
//string connectionString = "Server=192.168.88.40 ,1434; Database = SoftUni; User Id = sa; Password = password;";

