using Hospital.DAL.DataContext.Entities;

namespace Hospital.UI.Models
{
    public class DoctorListViewModel
    {
        public List<Doctor> Doctors { get; set; }
        public List<int> UserFavoriteDoctorIds { get; set; } = new List<int>();
    }
}
