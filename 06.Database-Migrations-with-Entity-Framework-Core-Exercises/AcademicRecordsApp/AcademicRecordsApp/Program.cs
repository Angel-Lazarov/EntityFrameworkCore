using AcademicRecordsApp.Data.Models;
using System.Text.Json;

namespace AcademicRecordsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Student student = new Student()
            {
                FullName = "Ange Lazarov",
                Age = 40,
                Jobtitle = "Celler",
                Celary = 5000
            };

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            string info = JsonSerializer.Serialize(student, options);
            //var fullInfo = JsonSerializer.Serialize(student);
            //Console.WriteLine(fullInfo);
            // Console.WriteLine(info);

            Student studentCopy = JsonSerializer.Deserialize<Student>(info);


            File.WriteAllText("../../../moeto.json", info);

            string jsonText = File.ReadAllText("../../../moeto.json");
            Student convertedStudent = JsonSerializer.Deserialize<Student>(jsonText);

            Console.WriteLine($"Tova e prez file -> {convertedStudent.FullName} {convertedStudent.Age} {convertedStudent.Jobtitle} {studentCopy.Celary}");

        }
    }
}
//string connectionString = "Server=192.168.88.40 ,1434; Database = FootballBookmakerSystem; User Id = sa; Password = password;";