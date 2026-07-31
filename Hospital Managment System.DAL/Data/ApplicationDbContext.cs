using Hospital_Managment_System.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentTranslations> DepartmentsTranslations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<AppointmentTranslation> AppointmentTranslations { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        public DbSet<MedicalRecordTranslations> MedicalRecordTranslations { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        public DbSet<PrescriptionTranslations> PrescriptionTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //change table names
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            // Doctor -> ApplicationUser
            builder.Entity<Doctor>()
             .HasOne(d => d.User)
             .WithOne(u => u.Doctor)
             .HasForeignKey<Doctor>(d => d.UserId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Doctor>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Doctor>()
                .HasOne(d => d.UpdatedBy)
                .WithMany()
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient -> ApplicationUser
            builder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithOne(u => u.Patient)
            .HasForeignKey<Patient>(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Patient>()
               .HasOne(d => d.CreatedBy)
               .WithMany()
               .HasForeignKey(d => d.CreatedById)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Patient>()
                .HasOne(d => d.UpdatedBy)
                .WithMany()
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);
            //Appointment
            builder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Appointment>()
                .HasMany(a => a.Translations)
                .WithOne(t => t.Appointment)
                .HasForeignKey(t => t.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Appointment>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Appointment>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);
            //medical
            builder.Entity<MedicalRecord>()
            .HasOne(x => x.Patient)
            .WithMany(x => x.MedicalRecords)
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MedicalRecord>()
                .HasOne(x => x.Doctor)
                .WithMany(x => x.MedicalRecords)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MedicalRecord>()
                .HasOne(x => x.Appointment)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey<MedicalRecord>(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MedicalRecord>()
                .HasMany(x => x.Translations)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey(x => x.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MedicalRecord>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MedicalRecord>()
                .HasOne(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);
            //Prescription
            builder.Entity<Prescription>()
                .HasOne(x => x.MedicalRecord)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.MedicalRecordId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Prescription>()
                .HasMany(x => x.Translations)
                .WithOne(x => x.Prescription)
                .HasForeignKey(x => x.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Prescription>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Prescription>()
                .HasOne(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var entries = ChangeTracker.Entries<AuditableEntity>();

                var currentUserId = _httpContextAccessor.HttpContext.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                        entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                        entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                    }
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
