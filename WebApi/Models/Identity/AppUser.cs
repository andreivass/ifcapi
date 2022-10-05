using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Identity
{
    /// <summary>
    /// App user model.
    /// </summary>
    public class AppUser : IdentityUser
    {
        [MinLength(1)]
        [MaxLength(128)]
        public string? FirstName { get; set; }

        [MinLength(1)]
        [MaxLength(128)]
        public string? LastName { get; set; }

        public ICollection<Project>? Projects { get; set; }
    }
}
