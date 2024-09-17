//DESKTOP - 2P8AV3C\SQLEXPRESS ,1434

using System.Data.SqlClient;

//string connectionString = @"Server=DESKTOP-2P8AV3C\SQLEXPRESS ,1434; Database = SoftUni; User Id = sa; Password = password;";
//string connectionString = @"Server=DESKTOP-2P8AV3C\SQLEXPRESS ,1434; Database = SoftUni; User Id = sa; Password = password; Trusted_Connection=True;";
string connectionString = "Server=192.168.88.40 ,1434; Database = SoftUni; User Id = sa; Password = password;";
string query = "SELECT EmployeeID, FirstName, LastName, JobTitle FROM Employees WHERE DepartmentID = @departmentID";
int departmentId = 3;

using SqlConnection connection = new SqlConnection(connectionString);

SqlCommand cmd = new SqlCommand(query, connection);
cmd.Parameters.AddWithValue("@departmentId", departmentId);
try
{
    connection.Open();
    SqlDataReader reader = await cmd.ExecuteReaderAsync();

    while (reader.Read())
    {
        Console.WriteLine($"{reader[1]} {reader[2]}: {reader[3]}");
    }    
}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}