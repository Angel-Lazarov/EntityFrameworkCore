using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ExportDtos;
using Medicines.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Medicines.DataProcessor
{
    using Medicines.Data;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            // string date = "2022-01-01";
            DateTime target = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var patients = context.Patients
                .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > target))
                .Select(p => new ExportPatientDto()
                {
                    Gender = p.Gender.ToString().ToLower(),
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Medicines = p.PatientsMedicines
                        .Where(pm => pm.Medicine.ProductionDate > target)
                        .OrderByDescending(m => m.Medicine.ExpiryDate)
                        .ThenBy(m => m.Medicine.Price)
                        .Select(pm => new ExportMedicineDto
                        {
                            Category = pm.Medicine.Category.ToString().ToLower(),
                            Name = pm.Medicine.Name,
                            Price = pm.Medicine.Price.ToString("f2"),
                            Producer = pm.Medicine.Producer,
                            BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        })
                        .ToArray()
                })
                .OrderByDescending(p => p.Medicines.Length)
                .ThenBy(m => m.Name)
                .ToArray();

            XmlHelper helper = new XmlHelper();
            StringBuilder sb = new();

            return helper.Serialize<ExportPatientDto[]>(patients, "Patients");

        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop == true)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = m.Price.ToString(),
                    Pharmacy = new
                    {
                        m.Pharmacy.Name,
                        m.Pharmacy.PhoneNumber
                    }
                })
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .ToArray();

            return JsonConvert.SerializeObject(medicines, Formatting.Indented);
        }
    }
}
