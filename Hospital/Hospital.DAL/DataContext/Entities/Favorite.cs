namespace Hospital.DAL.DataContext.Entities
{
    public  class Favorite:BaseEntity
    {
        public required string UserId { get; set; }
        public required AppUser User { get; set; }

        public int DoctorId { get; set; }
       
    }
}
