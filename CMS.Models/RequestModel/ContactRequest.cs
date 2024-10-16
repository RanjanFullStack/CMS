using System.ComponentModel.DataAnnotations;

namespace CMS.Models.RequestModel
{
    public class ContactRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
