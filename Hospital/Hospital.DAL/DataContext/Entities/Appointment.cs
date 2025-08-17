namespace Hospital.DAL.DataContext.Entities
{
      public class Appointment:BaseEntity
    { 
        public int PatientId { get; set; }
        public required Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
