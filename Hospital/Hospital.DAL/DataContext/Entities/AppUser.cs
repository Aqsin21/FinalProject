using Microsoft.AspNetCore.Identity;
namespace Hospital.DAL.DataContext.Entities
{
    public class AppUser:IdentityUser
    {
        public ICollection<Favorite>? FavoriteDoctors { get; set; } = new List<Favorite>();
    }
}
