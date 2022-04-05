using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace UV_Backend.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public Profile Profile { get; set; }
    }
}
