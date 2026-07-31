using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Department, DepartmentResponse>.NewConfig()
              .Map(destniation => destniation.UserCreated, source => source.CreatedBy.UserName)
              .Map(dest => dest.Name, source => source.Translations
              .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
              .Select(t => t.Name).FirstOrDefault() ?? "Default Department Name");
            TypeAdapterConfig<Appointment, AppointmentResponse>.NewConfig()

            .Map(dest => dest.UserCreated,
                 src => src.CreatedBy.UserName)

            .Map(dest => dest.DoctorName,
                 src => src.Doctor.User.FullName)

            .Map(dest => dest.PatientName,
                 src => src.Patient.User.FullName)

            .Map(dest => dest.Status,
                 src => src.Status.ToString())

            .Map(dest => dest.Notes,
                 src => src.Translations
                        .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                        .Select(t => t.Notes)
                        .FirstOrDefault() ?? "");
            TypeAdapterConfig<MedicalRecord, MedicalRecordResponse>.NewConfig()

            .Map(dest => dest.UserCreated,
                 src => src.CreatedBy.UserName)

            .Map(dest => dest.DoctorName,
                 src => src.Doctor.User.FullName)

            .Map(dest => dest.PatientName,
                 src => src.Patient.User.FullName)

            .Map(dest => dest.Diagnosis,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.Diagnosis)
                 .FirstOrDefault() ?? "")

            .Map(dest => dest.Notes,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.Notes)
                 .FirstOrDefault() ?? "");
            TypeAdapterConfig<Prescription, PrescriptionResponse>.NewConfig()

            .Map(dest => dest.UserCreated,
                 src => src.CreatedBy.UserName)

            .Map(dest => dest.MedicationName,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.MedicationName)
                 .FirstOrDefault() ?? "")

            .Map(dest => dest.Dosage,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.Dosage)
                 .FirstOrDefault() ?? "")

            .Map(dest => dest.Instructions,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.Instructions)
                 .FirstOrDefault() ?? "");
        }
        
    }
}
