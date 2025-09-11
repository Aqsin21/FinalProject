using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.DAL.DataContext.Entities
{
    public class Appointment : BaseEntity
    {
        // Hasta bilgileri
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        // Department
        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public required Department Department { get; set; }

        // Doctor
        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public required Doctor Doctor { get; set; }

        // Randevu tarihi ve saati
        [Required]
        public DateTime AppointmentDate { get; set; }

        public bool? Paid { get; set; }
  
    }
}
