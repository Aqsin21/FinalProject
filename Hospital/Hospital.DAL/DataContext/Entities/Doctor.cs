namespace Hospital.DAL.DataContext.Entities
{
    public class Doctor : BaseEntity
    {
        public required string FullName { get; set; }
        public int? DepartmentId { get; set; }
        public  Department? Department { get; set; }
        public required string Description { get; set; }
        public  string? ImagePath { get; set; }
        public decimal? Price { get; set; }

    }
}
