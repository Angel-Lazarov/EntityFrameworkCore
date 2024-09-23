using Microsoft.Data.SqlClient;

namespace Ado.Net_Demo;

public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server =.;Database = SoftUni;User Id = sa;Password = SoftUn!2021; TrustServerCertificate=True;";
            //string connectionString = "Server =.;Database = SoftUni;User Id = sa;Password = SoftUn!2021;Trusted_Connection=True;";
            string query = "SELECT EmployeeID, FirstName, LastName, JobTitle FROM Employees WHERE DepartmentID = @departmentId";
            int departmentId = 7;

            using SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@departmentId", departmentId);

            try
            {
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                Console.WriteLine($"{reader[1]}{reader[2]}: {reader[3]}");

            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
    }

