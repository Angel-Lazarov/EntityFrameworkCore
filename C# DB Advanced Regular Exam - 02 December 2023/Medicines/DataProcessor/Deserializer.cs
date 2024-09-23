using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ImportDtos;
using Medicines.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new();
            var deserializedDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

            List<Patient> patientsToImport = new List<Patient>();

            foreach (ImportPatientDto patientDto in deserializedDtos)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient newPatient = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender,
                };

                foreach (int medicineId in patientDto.Medicines)
                {
                    if (newPatient.PatientsMedicines.Any(pm => pm.MedicineId == medicineId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine newPatientMedicine = new PatientMedicine()
                    {
                        MedicineId = medicineId
                    };

                    newPatient.PatientsMedicines.Add(newPatientMedicine);
                }

                patientsToImport.Add(newPatient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, newPatient.FullName,
                    newPatient.PatientsMedicines.Count));
            }

            context.Patients.AddRange(patientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper helper = new XmlHelper();

            List<Pharmacy> pharmasiesToImport = new List<Pharmacy>();

            var deserializedDtos = helper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            foreach (ImportPharmacyDto pDto in deserializedDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isNonStop = bool.TryParse(pDto.NonStop, out bool result);
                if (!isNonStop)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy newPharmacy = new Pharmacy()
                {
                    IsNonStop = isNonStop,
                    Name = pDto.Name,
                    PhoneNumber = pDto.PhoneNumber
                };

                foreach (ImportMedicineDto mDto in pDto.Medicines)
                {
                    if (!IsValid(mDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValiProductionDate = DateTime.TryParseExact(mDto.ProductionDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ProductionDate);

                    bool isValidExpiryDate = DateTime.TryParseExact(mDto.ExpiryDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ExpityDate);

                    if (ProductionDate >= ExpityDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine newMedicine = new Medicine()
                    {
                        Category = (Category)mDto.Category,
                        Name = mDto.Name,
                        Price = mDto.Price,
                        ProductionDate = ProductionDate,
                        ExpiryDate = ExpityDate,
                        Producer = mDto.Producer
                    };

                    if (newPharmacy.Medicines.Any(m => m.Name == newMedicine.Name && m.Producer == newMedicine.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    newPharmacy.Medicines.Add(newMedicine);
                }

                pharmasiesToImport.Add(newPharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, newPharmacy.Name, newPharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(pharmasiesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
