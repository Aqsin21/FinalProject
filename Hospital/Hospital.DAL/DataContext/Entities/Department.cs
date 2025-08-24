namespace Hospital.DAL.DataContext.Entities
{
    public class Department:BaseEntity
    {
        public required String Name { get; set; }
        public required String Description { get; set; }
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
