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
             TypeAdapterConfig<DepartmentTranslationsRequest, DepartmentTranslations>
            .NewConfig();
            TypeAdapterConfig<DepartmentRequest, Department>.NewConfig()
            .Map(
                dest => dest.Translations,
                src => src.Translations
            );
                    TypeAdapterConfig<Department, DepartmentResponse>.NewConfig()
                      .Map(
                          dest => dest.UserCreated,
                          source => source.CreatedBy.UserName
                      )
                      .Map(
                          dest => dest.Name,
                          source => source.Translations
                              .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                              .Select(t => t.Name)
                              .FirstOrDefault()
                      )
                      .Map(
                          dest => dest.Description,
                          source => source.Translations
                              .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                              .Select(t => t.Description)
                              .FirstOrDefault())
                                 .Map(
                     dest => dest.ImageUrl,
                     src => $"https://localhost:7042/images/{src.ImageUrl}"
                 );

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
            TypeAdapterConfig<LabResult, LabResultResponse>.NewConfig()

            .Map(dest => dest.UserCreated,
                 src => src.CreatedBy.UserName)

            .Map(dest => dest.TestName,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.TestName)
                 .FirstOrDefault() ?? "")

            .Map(dest => dest.Result,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.Result)
                 .FirstOrDefault() ?? "")

            .Map(dest => dest.Notes,
                 src => src.Translations
                 .Where(x => x.Language == CultureInfo.CurrentCulture.Name)
                 .Select(x => x.Notes)
                 .FirstOrDefault() ?? "");
            TypeAdapterConfig<DoctorSchedule, DoctorScheduleResponse>
            .NewConfig()
            .Map(
             dest => dest.UserCreated,
             src => src.CreatedBy.UserName
            );
            TypeAdapterConfig<DoctorRating, DoctorRatingResponse>
            .NewConfig()
            .Map(
                dest => dest.PatientName,
                src => src.Patient.User.FullName
            );
            TypeAdapterConfig<Patient, PatientDetailsResponse>
            .NewConfig()
            .Map(
                dest => dest.FullName,
                src => src.User.FullName
            )
            .Map(
                dest => dest.Email,
                src => src.User.Email
            )
            .Map(
                dest => dest.PhoneNumber,
                src => src.User.PhoneNumber
            );
            TypeAdapterConfig<Doctor, DoctorResponse>
            .NewConfig()
            .Map(
                dest => dest.FullName,
                src => src.User.FullName
            )
            .Map(
                dest => dest.Email,
                src => src.User.Email
            )
            .Map(
                dest => dest.PhoneNumber,
                src => src.User.PhoneNumber
            )
            .Map(
                dest => dest.DepartmentName,
                src => src.Department.Translations
                    .Where(t =>
                        t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.Name)
                    .FirstOrDefault()
                    ?? "Default Department Name"
            );
            TypeAdapterConfig<Appointment, PatientAppointmentResponse>.NewConfig()
            .Map(dest => dest.DoctorName, src => src.Doctor.User.FullName)
            .Map(dest => dest.Specialty, src => src.Doctor.Specialty);
            TypeAdapterConfig<AppointmentBooking, BookingResponse>.NewConfig()
            .Map(dest => dest.PatientName,
                src => src.Patient.User.FullName)
            .Map(dest => dest.DoctorName,
                src => src.Doctor.User.FullName);
            TypeAdapterConfig<MedicalRecord, PatientMedicalRecordResponse>
            .NewConfig()
            .Map(
                dest => dest.DoctorName,
                src => src.Doctor.User.FullName
            )
            .Map(
                dest => dest.Diagnosis,
                src => src.Translations
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.Diagnosis)
                    .FirstOrDefault()
            )
            .Map(
                dest => dest.Notes,
                src => src.Translations
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.Notes)
                    .FirstOrDefault()
            );
            TypeAdapterConfig<Prescription, PrescriptionResponse>
            .NewConfig()
    
            .Map(dest => dest.MedicationName,
                src => src.Translations
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.MedicationName)
                    .FirstOrDefault())
            .Map(dest => dest.Dosage,
                src => src.Translations
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.Dosage)
                    .FirstOrDefault())
            .Map(dest => dest.Instructions,
                src => src.Translations
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.Instructions)
                    .FirstOrDefault());
            TypeAdapterConfig<Bill, BillResponse>.NewConfig()
            .Map(
                dest => dest.PatientName,
                src => src.Patient.User.FullName
            );

        }


    }
}
